using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static PlayerMovement instance;

    private CharacterController characterController;
    private Animator animator;

    public new Transform camera;
    public float speed = 4;
    public float gravity = -9.8f;
    public float jumpForce = 5f;

    private float verticalVelocity = 0f;
    private bool isJumping = false;

    void Awake()
    {
        // Mantener entre escenas sin duplicar
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 movement = Vector3.zero;
        float movementSpeed = 0;

        bool isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float maxSpeed = isRunning ? 1.0f : 0.5f;

        if (characterController.isGrounded)
        {
            if (!isJumping)
            {
                verticalVelocity = -1f;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
                isJumping = true;
                animator.SetTrigger("Jump");
                animator.SetBool("IsInAir", true);
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        if (hor != 0 || ver != 0)
        {
            Vector3 forward = camera.forward;
            forward.y = 0;
            forward.Normalize();

            Vector3 right = camera.right;
            right.y = 0;
            right.Normalize();

            Vector3 direction = forward * ver + right * hor;
            movementSpeed = Mathf.Clamp01(direction.magnitude);
            direction.Normalize();

            movementSpeed = Mathf.Min(movementSpeed, maxSpeed);

            movement = direction * speed * movementSpeed;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), 0.2f);
        }

        movement.y = verticalVelocity;
        characterController.Move(movement * Time.deltaTime);

        animator.SetFloat("Speed", movementSpeed);

        if (isJumping && characterController.isGrounded && verticalVelocity < 0f)
        {
            isJumping = false;
            animator.SetBool("IsInAir", false); // se baja al aterrizar
        }
    }
}
