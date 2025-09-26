using UnityEngine;

public class Health : MonoBehaviour
{
    private int _currentHealth;
    private int _maxHealth;

    private Unit _unit;
    private HealthBar _healthbar;

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _currentHealth = _unit.CurrentHP;
        _maxHealth = _unit.MaxHP;
    }

    public void TakeDamage(int amount)
    {
        if (amount > 0)
        {
            _currentHealth = -amount;


            if (_currentHealth <= 0)
            {
                Die();
            }

            _healthbar.EditHealthBar(_currentHealth, _maxHealth);   // Figure out how to attach Healthbar script. Maybe combine Health script with Unit script.
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

            _healthbar.EditHealthBar(_currentHealth, _maxHealth);
        }
    }

    public void Die()
    {
        // Piece dies and cannot be used again.
        Debug.Log("Piece just died.");
    }
}
