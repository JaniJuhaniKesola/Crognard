using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class QueenPiece : Piece
    {
        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            Vector2Int[] directions = new Vector2Int[]
            {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
        new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1)
            };

            foreach (var dir in directions)
            {
                Vector2Int current = gridPosition + dir;

                while (board.InBounds(current))
                {
                    Piece targetPiece = board.GetPieceAt(current);
                    if (targetPiece != null)
                    {
                        if (targetPiece.team != team)
                        {
                            // Enemy found — can attack this tile
                            moves.Add(current);
                        }
                        // Stop scanning past this tile either way
                        break;
                    }

                    // Empty tile — valid move
                    moves.Add(current);
                    current += dir;
                }
            }

            return moves;
        }
    }
}
