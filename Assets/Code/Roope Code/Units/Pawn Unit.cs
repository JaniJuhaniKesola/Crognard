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
            LightDamage += _damageBonus * _allies;
            MediumDamage += _damageBonus * _allies;
            HeavyDamage += _damageBonus * _allies;
        }

        private void CountAllies()
        {
            Vector2Int battleSpace;

            if (GameSetter.attacker == Faction.White) { battleSpace = GameSetter.blackCombatant.Position; }
            else { battleSpace = GameSetter.whiteCombatant.Position; }

            if (Faction == GameSetter.attacker)
            {
                // Assuming that the attacker occupies the adjacent space or its original space while initiating a battle
                _allies--;
            }

            for (int i = 0; i < _adjacents.Length; i++)
            {
                if (CheckSpace(battleSpace + _adjacents[i], Faction))
                {
                    _allies++;
                }
            }
        }
        
        private bool CheckSpace(Vector2Int space, Faction faction)
        {
            // There should be static statement for every occupied space.
            /*for (int i = 0; i < GameSetter.pieces.Count; i++)
            {
                if (GameSetter.pieces[i].Position == space)
                {
                    if (GameSetter.pieces[i].Faction == faction)
                    {
                        if (GameSetter.pieces[i].Alive)
                        {
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
            }
            return false;*/

            // Implement Dictionary<Vector2Int, GameObject>

            if (GameSetter.boardOccupiers[space].Faction == faction)
            {
                if (!GameSetter.boardOccupiers[space].Alive)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
