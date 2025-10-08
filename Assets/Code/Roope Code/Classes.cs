using System;
using UnityEngine;

namespace Crognard
{
    [Serializable]
    public class ChessPiece
    {
        public string Name;                 // ID or name for the piece
        public Faction Faction;             // Black or White
        public Character PieceType;         // Type of piece: pawn, knight so on...
        public bool Alive;                  // Alive = true & Dead = false;
        public int HP;                      // Remaining Health
        public int Stamina;                 // Remaining Stamina
        public Vector2Int Position;         // Position in chess board
        public GameObject BoardPrefab;      // Prefab during board phase
        public GameObject CombatPrefab;     // Prefab during combat phase
    }

    public class Act
    {
        public Faction faction;
        public ActionType action;
        public int priority;

        public Act(Faction faction, ActionType action, int priority)
        {
            this.faction = faction;
            this.action = action;
            this.priority = priority;
        }
    }

    [Serializable]
    public class Action
    {
        public int priority;
        public int staminaCost;
        public Action(int priority, int staminaCost)
        {
            this.priority = priority;
            this.staminaCost = staminaCost;
        }
    }
}
