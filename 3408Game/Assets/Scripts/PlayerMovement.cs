using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;
    public HideEvent hideEvent;
    public float distance;
    public LayerMask whatIsLadder;

    private Rigidbody2D rigidbody;

    public bool canMove = true;

    public float runSpeed = 30f;
    float climbSpeed = 20f;
    float horizontalMove = 0f;
    float inputVertical;
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

        if (canMove == true)
        {
            if (onLadder)
            {
                if (Input.GetButtonDown("Climb"))
                {
                    isClimbing = true;
                    animator.SetTrigger("IsClimbing");
                    animator.Play("Player_Ladder_Climb");
                    inputVertical = Input.GetAxisRaw("Vertical") * climbSpeed;
                    rigidbody.velocity = new Vector2(rigidbody.velocity.x, inputVertical);
                    rigidbody.gravityScale = 0;
                }
                else
                {
                    isClimbing = false;
                    rigidbody.gravityScale = 1;
                }
            }
        }

    }

}
