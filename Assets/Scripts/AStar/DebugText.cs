using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField] private RectTransform arrow;

    [SerializeField] private TextMeshProUGUI f, g, h, p;

    public RectTransform MyArrow {get => arrow; set => arrow = value;}
    public TextMeshProUGUI F {get => f; set => F = value;}
    public TextMeshProUGUI G {get => g; set => h = value;}
    public TextMeshProUGUI H {get => h; set => g = value;}
    public TextMeshProUGUI P {get => p; set => p = value;}
}
