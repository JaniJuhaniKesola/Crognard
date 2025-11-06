using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    public static class GameSetter
    {
        // Store information such as who initiated the attack,
        // which pieces participate combat and where the pieces were in a board.
        #region Combat Phase
        public static Faction attacker; // Attacker unit's Faction -> Can be found from Unit Script.

        // When the game moves to combat, while in board state, game has to save attacker's and defender's prefabs.
        public static ChessPiece blackCombatant, whiteCombatant;
        public static int blackCombatID, whiteCombatID; // When you take pieces to combat from list, save these when entering Combat so after the fight we can update the right item in the list.

        public static ChessPiece defeated, doubleKill;  // Most of the times there is only one defeated character.
        #endregion

        #region Board Phase
        // Information that should be saved while combat phase starts.

        // Maybe which tiles has which pieces

        // Number of allies adjacent to enemy target. Pawns like their friends.

        public static Dictionary<Vector2Int, ChessPiece> boardOccupiers;

        public static Dictionary<string, ChessPiece> participants;

        #endregion

        #region Pieces
        // Save each individual stat changes here. Like HP that is left.
        /*
        Class ChessPiece
        -   public string Name;                 // ID or name for the piece
        -   public Faction Faction;             // Black or White
        -   public Character PieceType;         // Type of piece: pawn, knight so on...
        -   public bool Alive;                  // Alive = true & Dead = false;
        -   public int HP;                      // Remaining Health
        -   public int Stamina;                 // Remaining Stamina
        -   public Vector2Int Position;         // Position in chess board
        -   public GameObject PiecePrefab;      // Prefab during board phase
        -   public GameObject CombatPrefab;     // Prefab during combat phase
        */
        public static List<ChessPiece>pieces;

        #endregion

        // At the start of the game each piece is saved here

    }
}
