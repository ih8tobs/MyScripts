using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    readonly float playerHeight = 2f;

    [SerializeField] Transform orientation;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float airMultiplier = 0.4f;
    float movementMultiplier = 10f;


    [Header("Sliding")]
    private Vector3 crouchScale = new(1, 0.5f, 1);
    private Vector3 playerScale = new(1, 1, 1);
    public float slideForce = 400;
    public float slideCounterMovement = 0.5f;
    bool crouching;
    private bool CanSprint = true;
    private bool isSliding = false;

    [Header("Health")]
    public float health;
    public int damage = 1;
    public int NextScene;


    [Header("CameraFOV change")]
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float sprintFov;
    [SerializeField] private float fovTime;


    [Header("Sprinting")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintSpeed = 6f;
    [SerializeField] float acceleration = 10f;

    [Header("Jumping")]
    public float jumpForce = 5f;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Drag")]
    [SerializeField] float groundDrag = 6f;
    [SerializeField] float airDrag = 2f;

    float horizontalMovement;
    float verticalMovement;

    [Header("Ground Detection")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundMask;
    [SerializeField] float groundDistance = 0.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public float minMoveDistance = 0.1f;
    public float minMoveDistanceSliding = 0.1f;
    private Vector3 previousPosition;

    [SerializeField] private AudioClip[] clips1;
    private int clipIndex1;
    public AudioSource audioSource5;


    public bool IsGrounded { get; private set; }

    Vector3 moveDirection;
    Vector3 slopeMoveDirection;

    Rigidbody rb;
   


    RaycastHit slopeHit;

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight / 2 + 0.5f))
        {
            if (slopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        previousPosition = transform.position;
    }

    private void Update()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        MyInput();
        ControlDrag();
        ControlSpeed();

        if (Input.GetKeyDown(jumpKey) && IsGrounded)
        {
            Jump();
        }

        slopeMoveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);

        // add sound to movement
        float moveDistance = Vector3.Distance(transform.position, previousPosition);

        
        if (moveDistance >= minMoveDistance && IsGrounded && !crouching)
        {
            
            audioSource.Play();

            
            previousPosition = transform.position;
        }

        if (moveDistance >= minMoveDistanceSliding && isSliding && slideForce > 100)
        {
            if (!audioSource2.isPlaying)
            {
                audioSource2.Play();
                previousPosition = transform.position;
            }
        }
    }

    


    void MyInput()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        //crouching input keys
        crouching = Input.GetKey(KeyCode.C);
        if (Input.GetKeyDown(KeyCode.C))
        {
            CanSprint = false;
            StartCrouch();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            CanSprint = true;
            StopCrouch();
        }
        
        moveDirection = orientation.forward * verticalMovement + orientation.right * horizontalMovement;
    }
    private void StartCrouch()
    {

        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (IsGrounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
                isSliding = true;
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        isSliding = false;
    }

    void Jump()
    {
        if (IsGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            clipIndex1 = Random.Range(0, clips1.Length);
            audioSource5.clip = clips1[clipIndex1];
            audioSource5.Play();
        }
    }

    void ControlSpeed()
    {
        if (Input.GetKey(sprintKey) && CanSprint == true)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, sprintFov, fovTime * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, acceleration * Time.deltaTime);
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, fovTime * Time.deltaTime);
        }
    }

    void ControlDrag()
    {
        if (IsGrounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = airDrag;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (IsGrounded && !OnSlope())
        {
            rb.AddForce(force: movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
            
        }
        else if (IsGrounded && OnSlope())
        {
            rb.AddForce(force: movementMultiplier * moveSpeed * slopeMoveDirection.normalized, ForceMode.Acceleration);
            
        }
        else if (!IsGrounded)
        {
            rb.AddForce(force: airMultiplier * movementMultiplier * moveSpeed * moveDirection.normalized, ForceMode.Acceleration);
            
        }


        if (crouching && IsGrounded)
        {
            rb.AddForce(3000 * Time.deltaTime * Vector3.down);
            return;
        }

        if (IsGrounded && crouching) movementMultiplier = 0f;

        if (crouching)
        {
            rb.AddForce(moveSpeed * slideCounterMovement * Time.deltaTime * -rb.velocity.normalized);
            return;
        }

    }
}