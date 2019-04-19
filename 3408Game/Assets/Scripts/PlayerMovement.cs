using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 30f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    bool isArmed = false;

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
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

    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}
