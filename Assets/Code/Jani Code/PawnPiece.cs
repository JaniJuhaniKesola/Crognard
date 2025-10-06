using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class PawnPiece : Piece
{
    private bool hasMoved = false; // Track if pawn already moved

    public override List<Vector2Int> GetValidMoves()
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // For now forward = up; we'll add color/team later
        Vector2Int forward = Vector2Int.up;

        Vector2Int oneStep = gridPosition + forward;

        // Move one forward if empty
        if (board.InBounds(oneStep) && !board.IsOccupied(oneStep))
        {
            moves.Add(oneStep);

            // If pawn hasn't moved, it can move two tiles from its starting square
            Vector2Int twoStep = gridPosition + forward * 2;
            if (!hasMoved && board.InBounds(twoStep) && !board.IsOccupied(twoStep))
            {
                moves.Add(twoStep);
            }
        }

        return moves;
    }

    // Called by BoardManager when the pawn is actually moved
    public override void MoveTo(Vector2Int newPos)
    {
        base.MoveTo(newPos); // does the smooth movement & gridPosition update
        hasMoved = true;     // now mark it as moved so double-step is disabled next time
    }
}
}
