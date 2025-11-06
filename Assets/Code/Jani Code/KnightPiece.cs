using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class KnightPiece : Piece
    {
        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // All possible knight jumps (L-shapes)
            Vector2Int[] jumpOffsets = new Vector2Int[]
            {
                new Vector2Int( 1,  2),
                new Vector2Int( 2,  1),
                new Vector2Int( 2, -1),
                new Vector2Int( 1, -2),
                new Vector2Int(-1, -2),
                new Vector2Int(-2, -1),
                new Vector2Int(-2,  1),
                new Vector2Int(-1,  2),
            };

            foreach (var offset in jumpOffsets)
            {
                Vector2Int target = gridPosition + offset;

                // Skip if outside the board
                if (!board.InBounds(target))
                    continue;

                Piece occupier = board.GetPieceAt(target);

                // Knight can jump over pieces; square is valid if empty or contains an enemy
                if (occupier == null || occupier.team != team)
                    moves.Add(target);
            }

            return moves;
        }
    }
}
