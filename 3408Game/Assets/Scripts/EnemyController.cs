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
    public Animation animation;
    public bool isAttacking;
    private bool canMove;
    private Vector2 enemyVelocity;
    public GameObject player;
    private Damageable playerDamageableScript;
    private Damager playerDamagerScript;
    private PlayerMovement playerMovementScript;
    private bool isDead;
    private int enemyHealth = 3; 

    void Start()
    {
        enemyTransform = this.transform;
        enemyRigidBody = this.GetComponent<Rigidbody2D>();
        SpriteRenderer enemySprite = this.GetComponent<SpriteRenderer>();
        enemyWidth = enemySprite.bounds.extents.x;
        enemyHeight = enemySprite.bounds.extents.y;
        animator = this.GetComponent<Animator>();
        animation = GetComponent<Animation>();
        isAttacking = false;
        enemyVelocity = enemyRigidBody.velocity;
        canMove = true;
        isDead = false;
        playerDamageableScript = (Damageable) player.GetComponent(typeof(Damageable));
        playerDamagerScript = (Damager) player.GetComponent(typeof(Damager));
        playerMovementScript = (PlayerMovement)player.GetComponent(typeof(PlayerMovement));
    }

    void FixedUpdate()
    {
        Vector2 lineCastPos = enemyTransform.position + enemyTransform.right * enemyWidth;
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, enemyMask);

        if (!isGrounded)
        {
            Flip();
        }
        if (!isDead)
        {
            Move();
        }
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
            if (!playerMovementScript.isHidden)
            {
                isAttacking = true;
                canMove = false;
                Flip();
                Attack();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        isAttacking = false;
        canMove = true;
        Flip();
        Attack();
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
        if (playerDamageableScript.CurrentHealth > 0)
        {
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

    IEnumerator PlayAnimationAndDisappear()
    {
        animator.SetBool("IsDead", isDead);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false); // deactivate object
    }
}