using UnityEngine;

public class CharaterStats : MonoBehaviour
{
    public int maxHealth = 100; // Maximum amount of health
    public int currentHealth;// Current amount of health

    public Stat damage;
    public Stat armor;
    public Stat attackRange;

    public event System.Action<int, int> OnHealthChanged;

    public virtual void Awake()
    {
        currentHealth = maxHealth;
    }

    //Damage the charater
    public void TakeDamage(float damage)
    {
        // Subtract the armor value - Make sure damage doesn't go below 0
        damage -= armor.GetValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        // Subtract damage from health
        currentHealth -= (int)damage;
        Debug.Log(transform.name + " takes " + damage + " damage." );

        if(OnHealthChanged != null)
        {
            OnHealthChanged(maxHealth, currentHealth);
        }

        // If we hit 0. Die
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        //Die is some way
        // This mothod is meant to be overwritten
        Debug.Log(transform.name + " die.");
    }

    
}
