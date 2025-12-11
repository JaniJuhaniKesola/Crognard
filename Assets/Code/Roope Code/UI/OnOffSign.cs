using TMPro;
using UnityEngine;

namespace Crognard
{
    public class OnOffSign : MonoBehaviour
    {
        public TextMeshProUGUI backgroundOnOff;
        public TextMeshProUGUI actionOnOff;

        private void Start()
        {
            TurnOn(Options.backgroundAnimationsOn, backgroundOnOff);

            TurnOn(Options.combatAnimationsOn, actionOnOff);
        }

        public void PressBackground(bool on)
        {
            TurnOn(on, backgroundOnOff);
        }

        public void PressAction(bool on)
        {
            TurnOn(on, actionOnOff);
        }

        private void TurnOn(bool on, TextMeshProUGUI TMP)
        {
            if (on)
            {
                TMP.text = "on";
                TMP.color = Color.green;
            }
            else
            {
                TMP.text = "off";
                TMP.color = Color.red;
            }
        }
    }
}
