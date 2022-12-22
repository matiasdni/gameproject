using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 _offset;
    public Camera camera;
    private Vector3 _previousPosition;
    [SerializeField] private Transform target;
    public float rotationSpeed = 5.0f;
    public float smoothFactor = 0.5f;
    public static bool isRotating = false;
    private void Start() {
        _offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    private void LateUpdate() {
        if (Input.GetMouseButtonDown(1)) {
            _previousPosition = camera.ScreenToViewportPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1)) {
            isRotating = true;
            StartCoroutine(CheckInput());
            Vector3 direction = _previousPosition - camera.ScreenToViewportPoint(Input.mousePosition);
            camera.transform.position = target.position;
            camera.transform.Rotate(new Vector3(1,0,0), direction.y * 180);
            camera.transform.Rotate(new Vector3(0, 1,0), -direction.x * 180, Space.World);
            camera.transform.Translate(new Vector3(0,0, -10));
            _previousPosition = camera.ScreenToViewportPoint(Input.mousePosition);
           // _offset = transform.position - player.transform.position;
        } else {
            isRotating = false;
        }
        transform.position = player.transform.position + _offset;
    }

    private static IEnumerator CheckInput() {
        while (Input.GetMouseButton(1)) yield return null;
        isRotating = false;
    }
}
