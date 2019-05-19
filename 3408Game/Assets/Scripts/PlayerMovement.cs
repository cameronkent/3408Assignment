using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public HideEvent hideEvent;
    public float distance;

    private Rigidbody2D rigidbody;

    public bool canMove = true;
    public bool isHidden = false;

    public float runSpeed = 30f;
    float climbSpeed = 5f;
    float horizontalMove = 0f;
    float verticalMove = 0f;
    public bool onLadder = false;
    bool isClimbing = false;
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
        if (canMove == true)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
            verticalMove = Input.GetAxisRaw("Vertical") * climbSpeed;
            animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

            if (Input.GetButtonDown("Jump") && isClimbing == false)
            {
                jump = true;
                animator.SetBool("IsJumping", true);
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                animator.SetBool("IsCrouching", true);
            }
            else if (Input.GetButtonUp("Crouch"))
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
                }
                else
                {
                    isArmed = false;
                    animator.SetBool("IsArmed", false);

                }
            }

            if (Input.GetButtonDown("Attack"))
            {
                Attack();
            }

            if (onLadder)
            {
                controller.m_Grounded = true;
                if (Input.GetButtonDown("Climb"))
                {
                    isClimbing = true;
                    rigidbody.gravityScale = 0;
                    rigidbody.velocity = new Vector2(horizontalMove, verticalMove);

                    animator.SetBool("IsClimbing", true);
                    animator.Play("Player_Ladder_Climb");
                }
                else
                {
                    isClimbing = false;
                    animator.SetBool("IsClimbing", false);
                }
            } else
            {
                rigidbody.gravityScale = 1;
            }
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
    }

}
