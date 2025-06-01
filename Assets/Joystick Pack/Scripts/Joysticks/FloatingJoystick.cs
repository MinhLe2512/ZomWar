using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    public static Action<Vector2> OnJoystickDrag;
    public float DefaultAlpha, DragAlpha;
    [SerializeField] CanvasGroup _canvasGroup;
    protected override void Start()
    {
        base.Start();
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = DefaultAlpha;
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        _canvasGroup.alpha = DragAlpha;
        base.OnPointerDown(eventData);
    }
    private void FixedUpdate()
    {
        if (_isPointerDown)
        {
            OnJoystickDrag?.Invoke(input);
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("OnDrag");
        base.OnDrag(eventData);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("OnPointerUp");
        _canvasGroup.alpha = DefaultAlpha;
        background.anchoredPosition = Vector2.zero;
        base.OnPointerUp(eventData);
        OnJoystickDrag?.Invoke(input);
    }
}