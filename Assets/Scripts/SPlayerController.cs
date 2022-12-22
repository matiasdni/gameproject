using System;
using System.Collections;
using UnityEngine;

public class SPlayerController : MonoBehaviour
{
    // player speed
    public float speed;

    // player weapon
    private WeaponController _currentWeapon;

    public EquippedWeaponManager weaponManager;

    [SerializeField] private HealthManager healthManager;

    public float maxHealth = 5f;

    // player rigidbody
    private Rigidbody _rb;

    // main camera
    private Camera _mainCamera;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
        _mainCamera = Camera.main;
        healthManager.SetHealth(maxHealth);
        // _currentWeapon = GetComponent<WeaponController>();
        _currentWeapon = GetComponentsInChildren<WeaponController>()[0];
        StartCoroutine(HandleWeaponSwitch());
    }

    private void Update() {
        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, speed);

        // Ray from the main camera to the mouse position
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        // Create a plane that is perpendicular to Vector3.up and passes through Vector3.zero
        var groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out var rayDistance)) {
            // Ray intersects the plane, obtain the point of intersection
            Vector3 point = ray.GetPoint(rayDistance);

            // Draw a line from the ray's origin to the point of intersection
            Debug.DrawLine(ray.origin, point, Color.red);

            // look at the point of intersection
            transform.LookAt(new Vector3(point.x, transform.position.y, point.z));
            transform.forward = new Vector3(transform.forward.x, 0, transform.forward.z);
            // CursorController.Instance.cursorPosition = new Vector3(point.x, 0.5f, point.z);
        }

        
        // Check if the left mouse button is being pressed or released, and set the property of a weapon object
        if (Input.GetMouseButtonDown(0)) _currentWeapon.isFiring = true;
        if (Input.GetMouseButtonUp(0)) _currentWeapon.isFiring = false;
    }

    private void FixedUpdate() {
        // get camera angle
        var cameraAngle = _mainCamera.transform.eulerAngles.y;
        // Get the horizontal and vertical input axes
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");

        var movement = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 moveDirection = Quaternion.Euler(0, cameraAngle, 0) * movement;
        
        
        // Apply the movement to the rigidbody
        _rb.AddForce(moveDirection.normalized * (speed * 2), ForceMode.Impulse);
        

    }

    private IEnumerator HandleWeaponSwitch() {
        while (true) {
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                _currentWeapon.isFiring = false;
               _currentWeapon = weaponManager.SwitchWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                _currentWeapon.isFiring = false;
                _currentWeapon = weaponManager.SwitchWeapon(1);
            }
            yield return null;
        }
    }
}