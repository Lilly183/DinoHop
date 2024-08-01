using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //====================
    // Member Variable(s):
    //====================

    public uint maxHealth = 100;

    [HideInInspector]
    public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = (int)maxHealth;
    }

    public float CalcHealthPercentage()
    {
        return (((float)currentHealth / (float)maxHealth)) * 100;
    }

    public bool ChangeHealth(int healthAdjustment)
    {
        currentHealth = Mathf.Min((int)maxHealth, currentHealth + healthAdjustment);

        if (currentHealth <= 0)
        {
            Die();
            return true;
        }

        return false;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnValidate()
    {
        maxHealth = (uint)Mathf.Max(maxHealth, 1);
    }
}