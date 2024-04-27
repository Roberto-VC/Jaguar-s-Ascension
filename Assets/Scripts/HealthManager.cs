using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public static int health = 5;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    // Update is called once per frame
    void Update()
    {

        // Set all hearts to emptyHeart sprite
        foreach (Image img in hearts)
        {
            img.sprite = emptyHeart;
        }

        // Log the health value

        // Loop through hearts array up to health value and set sprites to fullHeart
        for (int i = 0; i < health; i++)
        {
            if (i < hearts.Length) // Make sure we're not accessing out of bounds
            {
                hearts[i].sprite = fullHeart;
            }
        }

        if (health == 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Handle player death logic here 
        // (e.g., reload scene, game over, etc.)
        Debug.Log("Player Died!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

