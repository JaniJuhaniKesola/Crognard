using UnityEngine;

namespace Crognard
{
    public class Actions : MonoBehaviour
    {
        public void Attack(Unit attacker, Unit target, CombatUI hub)
        {
            target.TakeDamage(attacker.Damage);

            hub.HealthBar(target.CurrentHP, target.MaxHP);
        }

        public void Defend(Unit defender)
        {
            defender.Defending = true;
        }
    }
}
