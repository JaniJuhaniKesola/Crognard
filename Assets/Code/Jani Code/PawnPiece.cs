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

            Vector2Int forward = (team == PieceTeam.White) ? Vector2Int.up : Vector2Int.down;
            Vector2Int oneStep = gridPosition + forward;

            if (board.InBounds(oneStep) && !board.IsOccupied(oneStep))
            {
                moves.Add(oneStep);

                Vector2Int twoStep = gridPosition + forward * 2;
                if (!hasMoved && board.InBounds(twoStep) && !board.IsOccupied(twoStep))
                    moves.Add(twoStep);
            }

            Vector2Int[] attackDirs =
            {
                new Vector2Int(1, forward.y),
                new Vector2Int(-1, forward.y)
            };

            foreach (var dir in attackDirs)
            {
                Vector2Int target = gridPosition + dir;
                if (!board.InBounds(target))
                    continue;

                Piece targetPiece = board.GetPieceAt(target);
                if (targetPiece != null && targetPiece.team != team)
                {
                    moves.Add(target);
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
