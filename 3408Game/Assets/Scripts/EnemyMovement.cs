using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public LayerMask enemyMask;
    public float speed = 1;
    Rigidbody2D enemyRigidBody;
    Transform enemyTransform;
    float enemyWidth, enemyHeight;
    Animator animator;
    bool isAttacking;

    void Start()
    {
        enemyTransform = this.transform;
        enemyRigidBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer enemySprite = this.GetComponent<SpriteRenderer>();
        enemyWidth = enemySprite.bounds.extents.x;
        enemyHeight = enemySprite.bounds.extents.y;
        animator = this.GetComponent<Animator>();
        isAttacking = false;

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
                Debug.Log("side");
                Vector3 currRot = enemyTransform.eulerAngles;
                currRot.y += 180;
                enemyTransform.eulerAngles = currRot;
          }
        }
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = enemyTransform.position + enemyTransform.right * enemyWidth;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
      
        if (!isGrounded)
        {
            Flip();
        }
        Move();
    }


    void Move()
    {
        Vector2 myVel = enemyRigidBody.velocity;
        myVel.x = enemyTransform.right.x * speed;
        enemyRigidBody.velocity = myVel;
        animator.SetFloat("Speed", speed);
    }

    void Flip()
    {
        Vector3 currRot = enemyTransform.eulerAngles;
        currRot.y += 180;
        enemyTransform.eulerAngles = currRot;
    }

    void Atack()
    {
        animator.SetBool("isAttaking", isAttacking);
    }
}