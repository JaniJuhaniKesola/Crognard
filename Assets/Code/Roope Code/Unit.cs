using UnityEngine;

public class Unit : MonoBehaviour
{
    public string Name;
    public int MaxHP;
    public int CurrentHP;
    public int Damage;

    public void TakeDamage(int amount)
    {
        if (amount > 0)
        {
            CurrentHP -= amount;


            if (CurrentHP <= 0)
            {
                Die();
            }

            //_healthbar.HealthBar(_currentHealth, maxHP);   // Figure out how to attach Healthbar script. Maybe combine Health script with Unit script.
        }
    }

    public void Heal(int amount)
    {
        if (amount > 0)
        {
            CurrentHP += amount;

            if (CurrentHP > MaxHP)
            {
                CurrentHP = MaxHP;
            }

            //_healthbar.HealthBar(_currentHealth, maxHP);
        }
    }

    public void Die()
    {
        // Piece dies and cannot be used again.
        Debug.Log("Piece just died.");
    }
}
