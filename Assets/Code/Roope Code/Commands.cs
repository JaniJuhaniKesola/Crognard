using UnityEngine;

namespace Crognard
{
    public class Commands : MonoBehaviour
    {
        private CombatManager _manager;
        private UIDisplay _display;



        private void Start()
        {
            _manager = GetComponent<CombatManager>();
            _display = GetComponent<UIDisplay>();
        }

        public void OnAttackButton()
        {
            _display.ActivateMenu(false);
            _display.ActivateAttacks(true);
        }

        public void OnDefendButton()
        {
            _manager.ActionChosen(ActionType.Defend);
        }

        public void OnItemButton()
        {
            // Open item selection
            _display.ActivateMenu(false);
            _display.ActivateItems(true);
        }

        public void OnCounter()
        {
            _manager.ActionChosen(ActionType.Counter);
        }

        public void OnReturn()
        {
            // Deactivate current buttons and activate base option buttons.
            _display.ActivateAttacks(false);
            _display.ActivateItems(false);
            _display.ActivateMenu(true);
        }

        public void OnLightAttack()
        {
            _manager.ActionChosen(ActionType.Light);
        }

        public void OnMediumAttack()
        {
            _manager.ActionChosen(ActionType.Medium);
        }

        public void OnHeavyAttack()
        {
            _manager.ActionChosen(ActionType.Heavy);
        }

        public void Recover()
        {
            _manager.ActionChosen(ActionType.Item1);
        }

        public void Restrain()
        {
            _manager.ActionChosen(ActionType.Item2);
        }

        public void Escape()
        {
            _manager.ActionChosen(ActionType.Item3);
        }

        public void OnContinue()
        {
            _manager.StartOver();
        }

        // Add buttons for Items.
    }
}
