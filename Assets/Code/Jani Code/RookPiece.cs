using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class RookPiece : Piece
{
    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

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

            // Move until we hit board edge or another piece
            while (board.InBounds(current))
            {
                if (board.IsOccupied(current))
                {
                    // Stop when encountering another piece
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
