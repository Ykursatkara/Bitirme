using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewControlls : MonoBehaviour
{
    Animator animator;

    Collider2D cocuk_collider;

    public GameObject DeathText;
    public GameObject PowerUp_Box;

    private float Horizontal;
    private float Speed = 8f;
    public float JumpPower = 20f;
    public float DoubleJumpTimer = 0.2f;
    private string PowerUpType;
    private bool isFacingRight = true;
    private bool DoubleJump = false;
    private bool DoubleJumped = false;
    private bool isGrounded = false;
    private bool Climbing = false;
    private bool isDying = false;
    private bool isDashing = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Text PowerUpText;

    void Start()
    {
        animator = GetComponent<Animator>();
        cocuk_collider = GetComponent<Collider2D>();
        Application.targetFrameRate = 47;
    }

    void Update()
    {
        if(isDashing)
        {
            return;
        }
        Horizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown("w") && isGrounded && !Climbing && !isDying)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            isGrounded = false;
            StartCoroutine(DoubleJumpOn());
        }
        else if (Input.GetKeyDown("w") && DoubleJump && !Climbing && !isDying)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPower);
            DoubleJump = false;
            DoubleJumped = true;
            animator.SetTrigger("Jump");
        }
        else if (Input.GetKey("w") && Climbing && !isDying)
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
        if(Input.GetKeyDown("space"))
        {
            UsePowerUp();
        }
        if(Input.GetKeyDown("k"))
        {
            Death();
        }
        Flip();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(Horizontal * Speed, rb.velocity.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("Grounded", true);
            DoubleJumped = false;
        }
        if(col.gameObject.tag == "Platform")
        {
            DoubleJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D trigger)
    {
        if(trigger.gameObject.tag == "PowerUp_TripleJump")
        {
            trigger.gameObject.SetActive(false);
            PowerUpText.text = "Triple Jump";
            PowerUpType = "Triple Jump";
        }
        else if(trigger.gameObject.tag == "PowerUp_Dash")
        {
            trigger.gameObject.SetActive(false);
            PowerUpText.text = "Dash";
            PowerUpType = "Dash";
        }
        else if(trigger.gameObject.tag == "PowerUp_Box")
        {
            trigger.gameObject.SetActive(false);
            PowerUpText.text = "Box";
            PowerUpType = "Box";
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

    void UsePowerUp()
    {
        if(PowerUpType == "Triple Jump" && DoubleJumped)
        {
            Debug.Log("Triple Jumped");
            rb.velocity = new Vector2(rb.velocity.x, JumpPower* 1.5f);
            animator.SetTrigger("Jump");
            PowerUpType = "";
            PowerUpText.text = "";
        }
        else if (PowerUpType == "Dash" && Horizontal != 0)
        {
            StartCoroutine(Dash());
        }
        else if(PowerUpType == "Box")
        {
            if(Horizontal != 0)
            {
                GameObject NewBox = Instantiate(PowerUp_Box, new Vector2(transform.localPosition.x - Horizontal*2 , transform.localPosition.y - 0.313f), Quaternion.identity);
                NewBox.SetActive(true);
                PowerUpType = "";
                PowerUpText.text = "";
            }
            else
            {
                if(isFacingRight)
                {
                    GameObject NewBox = Instantiate(PowerUp_Box, new Vector2(transform.localPosition.x - 2, transform.localPosition.y - 0.313f), Quaternion.identity);
                    NewBox.SetActive(true);
                    PowerUpType = "";
                    PowerUpText.text = "";
                }
                else
                {
                    GameObject NewBox = Instantiate(PowerUp_Box, new Vector2(transform.localPosition.x + 2, transform.localPosition.y - 0.313f), Quaternion.identity);
                    NewBox.SetActive(true);
                    PowerUpType = "";
                    PowerUpText.text = "";
                }
            }
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * 2f, 0f);
        animator.SetTrigger("Dash");
        PowerUpType = "";
        PowerUpText.text = "";
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("DashEnd");
        isDashing = false;
        rb.gravityScale = 1f;
    }

    void Flip()
    {
        if ((isFacingRight && Horizontal < 0f) || (!isFacingRight && Horizontal > 0f) && !isDying)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void Death()
    {
        isDying = true;
        Speed = 0;
        rb.velocity = new Vector2(rb.velocity.x, JumpPower * 1.5f);
        cocuk_collider.isTrigger = true;
        animator.SetTrigger("Dead");
        DeathText.SetActive(true);
    }

}
