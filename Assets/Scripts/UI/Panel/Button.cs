using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ButtonPressEventArgs : EventArgs
{
    public PointerEventData PointerEventData { get; set; }
}
public enum ButtonType
{
    task, 
    timeElasped, 
    description, 
    time,
    exit
}
public class Button : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public ButtonType ButtonType { get; private set; }
    public static event EventHandler<ButtonPressEventArgs> ButtonPress;
    public static void RaiseButtonPress(object sender, ButtonPressEventArgs e)
    {
        var h = ButtonPress;
        h?.Invoke(sender, e);
    }
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        ButtonPressEventArgs args = new ButtonPressEventArgs
        {
            PointerEventData = pointerEventData
        };
        RaiseButtonPress(this, args);
    }

}
