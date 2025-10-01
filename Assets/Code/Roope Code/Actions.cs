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
    }
}
