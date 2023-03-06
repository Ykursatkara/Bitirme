using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewControlls : MonoBehaviour
{
    Animator animator;

    private float Horizontal;
    private float Speed = 8f;
    public float JumpPower = 20f;
    public float DoubleJumpTimer = 0.2f;
    private bool isFacingRight = true;
    private bool DoubleJump = false;
    private bool isGrounded = false;

    [SerializeField] private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 47;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown("w") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            isGrounded = false;
            Debug.Log("Jumped");
            StartCoroutine(DoubleJumpOn());
        }
        else if (Input.GetKeyDown("w") && DoubleJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            DoubleJump = false;
            animator.SetTrigger("Jump");
        }
        if (Horizontal != 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else if (Horizontal == 0)
        {
            animator.SetBool("IsRunning", false);
        }
        Flip();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(Horizontal * Speed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("Grounded", true);
        }
        if(col.gameObject.tag == "Platform")
        {
            DoubleJump = false;
        }
    }

    IEnumerator DoubleJumpOn()
    {
        Debug.Log("DoubleJumpOn Entered");
        yield return new WaitForSeconds(DoubleJumpTimer);
        DoubleJump = true;
        Debug.Log(DoubleJump);
    }

    void Flip()
    {
        if ((isFacingRight && Horizontal < 0f) || (!isFacingRight && Horizontal > 0f))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

}
