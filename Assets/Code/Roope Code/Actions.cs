using UnityEngine;

namespace Crognard
{
    public class Actions : MonoBehaviour
    {
        [Header("Action settings")]
        [SerializeField] private Attack _light = new Attack(new Action(1, 1), 0);
        [SerializeField] private Attack _medium = new Attack(new Action(0, 0), 1);
        [SerializeField] private Attack _heavy = new Attack(new Action(-1, 2), 5);
        [SerializeField] private Action _defend = new Action(1, 1);
        [SerializeField] private Action _counter = new Action(-1, 1);
        [SerializeField] private Action _potion = new Action(0, 1);
        [SerializeField] private Action _restrain = new Action(0, 2);
        [SerializeField] private Action _escape = new Action(2, 2);

        [Header("Other Settings")]
        [SerializeField] private int _criticalBonus = 2;

        private Announcement _announcement;
        private CombatManager _manager;

        private void Start()
        {
            _manager = GetComponent<CombatManager>();
            _announcement = GetComponent<Announcement>();
        }

        public int GetPriority(ActionType type)
        {
            switch (type)
            {
                case ActionType.Light: return _light.actionData.priority;
                case ActionType.Medium: return _medium.actionData.priority;
                case ActionType.Heavy: return _heavy.actionData.priority;
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
                case ActionType.Light: return _light.actionData.staminaCost;
                case ActionType.Medium: return _medium.actionData.staminaCost;
                case ActionType.Heavy: return _heavy.actionData.staminaCost;
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
                AttackAction(type, user, target);
            }
            
            switch (type)
            {
                case ActionType.Defend:
                    user.TakeStamina(_defend.staminaCost);
                    user.Defending = true;
                    _announcement.Defending(user.Name);
                    break;

                case ActionType.Counter:
                    user.TakeStamina(_counter.staminaCost);
                    if (user.Damaged)
                    {
                        target.TakeDamage(user.DamageTaken * 2);
                        _announcement.CounterAttack(user.Name, target.Name);
                    }
                    else
                    {
                        user.TakeStamina(3);
                    }
                    break;

                case ActionType.Item1:
                    Debug.Log("Taking a Potion");
                    user.TakeStamina(_potion.staminaCost);
                    user.Heal(user.Potion);
                    _announcement.Healed(user.Name);
                    break;

                case ActionType.Item2:
                    user.TakeStamina(_restrain.staminaCost);
                    target.Restrained = true;
                    _announcement.Sticky(user.Name, target.Name);
                    break;

                case ActionType.Item3:
                    user.TakeStamina(_escape.staminaCost);
                    _manager._escaped = true;
                    _announcement.Smoke(user.Name);
                    break;
            }

            //Debug.Log("User's HP after it's turn: " + user.CurrentHP + ", target's HP after it's turn: " + target.CurrentHP);

            userHub.HealthBar(user.CurrentHP, user.MaxHP);
            userHub.StaminaBar(user.Stamina, user.MaxStamina);

            targetHub.HealthBar(target.CurrentHP, target.MaxHP);
            targetHub.StaminaBar(target.Stamina, target.MaxStamina);

            //Debug.Log("HUBs updated");
            //StartCoroutine(UIUpdate(user, target, userHub, targetHub));
        }

        private void AttackAction(ActionType weight, Unit user, Unit target)
        {
            Debug.Log("Using this method");
            switch (weight)
            {
                case ActionType.Light:
                Attack(_light, user, target, user.LightDamage);
                /*
                user.TakeStamina(_lightAttack.actionData.staminaCost);
                target.TakeDamage(user.LightDamage);
                Blocked(user, target);*/
                break;

                case ActionType.Medium:
                Attack(_medium, user, target, user.MediumDamage);
                /*
                user.TakeStamina(_mediumAttack.actionData.staminaCost);
                Hit hit = AccuracyCheck(_mediumAttack.missValue, user.CritRange);
                if (hit == Hit.Hit)
                { target.TakeDamage(user.MediumDamage); }
                else if (hit == Hit.Crit)
                {
                    target.TakeDamage(user.MediumDamage * _criticalBonus);
                    _announcement.Critical(user.Name);
                }
                else
                { _announcement.Miss(user.Name); }
                Blocked(user, target);*/
                break;

                case ActionType.Heavy:
                Attack(_heavy, user, target, user.HeavyDamage);
                /*
                user.TakeStamina(_heavyAttack.actionData.staminaCost);
                Hit hit1 = AccuracyCheck(_heavyAttack.missValue, user.CritRange);
                if (hit1 == Hit.Hit)
                { target.TakeDamage(user.HeavyDamage); }
                else if (hit1 == Hit.Crit)
                {
                    target.TakeDamage(user.HeavyDamage * 2);
                    _announcement.Critical(user.Name);
                }
                else
                { _announcement.Miss(user.Name); }
                Blocked(user, target);*/
                break;
            }
        }

        private void Blocked(Unit user, Unit target)
        {
            if (target.Defending)
            {
                user.TakeStamina(target.Fatigue);
                _announcement.Blocked(target.Name);
            }
        }

        private Hit AccuracyCheck(int missValue, int critRange)
        {
            int number = Random.Range(1, 21);
            Debug.Log("Random number = " + number + ", missfire value = " + missValue);
            if (number <= missValue)
            {
                return Hit.Miss;
            }
            else if (number == 21 - critRange)
            {
                return Hit.Crit;
            }
            else
            {
                return Hit.Hit;
            }
        }

        private void Attack(Attack weight, Unit user, Unit target, int damage)
        {
            user.TakeStamina(weight.actionData.staminaCost);
            Hit hit = AccuracyCheck(weight.missValue, user.CritRange);
            if (hit == Hit.Hit)
            { target.TakeDamage(damage); }
            else if (hit == Hit.Crit)
            {
                target.TakeDamage(damage * _criticalBonus);
                _announcement.Critical(user.Name);
            }
            else
            { _announcement.Miss(user.Name); }
            Blocked(user, target);
        }
    }
}
