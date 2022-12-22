using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController Instance;
    public Vector3 cursorPosition;
    
    public Texture2D cursorTexture;
    private Camera _camera;
    private bool _isCameraNotNull;
    private Vector2 _hotSpot;

    private void Awake()
    {
        Instance = this;
    }
    private void Start() {
        _hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        _isCameraNotNull = _camera != null;
        _camera = Camera.main;
        Cursor.SetCursor(cursorTexture, _hotSpot, CursorMode.Auto);
    }

    private void Update() {
        if (_isCameraNotNull) {
            cursorPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }
}
