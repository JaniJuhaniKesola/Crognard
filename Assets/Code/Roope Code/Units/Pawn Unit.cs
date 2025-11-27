using UnityEngine;

namespace Crognard
{
    public class PawnUnit : Unit
    {
        /*
        Pawn's damage depends on how many allies are adjacent to target in board.
        Alone: +0
        1 Ally: +1
        2 Allies: +2
        and so-on.
        */

        private int _allies = 0;
        private Vector2Int[] _adjacents = {
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(1, -1)
        };
        [SerializeField] private int _damageBonus = 1;

        private void Start()
        {
            CountAllies();
            Debug.Log("Allies: " + _allies);
            LightDamage += _damageBonus * _allies;
            MediumDamage += _damageBonus * _allies;
            HeavyDamage += _damageBonus * _allies;
        }

        private void CountAllies()
        {
            Vector2Int battleSpace;

            if ((int)BattleData.AttackerTeam == (int)Faction)
            { battleSpace = BattleData.AttackerPos; }
            else
            { battleSpace = BattleData.DefenderPos; }

            for (int i = 0; i < _adjacents.Length; i++)
            {
                if (CheckSpace(battleSpace + _adjacents[i], Faction))
                {
                    _allies++;
                    // I don't know what happened but it seemed to fix itself
                }
            }
        }
        
        private bool CheckSpace(Vector2Int space, Faction faction)
        {
            if (BattleData.Positions.ContainsKey(space))
            {
                if ((int)BattleData.Positions[space].team == (int)faction)
                {
                    return true;
                }                
            }
            
            return false;
        }
    }
}
