using System;
using UnityEngine;
using TouchScript.Gestures;
using TouchScript.Gestures.TransformGestures;

public class JoyStickScript : MonoBehaviour
{
    Vector2 _startPos;
    public float _radius = 150f;
    bool _pressed;
    public Vector2 input;

    void OnEnable()
    {
        GetComponent<TapGesture>().Tapped += TappedHandle;

        GetComponent<ScreenTransformGesture>().TransformStarted += StartHandle;
        GetComponent<ScreenTransformGesture>().StateChanged += StateChangedHandle;
        GetComponent<ScreenTransformGesture>().TransformCompleted += TransformCompletedHandle;
        GetComponent<ScreenTransformGesture>().Cancelled += CancelledHandle;
    }

    void OnDisable()
    {
        UnsubscribeEvent();
    }

    void OnDestroy()
    {
        UnsubscribeEvent();
    }

    void UnsubscribeEvent()
    {
        GetComponent<TapGesture>().Tapped += TappedHandle;

        GetComponent<ScreenTransformGesture>().TransformStarted -= StartHandle;
        GetComponent<ScreenTransformGesture>().StateChanged -= StateChangedHandle;
        GetComponent<ScreenTransformGesture>().TransformCompleted -= TransformCompletedHandle;
        GetComponent<ScreenTransformGesture>().Cancelled -= CancelledHandle;
    }

    void TappedHandle(object sender, EventArgs e)
    {
        Debug.Log("Tapped");
    }

    void StartHandle(object sender, EventArgs e)
    {
        var gesture = sender as ScreenTransformGesture;
        _startPos = gesture.ScreenPosition;
        _pressed = true;
    }

    void StateChangedHandle(object sender, EventArgs e)
    {
        if (!_pressed)
        {
            input = Vector2.zero;
            return;
        }

        var gesture = sender as ScreenTransformGesture;
        Vector2 moveVector = gesture.ScreenPosition - _startPos;

        float range = _radius * _radius;
        if (moveVector.sqrMagnitude > range)
            input = moveVector.normalized;
        else
            input = moveVector / _radius;
    }

    void TransformCompletedHandle(object sender, EventArgs e)
    {
        DragEnd();
    }
    void CancelledHandle(object sender, EventArgs e)
    {
        DragEnd();
    }

    void DragEnd()
    {
        _pressed = false;
    }
}