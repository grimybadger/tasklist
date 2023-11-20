using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Highlight : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [field: Header("General Properties")]
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Color DefaultColor {get; private set;}
    [field: SerializeField] public Color HighlightedColor { get; private set; }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Image.color = HighlightedColor;
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        Image.color = DefaultColor;
    }
}
