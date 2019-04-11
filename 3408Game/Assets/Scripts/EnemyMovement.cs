using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D enemyRigidBody;
    public float m_Speed = 0.001f;
    public bool attack = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        enemyRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!attack)
        {
            animator.SetTrigger("Speed");
            SetMovementSpeed(m_Speed);
        }
        else
        {
            SetMovementSpeed(0.0f);
            animator.SetTrigger("Attack");
        }
          
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            attack = true;
        }
    }
    void SetMovementSpeed(float speed)
    {
        Vector2 enemyVol = enemyRigidBody.velocity;
        enemyVol.x = speed;
        enemyRigidBody.velocity = -enemyVol;
    }
}
