using UnityEngine;

namespace Crognard
{
    public static class GameSetter
    {
        // Store information such as who initiated the attack,
        // which pieces participate combat and where the pieces were in a board.

        public static Faction attacker;

        // When the game moves to combat, while in board state, game has to save attacker's and defender's prefabs.
        public static GameObject blackCombatant, whiteCombatant;
    }
}
