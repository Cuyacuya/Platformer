using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anim;

    public float Speed = 5f;
    public float JumpSpeed = 5f;
    public float JumpCnt = 0;

    private bool isGrounded;

    public enum PlayerState
    {
        Idle,
        Running,
        Jumping
    }

    private PlayerState currentState = PlayerState.Idle;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        OnMove();
        FlipSprite();
        OnJump();
        HandleState();
    }

    void HandleState()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                HandleIdle();
                break;

            case PlayerState.Running:
                HandleRunning();
                break;

            case PlayerState.Jumping:
                HandleJumping();
                break;
        }
    }

    public void HandleIdle()
    {
        anim.SetBool("isIdling", true);
        anim.SetBool("isRunning",false);
        anim.SetBool("isJumping",false);

        if (Input.GetAxisRaw("Horizontal") != 0) currentState = PlayerState.Running;
        else if (Input.GetButton("Jump") && (isGrounded || JumpCnt < 2)) currentState = PlayerState.Jumping;
    }

    public void HandleRunning()
    {
        anim.SetBool("isIdling", false);
        anim.SetBool("isRunning", true);
        anim.SetBool("isJumping", false);

        if (Input.GetAxisRaw("Horizontal") == 0) currentState = PlayerState.Idle;
        else if (Input.GetButton("Jump") && (isGrounded || JumpCnt < 2))
        {
            Debug.Log("점프를 누름");
            currentState = PlayerState.Jumping;
            Debug.Log(currentState);
        }
    }

    public void HandleJumping()
    {
        anim.SetBool("isIdling", false);
        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", true);

        if (Input.GetAxisRaw("Horizontal") != 0) currentState = PlayerState.Running;
        else if (Input.GetAxisRaw("Horizontal") == 0) currentState = PlayerState.Idle;
    }

    void OnMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        Vector2 moveInput = new Vector2(moveX, 0f).normalized; // 벡터 정규화

        if(moveX != 0)
        {
            rigid.velocity = new Vector2(moveInput.x * Speed, rigid.velocity.y);
            //currentState = PlayerState.Running;
        }        
        else
        {
            rigid.velocity = new Vector2(moveInput.x * Speed, rigid.velocity.y);
            //currentState = PlayerState.Idle;
        }
        
    }

    void FlipSprite()
    {
        float FlipPlayer = Input.GetAxisRaw("Horizontal");

        if (FlipPlayer < 0)
            sprite.flipX = true;
        else if (FlipPlayer > 0)
            sprite.flipX = false;
    }

    void OnJump()
    {
        if(Input.GetButtonDown("Jump") && (isGrounded || JumpCnt < 2))
        {
             currentState = PlayerState.Jumping;
            rigid.velocity = Vector2.zero; //1단 점프 후 2점프 시 최대 높이의 점프 가능
            rigid.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
            JumpCnt++;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            JumpCnt = 0;
            if(rigid.velocity.x > 0) currentState = PlayerState.Running;
            if(rigid.velocity.x == 0) currentState = PlayerState.Idle;

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
