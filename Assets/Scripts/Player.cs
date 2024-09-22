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
    }

    void OnMove()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        Vector2 moveInput = new Vector2(moveX, 0f).normalized; // 벡터 정규화
        rigid.velocity = new Vector2(moveInput.x * Speed, rigid.velocity.y);

        anim.SetBool("isRunning", moveX != 0);
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
            rigid.velocity = Vector2.zero; //1단 점프 후 2점프 시 최대 높이의 점프 가능
            rigid.AddForce(Vector2.up * JumpSpeed, ForceMode2D.Impulse);
            JumpCnt++;

            anim.SetBool("isJumping", true);
            anim.SetBool("isIdling", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            JumpCnt = 0;

            anim.SetBool("isJumping", false);
            anim.SetBool("isIdling", true);
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
