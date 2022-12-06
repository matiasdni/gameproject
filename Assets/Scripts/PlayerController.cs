using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public float forwardSpeed;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    private Rigidbody rb;
    private Vector3 movement;
    private int count;
    public float jumpForce;
    private float movementX;
    private float movementY;

    public LayerMask groundLayer;
    private readonly float jumpRaycastDistance = 0.6f;
    private readonly float checkRaycastDistance = 15;
    private bool isGrounded;
    private bool playerJump;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 7.5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);
        
    }

    void OnMove(InputValue movementValue) {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void Update()
    {
        movement.x = movementX;
        movement.y = movementY;

        // speed check
        rb.velocity = forwardSpeed * (rb.velocity.normalized);
        if (rb.velocity.y < 0)
            rb.velocity += Vector3.up * (fallMultiplier - 1) * Physics.gravity.y * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.UpArrow))
            rb.velocity += Vector3.up * (lowJumpMultiplier - 1) * Physics.gravity.y * Time.deltaTime;

        // ground check
        if (Physics.Raycast(transform.position, Vector3.down, jumpRaycastDistance, groundLayer)) 
            isGrounded = true;
        else isGrounded = false;

        // jump check
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
            playerJump = true;
        else playerJump = false;

        if (isGrounded && playerJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        

        if (Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded)
        {
            rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        }

        // if player position is below 0 do check
        if (transform.position.y < 0) {
            // wait for 2 seconds and check with raycast
            StartCoroutine(checkPlayerBounds());
        }
    }

    void FixedUpdate()
    {
        // Move player
        rb.AddForce(Vector3.right * movementX, ForceMode.VelocityChange);
        rb.AddForce(Vector3.forward * forwardSpeed);

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;
            SetCountText();
        }
    }

    private void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if(count >= 12)
        {
            winTextObject.SetActive(true);
        }
    }

    private IEnumerator checkPlayerBounds() {
        yield return new WaitForSeconds(0.5f);
        if (!Physics.Raycast(transform.position, Vector3.down, checkRaycastDistance, groundLayer)) {
            PlayerManager.gameOver = true;
        }
    }
}
