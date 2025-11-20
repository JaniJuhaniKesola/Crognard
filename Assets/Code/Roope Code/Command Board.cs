using UnityEngine;

namespace Crognard
{
    public class ActionButtons : MonoBehaviour
    {
        [Header("Base Buttons")]
        [SerializeField] private GameObject _counter;
        [SerializeField] private GameObject _defend;
        [Header("Attacks")]
        [SerializeField] private GameObject _heavy;
        [SerializeField] private GameObject _light;
        [Header("Items")]
        [SerializeField] private GameObject _sticky;
        [SerializeField] private GameObject _potion;
        [SerializeField] private GameObject _smoke;

        public void GetButton(ActionType type, bool enable)
        {
            if (type == ActionType.Light)
            {
                EnableButton(_light, enable);
            }
            else if (type == ActionType.Heavy)
            {
                EnableButton(_heavy, enable);
            }
            else if (type == ActionType.Counter)
            {
                EnableButton(_counter, enable);
            }
            else if (type == ActionType.Defend)
            {
                EnableButton(_defend, enable);
            }
            else if (type == ActionType.Item1)
            {
                EnableButton(_potion, enable);
            }
            else if (type == ActionType.Item2)
            {
                EnableButton(_sticky, enable);
            }
            else if (type == ActionType.Item1)
            {
                EnableButton(_smoke, enable);
            }
        }

        public void EnableButton(GameObject button, bool enabled)
        {
            button.SetActive(enabled);
        }
    }
}
