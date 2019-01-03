using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int startingHealth;
    int health;
    public GameObject deadBody;

    private void Start() 
    {
        ResetHealth();

    }

    public void TakeDamage(int damage)
    {
        health -= damage; 
           if(health<=0)
        {
            Die();
        }
    }

    void Die()
    {
        if(deadBody!=null) 
        {
            Instantiate(deadBody, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void ResetHealth() 
    {
        if(startingHealth>0)
        {
            health = startingHealth;
        }
        else
        {
            health = 100;
        }
    }

}
