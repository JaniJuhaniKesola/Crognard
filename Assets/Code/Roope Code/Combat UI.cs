using System.Collections;
using TMPro;
using UnityEngine;

namespace Crognard
{
    public class CombatUI : MonoBehaviour
    {
        [SerializeField] private NameTag _nameText;
        [SerializeField] private RectTransform _healthFill;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private RectTransform _staminaFill;
        [SerializeField] private TextMeshProUGUI _staminaText;

        private bool setup = true;

        public void SetupInfo(Unit unit)
        {
            setup = true;
            _nameText.NewName(unit.Name);
            HealthBar(unit.CurrentHP, unit.MaxHP);
            StaminaBar(unit.Stamina, unit.MaxStamina);
            setup = false;
        }

        public void HealthBar(int amount, int max)
        {
            if (amount < 0) { amount = 0; }
            if (setup)
            {
                _healthFill.localScale = new Vector3(amount / (float)max, 1, 1);
            }
            else
            {
                StartCoroutine(FastFill(_healthFill, amount, max));
            }
            
            _healthText.text = amount.ToString();
            
        }

        public void StaminaBar(int amount, int max)
        {
            if (amount < 0) { amount = 0; }
            if (setup)
            {
                _staminaFill.localScale = new Vector3(amount / (float)max, 1, 1);
            }
            else
            {
                StartCoroutine(FastFill(_staminaFill, amount, max));
            }
            _staminaText.text = amount.ToString();
        }

        private IEnumerator FastFill(RectTransform print, int amount, int max)
        {
            for (int i = 0; i < 1;)
            {
                print.localScale = Vector3.MoveTowards(print.localScale, new Vector3(amount / (float)max, 1, 1), 0.5f * Time.deltaTime);
                yield return null;
                if (print.localScale.x == amount / (float)max)
                { i = 1; }
            }
        }
    }
}
