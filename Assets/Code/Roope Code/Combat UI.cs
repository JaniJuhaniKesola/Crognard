using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private RectTransform _healthFill;
    [SerializeField] private TextMeshProUGUI _healthText;

    public void SetupInfo(Unit unit)
    {
        _nameText.text = unit.Name;
        HealthBar(unit.CurrentHP, unit.MaxHP);
    }

    public void HealthBar(int amount, int max)
    {
        _healthFill.localScale = new Vector3(amount / (float)max, 1, 1);
        _healthText.text = amount.ToString();
    }
}
