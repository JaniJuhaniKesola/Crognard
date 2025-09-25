using UnityEngine;

public class Health : MonoBehaviour
{
    private int _currentHealth;
    [SerializeField] private int _maxHealth = 1;

    public void TakeDamage(int amount)
    {
        if (amount > 0)
        {
            _currentHealth =- amount;

            if (_currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void Heal(int amount)
    {
        if (amount > 0)
        {
            _currentHealth =+ amount;

            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
        }
    }

    public void Die()
    {
        // Piece dies and cannot be used again.
        Debug.Log("Piece just died.");
    }
}
