using UnityEngine;

namespace Crognard
{
    public class Actions : MonoBehaviour
    {
        public void Attack(Action weight, Unit user, Unit target, CombatUI hub)
        {
            switch (weight)
            {
                case Action.Light:
                    target.TakeDamage(user.LightDamage);
                    break;

                case Action.Medium:
                    target.TakeDamage(user.MediumDamage);
                    break;

                case Action.Heavy:
                    target.TakeDamage(user.HeavyDamage);
                    break;
            }

            hub.HealthBar(target.CurrentHP, target.MaxHP);
        }

        public void Defend(Unit defender)
        {
            defender.Defending = true;
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
