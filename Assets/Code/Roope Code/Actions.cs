using System;
using UnityEngine;

namespace Crognard
{
    public class Actions : MonoBehaviour
    {
        public void Attack(Action weight, Unit user, Unit target, CombatUI userHub, CombatUI targethub)
        {
            switch (weight)
            {
                case Action.Light:
                    user.TakeStamina(1);
                    target.TakeDamage(user.LightDamage);
                    break;

                case Action.Medium:
                    user.TakeStamina(0);
                    target.TakeDamage(user.MediumDamage);
                    break;

                case Action.Heavy:
                    user.TakeStamina(2);
                    target.TakeDamage(user.HeavyDamage);
                    break;
            }

            userHub.StaminaBar(user.Stamina, user.MaxStamina);
            targethub.HealthBar(target.CurrentHP, target.MaxHP);
        }

        public void Defend(Unit user)
        {
            user.Defending = true;
        }

        public void Counter(Unit user, Unit target, CombatUI hub)
        {
            if (user.Damaged)
            {
                target.TakeDamage(user.DamageTaken * 2);
            }

            hub.HealthBar(target.CurrentHP, target.MaxHP);
        }

        /* Items / Spells
        Restrain: Sticky Bomb or Entangle: Target cannot move in a board for the following turn.
        Escape: Smoke Bomb or Teleport: User escapes the combat phase that in preventing the fight from happening.
        Healing: Potion or Heal: User recovers HP.
        */

        public void Recover(Unit user, CombatUI hub)
        {
            user.Heal(5);   // Recover amount unclear.

            hub.HealthBar(user.CurrentHP, user.MaxHP);
        }

        public void Restrain(Unit target)
        {
            target.Restrained = true;
        }

        public void Escape()
        {

        }
    }

    
}
