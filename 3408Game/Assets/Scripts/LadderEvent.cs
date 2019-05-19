using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderEvent : MonoBehaviour
{
    public Animator animator;
    PlayerMovement playerMovement;
    bool canClimb;
    bool isClimbing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canClimb = true;
        playerMovement = collision.GetComponent<PlayerMovement>();
        playerMovement.onLadder = true;

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        canClimb = false;
        playerMovement.onLadder = false;
    }

}
