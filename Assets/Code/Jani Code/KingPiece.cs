using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class KingPiece : Piece
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
                Vector2Int target = gridPosition + dir;

                if (!board.InBounds(target))
                    continue;

                Piece targetPiece = board.GetPieceAt(target);
                if (targetPiece == null || targetPiece.team != team)
                {
                    // Empty or enemy — can move or attack
                    moves.Add(target);
                }
            }

            return moves;
        }
    }
}
