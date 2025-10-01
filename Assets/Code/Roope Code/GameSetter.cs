using UnityEngine;

namespace Crognard
{
    public static class GameSetter
    {
        // Store information such as who initiated the attack,
        // which pieces participate combat and where the pieces were in a board.
        #region Combat Phase
        public static Faction attacker;

        // When the game moves to combat, while in board state, game has to save attacker's and defender's prefabs.
        public static GameObject blackCombatant, whiteCombatant;
        #endregion

        #region Board Phase
        // Information that should be saved while combat phase starts.

        // Maybe which tiles has which pieces
        #endregion

        #region Pieces
        // Save each individual stat changes here. Like HP that is left.
        /*
        public static int wP1HP, wP2HP, wP3HP, wP4HP, wP5HP, wP6HP, wP7HP, wP8HP;
        public static int wR1HP, wR2HP, wH1HP, wH2HP, wB1HP, wB2HP, wQHP, wKHP;

        public static int bP1HP, bP2HP, bP3HP, bP4HP, bP5HP, bP6HP, bP7HP, bP8HP;
        public static int bR1HP, bR2HP, bH1HP, bH2HP, bB1HP, bB2HP, bQHP, bKHP;
        */
        
        #endregion
    }
}
