using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAdvanced : MonoBehaviour
{
    public float jumpForce = 5f;
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private bool isGrounded;
    private bool isJumping;

    // New Jump variables
    public int maxJumps = 2; // How many jumps are allowed
    private int jumpsLeft;

    // New Attack variables
    public GameObject weaponPrefab; // Assign your weapon prefab in the editor
    public Transform attackSpawnPoint; // Where the weapon should spawn

    public float resetPositionZValue = -10f;
    private Vector3 initialPosition;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpsLeft = maxJumps; // New
        initialPosition = transform.position;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);

        if (moveHorizontal != 0)
        {
            animator.SetBool("isWalking", true);
            FlipSprite(moveHorizontal);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                jumpsLeft = maxJumps;
                isGrounded = false;
                animator.SetBool("IsJumping", true);



            }

            if (jumpsLeft > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", true);
                animator.SetBool("isGrounded", false);
                jumpsLeft--;
            }

        }
        float moveVertical = rb.velocity.y;
        if (!isGrounded && moveVertical <= 0)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }

        if (isGrounded && moveVertical == 0)
        {
            animator.SetBool("isFalling", false);
            animator.SetBool("isGrounded", true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

            animator.SetBool("isAttacking", true);
            StartCoroutine(stopAttackAnimation());
            Attack();
        }

        if (transform.position.y < resetPositionZValue)
        {
            transform.position = initialPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isJumping", false);
            jumpsLeft = maxJumps;
        }
    }

    IEnumerator stopAttackAnimation()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("isAttacking", false);
    }

    void FlipSprite(float horizontalInput)
    {
        Vector3 scale = transform.localScale;
        scale.x = (horizontalInput > 0) ? Mathf.Abs(scale.x) : -Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

    void Attack()
    {
        GameObject weapon = Instantiate(weaponPrefab, attackSpawnPoint.position, Quaternion.identity);
        weapon.transform.SetParent(this.transform);
        Destroy(weapon, 0.2f);
    }
}
