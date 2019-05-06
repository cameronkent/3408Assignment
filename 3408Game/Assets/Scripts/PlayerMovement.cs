using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    private Rigidbody2D rigidbody;

    public float runSpeed = 30f;
    float horizontalMove = 0f;
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
            animator.SetFloat("vSpeed", rigidbody.velocity.y);

        }

        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
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
    }
}
