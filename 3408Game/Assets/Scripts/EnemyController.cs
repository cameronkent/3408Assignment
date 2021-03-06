﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public LayerMask enemyMask;
    public float speed = 1;
    public GameObject player;
    private Rigidbody2D enemyRigidBody;
    private Transform enemyTransform;
    private float enemyWidth, enemyHeight;
    private Animator animator;
    private bool isAttacking;
    private bool canMove;
    private Vector2 enemyVelocity;
    private Damageable playerDamageableScript;
    private Damager playerDamagerScript;
    private PlayerMovement playerMovementScript;
    private bool isDead;
    private int enemyHealth = 3;
    private bool facingRight;
 

    void Start()
    {
        enemyTransform = this.transform;
        enemyRigidBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer enemySprite = this.GetComponent<SpriteRenderer>();
        enemyWidth = enemySprite.bounds.extents.x;
        enemyHeight = enemySprite.bounds.extents.y;
        animator = this.GetComponent<Animator>();
        isAttacking = false;
        enemyVelocity = enemyRigidBody.velocity;
        canMove = true;
        isDead = false;
        playerDamageableScript = (Damageable)player.GetComponent(typeof(Damageable));
        playerDamagerScript = (Damager)player.GetComponent(typeof(Damager));
        playerMovementScript = (PlayerMovement)player.GetComponent(typeof(PlayerMovement));
        facingRight = true;
    }

    void FixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(9,12);
        Vector2 lineCastPos = enemyTransform.position + ((enemyTransform.right  *0.5f) -new Vector3(0.0f, 0.3f, 0.0f));
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos - enemyTransform.right.toVector2()*0.2f);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - enemyTransform.right.toVector2() * 0.2f, enemyMask);
        if (!isGrounded || isBlocked)
        {
            Flip();
        }
        if (!isDead)
        {
           
        }
        Move();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            if (!playerMovementScript.isHidden)
            {

                if ((player.transform.position.x < enemyTransform.position.x) && facingRight)
                {
                    Flip();
                }else if ((player.transform.position.x > enemyTransform.position.x) && !facingRight)
                {
                    Flip();
                }
                enemyRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
                isAttacking = true;
                canMove = false;
                Attack();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
       
        isAttacking = false;
        canMove = true;
        Attack();
    }
 
    // Moves the enemy
    void Move()
    {
        if (canMove)
        {
            enemyRigidBody.constraints = RigidbodyConstraints2D.None;
            speed = 1;
        }
        else
        {
            speed = 0;
        }
            
        enemyVelocity.x = enemyTransform.right.x * speed;
        enemyRigidBody.velocity = enemyVelocity;
        animator.SetFloat("Speed", speed);
    }

    // Flips the enemy direction
    void Flip()
    { 
        if (facingRight)
        {
            facingRight = false;
        }
        else { facingRight = true; }
        Vector3 currRot = enemyTransform.eulerAngles;
        currRot.y += 180;
        enemyTransform.eulerAngles = currRot;
    }

    // Enable the attack animation
    void Attack()
    {
        if (playerDamageableScript.CurrentHealth > 0)
        {

            animator.SetBool("Attack", isAttacking);
        }
    }

    public void OnHurt(Damager damager, Damageable damageable)
    {
        animator.SetTrigger("IsHurt");
        damageable.TakeDamage(playerDamagerScript, false);

        //play hurt audio
        //take away the health
    }

    public void OnDie(Damager damager, Damageable damageable)
    {
        canMove = false;
        isDead = true;
        StartCoroutine(PlayAnimationAndDisappear());
        animator.SetBool("IsDead", isDead);
        // :(
    }

    // Waits a few seconds before disabling the enemy
    IEnumerator PlayAnimationAndDisappear()
    {
        animator.SetBool("IsDead", isDead);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false); 
    }
}