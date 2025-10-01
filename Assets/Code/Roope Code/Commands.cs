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
            _manager.ActionChosen(Action.Defend, 1);
        }

        public void OnItemButton()
        {
            // Open item selection
            _display.ActivateMenu(false);
            _display.ActivateItems(true);
        }

        public void OnCounter()
        {
            _manager.ActionChosen(Action.Counter, -1);
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
            _manager.ActionChosen(Action.Light, 1);
        }

        public void OnMediumAttack()
        {
            _manager.ActionChosen(Action.Medium, 0);
        }

        public void OnHeavyAttack()
        {
            _manager.ActionChosen(Action.Heavy, -1);

        }

        public void OnItem1()
        {

        }

        public void OnItem2()
        {

        }

        public void OnItem3()
        {

        }

        public void OnContinue()
        {
            _manager.StartOver();
        }

        // Add buttons for Items.
    }
}
