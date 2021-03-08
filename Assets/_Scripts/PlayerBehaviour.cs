using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

    public enum PlayerState
    {
        IDLE,
        RUN,
        JUMP,
        KICK
    }

    private Animator animator;
    public CharacterController controller;
    public AudioSource hitSound;

    [Header("Movement")]
    public float maxSpeed = 10.0f;
    public float gravity = -30.0f;
    public float jumpHeight = 3.0f;

    [Header("Ground Detection")]
    public Transform groundCheck;
    public float groundRadius = 0.5f;
    public bool isGrounded;
    public LayerMask groundMask;
    public Vector3 velocity;
   
    private PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        pauseMenu = GameObject.Find("Canvas").GetComponent<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("AnimState", (int)PlayerState.IDLE);
        if (pauseMenu.Paused)
        {
            return;
        }

      
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2.0f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * maxSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.W))
        {
            animator.SetInteger("AnimState", (int)PlayerState.RUN);
        }
        else {
            animator.SetInteger("AnimState", (int)PlayerState.IDLE);
        }
        

        if (Input.GetButton("Jump") && isGrounded)
        {
            Debug.Log("jumping");
            animator.SetInteger("AnimState", (int)PlayerState.JUMP);
            velocity.y = Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}
