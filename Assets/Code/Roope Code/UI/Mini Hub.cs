using TMPro;
using UnityEngine;

namespace Crognard
{
    public class MiniHub : MonoBehaviour
    {
        [SerializeField] private RectTransform _hpFill, _staminaFill;
        [SerializeField] private TextMeshProUGUI _hpValue, _staminaValue;

        public void SetupData(int curHP, int maxHP, int curSta, int maxSta)
        {
            EditBar(_hpFill, curHP, maxHP);
            EditBar(_staminaFill, curSta, maxSta);
        }

        private void EditBar(RectTransform fill, int current, int max)
        {
            fill.localScale = new Vector2(current/(float)max, 1);
        }
    }
}
