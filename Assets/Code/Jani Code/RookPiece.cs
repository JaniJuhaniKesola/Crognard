using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class RookPiece : Piece
    {
        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // Four cardinal directions (horizontal and vertical)
            Vector2Int[] directions = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.down,
                Vector2Int.left,
                Vector2Int.right
            };

            foreach (var dir in directions)
            {
                Vector2Int current = gridPosition + dir;

                // Move until we hit the board edge or another piece
                while (board.InBounds(current))
                {
                    Piece occupier = board.GetPieceAt(current);

                    if (occupier == null)
                    {
                        // empty square -> valid move
                        moves.Add(current);
                    }
                    else
                    {
                        // occupied -> if enemy, we can attack that tile; either way we stop scanning
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
