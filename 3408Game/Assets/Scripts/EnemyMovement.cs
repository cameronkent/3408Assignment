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
    BoxCollider2D boxCollider;

    void Start()
    {
        enemyTransform = this.transform;
        enemyRigidBody = this.GetComponent<Rigidbody2D>();
        boxCollider = this.GetComponent<BoxCollider2D>();
        SpriteRenderer enemySprite = this.GetComponent<SpriteRenderer>();
        enemyWidth = enemySprite.bounds.extents.x;
        enemyHeight = enemySprite.bounds.extents.y;
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = enemyTransform.position + enemyTransform.right * enemyWidth;
        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);
        Debug.DrawLine(lineCastPos, lineCastPos + enemyTransform.right.toVector2() * -0.2f);
        bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos + enemyTransform.right.toVector2() * -0.2f, enemyMask);
        
        if (!isGrounded || isBlocked)
        {
            Vector3 currRot = enemyTransform.eulerAngles;
            currRot.y += 180;
            enemyTransform.eulerAngles = currRot;
        }
        Vector2 myVel = enemyRigidBody.velocity;
        myVel.x = enemyTransform.right.x * speed;
        enemyRigidBody.velocity = myVel;
        animator.SetFloat("Speed", speed);
    }
}