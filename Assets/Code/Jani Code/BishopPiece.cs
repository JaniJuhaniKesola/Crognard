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
                    Piece occupier = board.GetPieceAt(current);

                    if (occupier == null)
                    {
                        // empty -> can move here
                        moves.Add(current);
                    }
                    else
                    {
                        // occupied -> if enemy, allow attacking that square; stop scanning afterwards
                        if (occupier.team != team)
                            moves.Add(current);
                        break;
                    }

                    current += dir;
                }
            }

            return moves;
        }
    }
}
