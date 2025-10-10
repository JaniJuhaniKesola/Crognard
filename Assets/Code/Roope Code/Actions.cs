using System.Collections;
using UnityEngine;

namespace Crognard
{
    public class Actions : MonoBehaviour
    {
        [SerializeField] private Action _lightAttack = new Action(1, 1);
        [SerializeField] private Action _mediumAttack = new Action(0, 0);
        [SerializeField] private Action _heavyAttack = new Action(-1, 2);
        [SerializeField] private Action _defend = new Action(1, 1);
        [SerializeField] private Action _counter = new Action(-1, 1);
        [SerializeField] private Action _potion = new Action(0, 1);
        [SerializeField] private Action _restrain = new Action(0, 2);
        [SerializeField] private Action _escape = new Action(2, 2);

        #region TestActions
        public void Attack(ActionType weight, Unit user, Unit target, CombatUI userHub, CombatUI targethub)
        {
            switch (weight)
            {
                case ActionType.Light:
                    user.TakeStamina(1);
                    target.TakeDamage(user.LightDamage);
                    break;

                case ActionType.Medium:
                    user.TakeStamina(0);
                    target.TakeDamage(user.MediumDamage);
                    break;

                case ActionType.Heavy:
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
        #endregion

        public int GetPriority(ActionType type)
        {
            switch (type)
            {
                case ActionType.Light: return _lightAttack.priority;
                case ActionType.Medium: return _mediumAttack.priority;
                case ActionType.Heavy: return _heavyAttack.priority;
                case ActionType.Defend: return _defend.priority;
                case ActionType.Counter: return _counter.priority;
                case ActionType.Item1: return _potion.priority;
                case ActionType.Item2: return _restrain.priority;
                case ActionType.Item3: return _escape.priority;
            }
            return 0;
        }

        public int GetCost(ActionType type)
        {
            switch (type)
            {
                case ActionType.Light: return _lightAttack.staminaCost;
                case ActionType.Medium: return _mediumAttack.staminaCost;
                case ActionType.Heavy: return _heavyAttack.staminaCost;
                case ActionType.Defend: return _defend.staminaCost;
                case ActionType.Counter: return _counter.staminaCost;
                case ActionType.Item1: return _potion.staminaCost;
                case ActionType.Item2: return _restrain.staminaCost;
                case ActionType.Item3: return _escape.staminaCost;
            }
            return 0;
        }

        public void GetAction(ActionType type, Unit user, Unit target, CombatUI userHub, CombatUI targetHub)
        {
            if (type == ActionType.Light || type == ActionType.Medium || type == ActionType.Heavy)
            {
                Attack(type, user, target);
            }
            
            switch (type)
            {
                case ActionType.Defend:
                    user.TakeStamina(_defend.staminaCost);
                    user.Defending = true;
                    break;

                case ActionType.Counter:
                    user.TakeStamina(_counter.staminaCost);
                    if (user.Damaged)
                    {
                        target.TakeDamage(user.DamageTaken * 2);
                    }
                    if (target.Defending)
                    {
                        user.TakeStamina(3);
                    }
                    break;

                case ActionType.Item1:
                    Debug.Log("Taking a Potion");
                    user.TakeStamina(_potion.staminaCost);
                    user.Heal(5);
                    break;

                case ActionType.Item2:
                    user.TakeStamina(_restrain.staminaCost);
                    target.Restrained = true;
                    break;

                case ActionType.Item3:
                    user.TakeStamina(_escape.staminaCost);
                    break;
            }

            Debug.Log("User's HP after it's turn: " + user.CurrentHP + ", target's HP after it's turn: " + target.CurrentHP);

            userHub.HealthBar(user.CurrentHP, user.MaxHP);
            userHub.StaminaBar(user.Stamina, user.MaxStamina);

            targetHub.HealthBar(target.CurrentHP, target.MaxHP);
            targetHub.StaminaBar(target.Stamina, target.MaxStamina);

            Debug.Log("HUBs updated");
            //StartCoroutine(UIUpdate(user, target, userHub, targetHub));
        }

        private void Attack(ActionType weight, Unit user, Unit target)
        {
            switch (weight)
            {
                case ActionType.Light:
                    user.TakeStamina(_lightAttack.staminaCost);
                    target.TakeDamage(user.LightDamage);
                    Blocked(user, target);
                    break;

                case ActionType.Medium:
                    user.TakeStamina(_mediumAttack.staminaCost);
                    target.TakeDamage(user.MediumDamage);
                    Blocked(user, target);
                    break;

                case ActionType.Heavy:
                    user.TakeStamina(_heavyAttack.staminaCost);
                    target.TakeDamage(user.HeavyDamage);
                    Blocked(user, target);
                    break;
            }
        }

        private void Blocked(Unit user, Unit target)
        {
            if (target.Defending)
            {
                user.TakeStamina(3);
            }
        }

        private IEnumerator UIUpdate(Unit user, Unit target, CombatUI userHub, CombatUI targetHub)
        {
            yield return null;
        }
    }
}
