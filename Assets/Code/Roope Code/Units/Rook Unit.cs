using UnityEngine;

namespace Crognard
{
    public class RookUnit : Unit
    {
        private int _tilesMoved;
        [SerializeField] private int _damageBonus = 1;

        private void Start()
        {
            if ((int)BattleData.AttackerTeam == (int)Faction)
            {
                _tilesMoved = BattleData.AttackerTilesMoved;
                LightDamage += _damageBonus * _tilesMoved;
                MediumDamage += _damageBonus * _tilesMoved;
                HeavyDamage += _damageBonus * _tilesMoved;
            }
        }
    }
}
