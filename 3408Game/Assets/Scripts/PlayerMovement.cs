using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public float distance;
    public LayerMask whatIsLadder;

    private Rigidbody2D rigidbody;

    public float runSpeed = 30f;
    float horizontalMove = 0f;
    float inputVertical;
    bool isClimbing;
    bool jump = false;
    bool crouch = false;
    bool isArmed = false;
    int attack = 1;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);

        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            animator.SetBool("IsCrouching", true);
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            animator.SetBool("IsCrouching", false);
        }

        if (Input.GetButtonDown("Sheath")) 
        {
            if (!isArmed)
            {
                isArmed = true;
                animator.SetBool("IsArmed", true);
            } else
            {
                isArmed = false;
                animator.SetBool("IsArmed", false);

            }
        }

        if (Input.GetButtonDown("Attack"))
        {
            Attack();

        }

    }

    public void Attack()
    {
        animator.SetInteger("Attack", attack);
        animator.SetTrigger("Attacking");
        attack = attack + 1;
    }

    public void EndAttack()
    {
        attack = 1;
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
        jump = false;
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);

        if (hitInfo.collider != null)
        {
            if(Input.GetButtonDown("Climb"))
            {
                isClimbing = true;
                animator.SetBool("IsClimbing", true);
            }
        } else
        {
            isClimbing = false;
            animator.SetBool("IsClimbing", false);
        }

        if (isClimbing == true)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, inputVertical * runSpeed);
            rigidbody.gravityScale = 0;
        } else
        {
            rigidbody.gravityScale = 1;
        }
    }
}
