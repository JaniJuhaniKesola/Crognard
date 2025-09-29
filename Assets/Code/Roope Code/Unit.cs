using UnityEngine;

namespace Crognard
{
    public class Unit : MonoBehaviour
    {
        public string Name;
        public int MaxHP;
        public int CurrentHP;
        public int Damage;
        public int Defence;
        public bool Defending;

        public void TakeDamage(int amount)
        {
            if (amount > 0)
            {
                if (Defending)
                {
                    if (amount < Defence) { amount = Defence; }
                    CurrentHP -= amount - Defence;
                }
                else
                { CurrentHP -= amount; }

                if (CurrentHP <= 0) { Die(); }
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
            }
        }

        public void Die()
        {
            // Piece dies and cannot be used again.
            Debug.Log("Piece just died.");
        }
    }
}
