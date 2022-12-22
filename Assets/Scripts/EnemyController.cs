using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyController : MonoBehaviour {
    // The target (player) that the enemies will try to reach
    public SPlayerController player;

    // The speed at which the enemies will move
    public float speed = 3f;
    
    // The rigidbody component of the enemy game object
    private Rigidbody _rb;

    private Transform _transform;

    public float damage;
    
    private HealthManager _healthManager;
    
    public float health;

    public NavMeshAgent agent;
    
    private void Start() {
        // Get the rigidbody component of the enemy game object
        _rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<SPlayerController>();
        _transform = transform;
        _healthManager = GetComponent<HealthManager>();
        agent.speed = speed;
    }

    private void Update() {
        _transform.LookAt(player.transform.position);
        UpdateHealth();
    }

    private void FixedUpdate() {
        agent.SetDestination(player.transform.position);
    }
    
    private void UpdateHealth() {
        health = _healthManager.GetHealth();
    }
}
