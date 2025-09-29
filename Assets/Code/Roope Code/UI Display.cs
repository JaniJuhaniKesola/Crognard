using UnityEngine;
using UnityEngine.UI;

namespace Crognard
{
    public class UIDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject _whiteCommands, _blackCommands;

        public void WhiteCommands(bool turnedOn)
        {
            _whiteCommands.SetActive(turnedOn);
        }

        public void BlackCommands(bool turnedOn)
        {
            _blackCommands.SetActive(turnedOn);
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
    }
}
