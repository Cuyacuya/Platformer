using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 5f;

    private Rigidbody2D rigid;
    private SpriteRenderer sprite;
    private Animator anim;


    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        
    }
    void FixedUpdate()
    {
        Move();
        FlipSprite();
    }

    void Move()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector2 moveInput = new Vector2(moveX, moveY).normalized; // ∫§≈Õ ¡§±‘»≠
        rigid.velocity = moveInput * Speed;

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
}
