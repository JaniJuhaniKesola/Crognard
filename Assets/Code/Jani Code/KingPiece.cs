using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class KingPiece : Piece
    {
        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // All 8 directions (one step each)
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right,
                new Vector2Int(1, 1),    // up-right
                new Vector2Int(-1, 1),   // up-left
                new Vector2Int(1, -1),   // down-right
                new Vector2Int(-1, -1)   // down-left
            };

            foreach (var dir in directions)
            {
                Vector2Int target = gridPosition + dir;

                // Skip if outside the board
                if (!board.InBounds(target))
                    continue;

                // Skip if occupied (no captures yet)
                if (board.IsOccupied(target))
                    continue;

                moves.Add(target);
            }

            return moves;
        }
    }
}
