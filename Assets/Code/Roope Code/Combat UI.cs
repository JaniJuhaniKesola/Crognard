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

        public void SetupInfo(Unit unit)
        {
            _nameText.text = unit.Name;
            HealthBar(unit.CurrentHP, unit.MaxHP);
        }

        public void HealthBar(int amount, int max)
        {
            if (amount < 0) { amount = 0; }
            _healthFill.localScale = new Vector3(amount / (float)max, 1, 1);
            _healthText.text = amount.ToString();
            StartCoroutine(FillEffect());
        }

        private IEnumerator FillEffect()
        {
            for (int i = 0; i < 1;)
            {
                _fillPrint.localScale = Vector3.Lerp(_fillPrint.localScale, _healthFill.localScale, 1.5f * Time.deltaTime);
                yield return null;
                if (_fillPrint.localScale == _healthFill.localScale)
                { i = 1; }
            }
        }
    }
}
