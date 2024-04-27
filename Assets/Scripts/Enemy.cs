using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int enemyHealth = 100; // Example 
    public float damageCooldownTime = 2f; // Cooldown time in seconds
    private bool canDamagePlayer = true;
    public float knockbackForce = 1000f; // Force to push the player back

    private Animator playerAnimator;
    public AudioClip hurtSound; // Sound effect for when the player gets hurt
    private AudioSource audioSource;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            TakeDamage(50); // Example damage, adjust as needed
            Destroy(collision.gameObject); // Destroy the weapon
        }
        if (collision.gameObject.CompareTag("Player") && canDamagePlayer)
        {
            HealthManager.health--; // Decrease player's health
            canDamagePlayer = false; // Player can't take damage temporarily
            StartCoroutine(DamageCooldown());

            // Calculate the knockback direction
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            // Apply knockback force to the player
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce);
            playerAnimator = collision.gameObject.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                Debug.Log(":)");
                playerAnimator.SetBool("isHurt", true);
                StartCoroutine(ResetHurtState());
            }
            // Play hurt sound effect
            if (hurtSound != null)
            {
                audioSource = GetComponent<AudioSource>();
                audioSource.PlayOneShot(hurtSound);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        enemyHealth -= damage;
        if (enemyHealth <= 0)
        {
            // Check if the object has a parent
            if (transform.parent != null)
            {
                // If there's a parent, destroy the parent object
                Destroy(transform.parent.gameObject);
            }
            else
            {
                // If there's no parent, destroy this game object
                Destroy(gameObject);
            }
        }
    }


    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldownTime);
        canDamagePlayer = true; // Player can take damage again
    }

    IEnumerator ResetHurtState()
    {
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("isHurt", false); // Set the "Hurt" parameter back to false
        }
    }
}