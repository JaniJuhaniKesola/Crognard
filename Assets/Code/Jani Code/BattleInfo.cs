using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    public static class BattleData
    {
        public static string AttackerType;
        public static string DefenderType;

        public static PieceTeam AttackerTeam;
        public static PieceTeam DefenderTeam;

        // Roope's stupid plan starts now
        public static Vector2Int AttackerPos;
        public static Vector2Int DefenderPos;
        // Roope's stupid plan ends now

        
        public static Dictionary <Vector2Int, Piece> Positions;
    }
}

