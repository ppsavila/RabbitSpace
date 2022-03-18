using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    private RectTransform _rect;
    private Canvas _canvas;

    private void Awake()
    {
        ApplySafeArea();
    }

    private void ApplySafeArea()
    {
        if (_rect == null)
            SetupRectTransform();
        Rect safeArea = Screen.safeArea;
        Rect pixelRect = _canvas.pixelRect;
        var anchorMin = safeArea.position;
        var anchorMax = safeArea.position + safeArea.size;
        anchorMin.x /= pixelRect.width;
        anchorMin.y /= pixelRect.height;
        anchorMax.x /= pixelRect.width;
        anchorMax.y /= pixelRect.height;

        _rect.anchorMin = anchorMin;
        _rect.anchorMax = anchorMax;
    }

    private void SetupRectTransform()
    {
        _rect = GetComponent<RectTransform>();
        _canvas = FindObjectOfType<Canvas>();
    }
}
