using System;
using System.Security.Cryptography;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    // The amount of time the bullet should stay alive
    public float lifetime;

    // The rigidbody component attached to the bullet game object
    private Rigidbody _rb;
    
    // The speed at which the bullet moves
    public float speed;
    public float damage;

    private Vector3 _mousePosition;


    private void Start() {
        _mousePosition = CursorController.Instance.cursorPosition;
        // Get the rigidbody component
        _rb = GetComponent<Rigidbody>();
        
        // Destroy the bullet after the lifetime
        Destroy(gameObject, lifetime);
    }

    private void Awake() {
        _mousePosition = CursorController.Instance.cursorPosition;

    }

    private void Update() {
        transform.Translate(Vector3.up * (speed * Time.deltaTime));
        Vector3 transformPosition = transform.position;
        transformPosition.y = 1.1f;
        transform.position = transformPosition;
    }

    private void FixedUpdate() {
        // Move the bullet in the direction it_transform
        _rb.MovePosition(transform.position + transform.forward * (speed * Time.deltaTime));
         
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.gameObject.tag) {
            case "Enemy":
                HealthManager healthManager = other.gameObject.TryGetComponent(out HealthManager hManager)
                    ? hManager
                    : other.gameObject.GetComponentInParent<HealthManager>();
                
                healthManager.TakeDamage(damage);

                // the direction from the bullet to the enemy
                Vector3 direction = healthManager.gameObject.transform.position - transform.position;

                // Normalize the direction vector to remove any scaling
                direction = direction.normalized;
                // Apply the force to the enemy's rigidbody in the direction of the enemy
                other.attachedRigidbody.AddForce(direction*2f, ForceMode.Impulse);
                break;
            case "Player":
                return;
            case "Weapon":
                return;
            case "Bullet":
                return;
            
        }

        Destroy(gameObject);
    }
}