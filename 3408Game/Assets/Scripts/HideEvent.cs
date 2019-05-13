using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEvent : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    PlayerMovement playerMovement;
    bool canHide = false;
    bool isHiding = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canHide = true;
        playerMovement = collision.GetComponent<PlayerMovement>();

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canHide = false;
    }


    private void FixedUpdate()
    {
        if (canHide == true && (Input.GetButtonDown("Hide")))
        {
            if (isHiding == false)
            {
                animator.Play("Player_Hide");
                StartCoroutine (Wait());
                spriteRenderer.enabled = true;
                isHiding = true;
                animator.SetBool("IsHiding", true);
                playerMovement.canMove = false;
                playerMovement.isHidden = true;
            }
            else if (isHiding == true)
            {
                animator.Play("Player_Hide");
                StartCoroutine(Wait());
                spriteRenderer.enabled = false;
                isHiding = false;
                animator.SetTrigger("ChangeHiding");
                playerMovement.canMove = true;
                playerMovement.isHidden = false;
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.4f);
    }
}
