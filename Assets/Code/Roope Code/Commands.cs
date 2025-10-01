using UnityEngine;

namespace Crognard
{
    public enum Action { Attack, Defend, Item1, Item2, Item3 }
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

        public void OnReturn()
        {
            // Deactivate current buttons and activate base option buttons.
            _display.ActivateAttacks(false);
            _display.ActivateItems(false);
            _display.ActivateMenu(true);
        }

        public void OnLightAttack()
        {
            _manager.ActionChosen(Action.Attack, 1);
        }

        public void OnMediumAttack()
        {
            _manager.ActionChosen(Action.Attack, 0);
        }

        public void OnHeavyAttack()
        {
            _manager.ActionChosen(Action.Attack, -1);
            
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

        // Add buttons for Items.
    }
}
