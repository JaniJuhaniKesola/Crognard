using UnityEngine;
using System.Collections.Generic;

namespace Crognard
{
    public class PawnPiece : Piece
    {
        private bool hasMoved = false;

        public override List<Vector2Int> GetValidMoves()
        {
            List<Vector2Int> moves = new List<Vector2Int>();

            // White pawns move up, black pawns move down
            Vector2Int forward = (team == PieceTeam.White) ? Vector2Int.up : Vector2Int.down;

            Vector2Int oneStep = gridPosition + forward;

            // Move one step forward if not blocked
            if (board.InBounds(oneStep) && !board.IsOccupied(oneStep))
            {
                moves.Add(oneStep);

                // Two-step if first move and both tiles clear
                Vector2Int twoStep = gridPosition + forward * 2;
                if (!hasMoved && board.InBounds(twoStep) && !board.IsOccupied(twoStep))
                {
                    moves.Add(twoStep);
                }
            }

            return moves;
        }

        public override void MoveTo(Vector2Int newPos)
{
    base.MoveTo(newPos);
    hasMoved = true;

    if ((team == PieceTeam.White && newPos.y == board.boardSize - 1) ||
        (team == PieceTeam.Black && newPos.y == 0))
    {
        Debug.Log("Pawn promotion triggered!");
        PromotionUI.Instance.Show(this);
    }
}
    }
}
