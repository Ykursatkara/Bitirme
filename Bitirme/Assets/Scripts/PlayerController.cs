using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator animator;

    private float Horizontal;
    private float Speed = 8f;
    public float JumpPower = 20f;
    private bool isFacingRight = true;
    private bool DoubleJump = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask GroundLayer;

    void Start()
    {
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 47;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown("w") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            DoubleJump = true;
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
        }
        else if(Input.GetKeyDown("w") && DoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            DoubleJump = false;
            animator.SetTrigger("Jump");
        }
        if (Horizontal != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else if(Horizontal == 0)
        {
            animator.SetBool("IsRunning", false);
        }
        Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Horizontal * Speed, rb.velocity.y);
    }

    bool isGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("OnCollisionEnter2D");
        animator.SetBool("Grounded", true);
    }

    void Flip()
    {
        if((isFacingRight && Horizontal < 0f) || (!isFacingRight && Horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
 
}
