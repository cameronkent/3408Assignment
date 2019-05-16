using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public LayerMask enemyMask;
    public float speed = 1;
    private Rigidbody2D enemyRigidBody;
    private Transform enemyTransform;
    private float enemyWidth, enemyHeight;
    private Animator animator;
    public bool isAttacking;
    private bool canMove;
    private Vector2 enemyVelocity;
    public GameObject player;
    public int enemyHealth;

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
        player = this.GetComponent<GameObject>();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Collider2D collider = col.collider;

        if (col.gameObject.CompareTag("Floor") || col.gameObject.CompareTag("Enemy"))
        {
            Vector3 contactPoint = col.contacts[0].point;
            Vector3 center = collider.bounds.center;

            bool side = contactPoint.x > center.x;

            if (side)
            {
                Vector3 currRot = enemyTransform.eulerAngles;
                currRot.y += 180;
                enemyTransform.eulerAngles = currRot;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            isAttacking = true;
            canMove = false;
            Flip();
            Attack();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        isAttacking = false;
        canMove = true;
        Flip();
        Attack();
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = enemyTransform.position + enemyTransform.right * enemyWidth;
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

        if (!isGrounded)
        {
            Flip();
        }
        Move();
    }

    // Moves the enemy
    void Move()
    {
        if (canMove)
        {
            enemyVelocity.x = enemyTransform.right.x * speed;
            enemyRigidBody.velocity = enemyVelocity;
            speed = 1;
            animator.SetFloat("Speed", speed);
        }
    }

    // Flips the enemy direction
    void Flip()
    {
        Vector3 currRot = enemyTransform.eulerAngles;
        currRot.y += 180;
        enemyTransform.eulerAngles = currRot;
    }

    // Constraint enemy's x position and enable the attack animation
    void Attack()
    {
        //if (player.health > 0)
        if (isAttacking)
        {
            enemyRigidBody.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            enemyRigidBody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezePositionY;
        }
       
        animator.SetBool("Attack", isAttacking);
    }

    void onHurt()
    {
        enemyHealth -= 1;
    }
}