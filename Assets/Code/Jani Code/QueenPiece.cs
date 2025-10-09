using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class QueenPiece : Piece
    {
        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // All 8 possible movement directions (horizontal, vertical, diagonal)
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
                Vector2Int current = gridPosition + dir;

                // Keep moving until out of bounds or blocked
                while (board.InBounds(current))
                {
                    if (board.IsOccupied(current))
                    {
                        // Stop when blocked by another piece
                        break;
                    }

                    moves.Add(current);
                    current += dir;
                }
            }

            return moves;
        }
    }
}
