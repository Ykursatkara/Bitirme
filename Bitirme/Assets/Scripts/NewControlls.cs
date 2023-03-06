using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewControlls : MonoBehaviour
{
    Animator animator;

    private double Points = 0;
    private float Horizontal;
    private float Speed = 8f;
    public float JumpPower = 20f;
    public float DoubleJumpTimer = 0.2f;
    private bool isFacingRight = true;
    private bool DoubleJump = false;
    private bool isGrounded = false;
    private bool Climbing = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Text PointsText;

    void Start()
    {
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 47;
    }

    void Update()
    {
        Horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown("w") && isGrounded && !Climbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            isGrounded = false;
            StartCoroutine(DoubleJumpOn());
        }
        else if (Input.GetKeyDown("w") && DoubleJump && !Climbing)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            DoubleJump = false;
            animator.SetTrigger("Jump");
        }
        else if (Input.GetKey("w") && Climbing)
        {
            animator.SetBool("Climbing", true);
            rb.velocity = new Vector2(rb.velocity.x, JumpPower/2);
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

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "Pickup")
        {
            Points++;
            trigger.gameObject.SetActive(false);
            PointsText.text = Points.ToString("F1");
        }
        if(trigger.gameObject.tag == "Ladder")
        {
            Climbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "Ladder")
        {
            Climbing = false;
            animator.SetBool("Climbing", false);
        }
    }

    IEnumerator DoubleJumpOn()
    {
        yield return new WaitForSeconds(DoubleJumpTimer);
        DoubleJump = true;
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
