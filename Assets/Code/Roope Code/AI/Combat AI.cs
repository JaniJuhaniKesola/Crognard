using System;
using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    public class CombatAI : MonoBehaviour
    {
        private Unit _target, _user;
        private bool _attacker;
        private List<ActionType> _availableActions;
        private Actions _actions;

        public enum Nature { Aggressive, Passive, Coward, Patient, Trickster };

        private void Start()
        {
            _actions = GetComponent<Actions>();
        }

        private void AllOptions()
        {
            //_availableActions
            for (int i = 0; i < Enum.GetNames(typeof(ActionType)).Length; i++)
            {
                // Implement sometime.
            }
        }

        private void DecideAction()
        {
            // Which action do you want to make?
            // Can you do that action?
            // What is the most optimal option?

            if (_user.CurrentHP / (float)_user.MaxHP < 0.5)
            {
                // Heal if you can
            }

            // If nothing else works -> Medium Attack
        }

        private void AttackOptions()
        {
            if (CanKill(_target.CurrentHP, _user.LightDamage))
            {
                if (CheckStamina(_user.Stamina, _actions.GetCost(ActionType.Light)))
                {
                    
                }
            }
            else if (CanKill(_target.CurrentHP, _user.MediumDamage))
            {
                
            }
            else if (CanKill(_target.CurrentHP, _user.HeavyDamage))
            {
                
            }
        }

        private bool CanKill(int HP, int damage)
        {
            if (HP <= damage)
            {
                return true;
            }
            return false;
        }

        private bool CheckStamina(int stamina, int cost)
        {
            if (stamina >= cost)
            {
                return true;
            }
            return false;
        }
    }
}
