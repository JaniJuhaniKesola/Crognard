using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class BishopPiece : Piece
    {
        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // Diagonal directions
            Vector2Int[] directions = new Vector2Int[]
            {
                new Vector2Int(1, 1),    // up-right
                new Vector2Int(-1, 1),   // up-left
                new Vector2Int(1, -1),   // down-right
                new Vector2Int(-1, -1)   // down-left
            };

            foreach (var dir in directions)
            {
                Vector2Int current = gridPosition + dir;

                // Keep moving in this direction until blocked
                while (board.InBounds(current))
                {
                    if (board.IsOccupied(current))
                    {
                        // Stop moving further if something blocks the path
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
