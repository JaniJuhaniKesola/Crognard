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
        public Action action;
        public int priority;

        public Act(Faction faction, Action action, int priority)
        {
            this.faction = faction;
            this.action = action;
            this.priority = priority;
        }
    }

    [Serializable]
    public class Action1
    {
        public int priority;
        public int stamina;
        public int damage;
    }
}
