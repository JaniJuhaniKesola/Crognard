using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _fill;
    [SerializeField] private TextMeshProUGUI _healthText;
    private float _maxWidth;
    

    private void Start()
    {
        _maxWidth = _fill.sizeDelta.x;
    }

    public void EditHealthBar(int amount, int max)
    {
        _fill.sizeDelta = new Vector2(amount / max * _maxWidth, _fill.sizeDelta.y);
        _healthText.text = amount.ToString();
    }
}
