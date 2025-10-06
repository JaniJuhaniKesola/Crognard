using UnityEngine;

namespace Crognard
{
    public class Unit : MonoBehaviour
    {
        public string Name;
        public Faction Faction;
        public int MaxHP;
        public int CurrentHP;
        public int LightDamage, MediumDamage, HeavyDamage;
        public int Defence;
        public bool Damaged, Defending, Restrained;
        public int DamageTaken;

        public void TakeDamage(int amount)
        {
            if (amount > 0)
            {
                Damaged = true;
                int oldHP = CurrentHP;
                if (Defending)
                {
                    if (amount < Defence) { amount = Defence; }
                    CurrentHP -= amount - Defence;
                }
                else
                { CurrentHP -= amount; }

                if (CurrentHP <= 0) { Die(); }
                else { DamageTaken = oldHP - CurrentHP; }
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
