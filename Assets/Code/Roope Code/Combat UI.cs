using System.Collections;
using TMPro;
using UnityEngine;

namespace Crognard
{
    public class CombatUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private RectTransform _healthFill, _fillPrint;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private RectTransform _staminaFill, _staminafillPrint;
        [SerializeField] private TextMeshProUGUI _staminaText;

        public void SetupInfo(Unit unit)
        {
            _nameText.text = unit.Name;
            HealthBar(unit.CurrentHP, unit.MaxHP);
            StaminaBar(unit.Stamina, unit.MaxStamina);
        }

        public void HealthBar(int amount, int max)
        {
            if (amount < 0) { amount = 0; }
            _healthFill.localScale = new Vector3(amount / (float)max, 1, 1);
            _healthText.text = amount.ToString();
            StartCoroutine(FillEffect(_fillPrint, _healthFill));
        }

        public void StaminaBar(int amount, int max)
        {
            Debug.Log("Stamina = " + amount + ", MaxStamina = " + max);
            if (amount < 0) { amount = 0; }
            _staminaFill.localScale = new Vector3(amount / (float)max, 1, 1);
            _staminaText.text = amount.ToString();
            StartCoroutine(FillEffect(_staminafillPrint, _staminaFill));
        }

        private IEnumerator FillEffect(RectTransform print, RectTransform fill)
        {
            for (int i = 0; i < 1;)
            {
                print.localScale = Vector3.Lerp(print.localScale, fill.localScale, 1.5f * Time.deltaTime);
                yield return null;
                if (print.localScale == fill.localScale)
                { i = 1; }
            }
        }
    }
}
