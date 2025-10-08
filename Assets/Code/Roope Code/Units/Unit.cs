using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    public class Unit : MonoBehaviour
    {
        public int ID; 
        public string Name;
        public Faction Faction;
        public int MaxHP, CurrentHP;
        public int MaxStamina, Stamina;
        public int LightDamage, MediumDamage, HeavyDamage;
        public int Defence;
        public bool Damaged, Defending, Restrained;
        public int DamageTaken;
        public List<Action1> _actions;

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

                if (CurrentHP > MaxHP) { CurrentHP = MaxHP; }
            }
        }

        public void Die()
        {
            // Piece dies and cannot be used again.
            Debug.Log("Piece just died.");
        }

        public void TakeStamina(int amount)
        {
            if (amount > 0)
            {
                Stamina -= amount;
                if (Stamina < 0) { Stamina = 0; }
            }
        }

        public void RegainStamina(int amount)
        {
            if (amount > 0)
            {
                CurrentHP += amount;

                if (Stamina > MaxStamina) { Stamina = MaxStamina; }
            }
        }
    }

    
}
