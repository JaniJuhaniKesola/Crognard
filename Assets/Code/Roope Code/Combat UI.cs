using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    public TextMeshProUGUI _nameText;
    private HealthBar _healthBar;

    public void SetupInfo(Unit unit)
    {
        _nameText.text = unit.Name;
        _healthBar.EditHealthBar(unit.CurrentHP, unit.MaxHP);
    }
}
