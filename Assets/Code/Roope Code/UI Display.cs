using UnityEngine;
using UnityEngine.UI;

namespace Crognard
{
    public class UIDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _whiteCommands, _blackCommands;
        [SerializeField] private GameObject _whiteBase, _whiteAttacks, _whiteItems;
        [SerializeField] private GameObject _blackBase, _blackAttacks, _blackItems;

        private CombatManager _manager;

        private void Start()
        {
            _manager = GetComponent<CombatManager>();
        }

        public void WhiteCommands(bool turnedOn)
        {
            _whiteCommands.SetActive(turnedOn);
            ActivateAttacks(false);
            ActivateItems(false);
            ActivateMenu(true);
        }

        public void BlackCommands(bool turnedOn)
        {
            _blackCommands.SetActive(turnedOn);
            ActivateAttacks(false);
            ActivateItems(false);
            ActivateMenu(true);
        }

        /// <summary>
        /// In order to prevent players pushing buttons and causing unitentional effects,
        /// this method will disable and enable buttons after interaction.
        /// Keep in mind every button press may include using this many times.
        /// Better have a list of buttons in some script.
        /// </summary>
        /// <param name="button">Button that's enability is changed</param>
        /// <param name="enable">Boolean if the button is supposed to be enabled or disabled</param>
        public void EnableButton(Button button, bool enable)
        {
            button.interactable = enable;
        }

        public void ActivateMenu(bool active)
        {
            if (_manager != null)
            {
                if (_manager._currentState == CombatState.ChooseW)
                {
                    _whiteBase.SetActive(active);
                }
                else if (_manager._currentState == CombatState.ChooseB)
                {
                    _blackBase.SetActive(active);
                }
            }
            _whiteBase.SetActive(active);
            _blackBase.SetActive(active);
        }

        public void ActivateAttacks(bool active)
        {
            if (_manager != null)
            {
                if (_manager._currentState == CombatState.ChooseW)
                {
                    _whiteAttacks.SetActive(active);
                    return;
                }
                else if (_manager._currentState == CombatState.ChooseB)
                {
                    _blackAttacks.SetActive(active);
                    return;
                }
            }
            _whiteAttacks.SetActive(active);
            _blackAttacks.SetActive(active);
        }

        public void ActivateItems(bool active)
        {
            if (_manager != null)
            {
                if (_manager._currentState == CombatState.ChooseW)
                {
                    _whiteItems.SetActive(active);
                }
                else if (_manager._currentState == CombatState.ChooseB)
                {
                    _blackItems.SetActive(active);
                }
            }
            _whiteItems.SetActive(active);
            _blackItems.SetActive(active);
        }
    }
}
