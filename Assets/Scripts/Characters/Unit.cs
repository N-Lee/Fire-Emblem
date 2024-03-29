using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // Stats
    public int level, exp, maxHp, currentHp, might, skill, speed, luck, defense, resistance, constitution, move, terrain;
    public UnitClass unitClass;
    public bool isBoss;
    public CharacterInventory inventory;
    public Weapon equippedWeapon;
    Dictionary<WeaponType, int> weaponLevel;

    // Unity values
    float moveSpeed = 5f;
    Vector3 characterPosition;
    protected Vector3 characterOffset;
    public bool isDown, isLeft, isRight, isUp;
    public bool isCharacterMoving;
    int pathCount;
    List<Node> path;
    Animator animator;
    SpriteRenderer spriteRenderer;

    // NOTES: Weapon EXP: Get a certain amount based on weapon per attack (4 if the attacks twice in one turn). Start at 1 (E). 31 is D. 71 is C (+40). 121 is B. 181 is A. 251 is S 
    // Weapon rank bonus https://fireemblem.fandom.com/wiki/Weapon_Level

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isCharacterMoving = isDown = isLeft = isRight = isUp = false;
        inventory = new CharacterInventory(this);
    }

    protected virtual void Update()
    {
        animator.SetBool("isDown", isDown);
        animator.SetBool("isLeft", isLeft);
        animator.SetBool("isRight", isRight);
        animator.SetBool("isUp", isUp);

        if (isCharacterMoving)
        {
            Move();
        }
    }

    public void SetClass(UnitClass newClass, bool isPromotion)
    {
        if (!isPromotion)
        {
            unitClass = newClass;
            return;
        }

        foreach (KeyValuePair<WeaponType, int> promoteWeapon in unitClass.useableWeapon)
        {
            if (!weaponLevel.ContainsKey(promoteWeapon.Key))
            {
                weaponLevel.Add(promoteWeapon.Key, promoteWeapon.Value);
                break;
            }

            foreach (KeyValuePair<WeaponType, int> unitWeapon in weaponLevel)
            {
                if (promoteWeapon.Key == unitWeapon.Key && promoteWeapon.Value > unitWeapon.Value)
                {
                    weaponLevel[promoteWeapon.Key] = promoteWeapon.Value;
                }
            }
        }

        unitClass = newClass;
    }

    public int AttackDamage(int enemyDef, int enemyRes, int weaponTriangle)
    {
        int damage = 0;

        if (equippedWeapon.isPhysical)
        {
            damage = might - enemyDef;
        }
        else
        {
            damage = might - enemyRes;
        }

        return damage + weaponTriangle;
    }

    public int AttackSpeed(int enemySpeed)
    {
        return speed - enemySpeed >= 5 ? 2 : 1;
    }

    public double CritRate()
    {
        double crit = (skill/2 + equippedWeapon.critRate);

        if (crit > 100)
        {
            crit = 100;
        }
        else if (crit < 0)
        {
            crit = 0;
        }

        return crit;
    }

    double HitRate()
    {
        return ((skill * 2) + (luck * 0.5))  + equippedWeapon.hitRate;
    }

    public double AvoidRate()
    {
        return (speed * 2) + luck + terrain;
    }

    public double DisplayedHitRate()
    {
        double hit = HitRate() - AvoidRate();

        if (hit > 100)
        {
            hit = 100;
        }
        else if (hit < 0)
        {
            hit = 0;
        }

        return hit;
    }

    public int calculateWeaponEXP(int hitsLanded)
    {
        return hitsLanded * 2;
    }

    public int calculateEXP(Unit enemy, bool isKilled, bool hitLanded, int assassinateBonus)
    {
        int xp = 0;

        if (hitLanded)
        {
            xp += (31 + (enemy.level + enemy.unitClass.classBonusA) - (level + unitClass.classBonusA)) / unitClass.classPower;
        }
        else 
        {
            return 1;
        }

        if (isKilled)
        {
            int bossBonus = enemy.isBoss ? 40 : 0;
            int thiefBonus = unitClass.isThief ? 20 : 0;
            int baseKill = ((enemy.level * enemy.unitClass.classPower) + enemy.unitClass.classBonusB) - (((level * unitClass.classPower) + unitClass.classBonusB) / 1);

            if (baseKill < 1)
            {
                baseKill = ((enemy.level * enemy.unitClass.classPower) + enemy.unitClass.classBonusB) - (((level * unitClass.classPower) + unitClass.classBonusB) / 2);
            }
            
            xp += baseKill + 20 + bossBonus + thiefBonus;
        }

        return xp * assassinateBonus;
    }

    void Move()
    {
        Vector3 pathWithOffset = path[pathCount].Position + characterOffset;
        characterPosition = transform.position;

        if (characterPosition.x - pathWithOffset.x > 0)
        {
            spriteRenderer.flipX = true;
            isLeft = true;
            isDown = isRight = isUp = false;
        }
        if (characterPosition.x - pathWithOffset.x < 0)
        {
            isRight = true;
            isDown = isLeft = isUp = false;
        }
        if (characterPosition.y - pathWithOffset.y > 0)
        {
            isDown = true;
            isLeft = isRight = isUp = false;
        }
        if (characterPosition.y - pathWithOffset.y < 0)
        {
            isUp = true;
            isDown = isLeft = isRight = false;
        }

        transform.position = Vector3.MoveTowards(characterPosition, pathWithOffset, moveSpeed * Time.deltaTime);

        if (characterPosition == pathWithOffset && pathCount >= 0)
        {
            --pathCount;
        }

        if (pathCount < 0)
        {
            animator.Play("Idle", 0, AnimationSync.myInstance.GetAnimationFrame());
            spriteRenderer.flipX = false;
            isCharacterMoving = false;
            isDown = isLeft = isRight = isUp = false;
        }
    }

    public void SetPath(List<Node> path)
    {
        this.path = path;
        pathCount = path.Count - 1;
        isCharacterMoving = true;
    }

    public bool IsWeaponUseable(Weapon weapon)
    {
        if (unitClass.useableWeapon.ContainsKey(weapon.weaponType))
        {
            return true;
        }

        return false;
    }
}
