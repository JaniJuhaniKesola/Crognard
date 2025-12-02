using TMPro;
using UnityEngine;

namespace Crognard
{
    public class MiniHub : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _hpFill, _staminaFill;
        [SerializeField] private TextMeshProUGUI _hpValue, _staminaValue;

        private float _ogWidth;

        private Piece _piece;

        private void Awake()
        {
            _ogWidth = _hpFill.size.x;
            _piece = GetComponentInParent<Piece>();
            EditBar(_hpValue, _hpFill, _piece.hp, _piece.maxHP);
            EditBar(_staminaValue, _staminaFill, _piece.stamina, _piece.maxStamina);
        }

        public void SetupData(int curHP, int maxHP, int curSta, int maxSta)
        {
            EditBar(_hpValue, _hpFill, curHP, maxHP);
            EditBar(_staminaValue, _staminaFill, curSta, maxSta);
        }

        private void EditBar(TextMeshProUGUI value, SpriteRenderer fill, int current, int max)
        {
            fill.size = new Vector2(current/(float)max * _ogWidth, fill.size.y);
            value.text = current.ToString();
        }
    }
}
