using UnityEngine;

namespace Crognard
{
    public class PromotionUI : MonoBehaviour
    {
        public static PromotionUI Instance;

        private PawnPiece currentPawn;

        public GameObject panel;

        private void Awake()
        {
            Instance = this;
            panel.SetActive(false);
        }

        public void Show(PawnPiece pawn)
        {
            currentPawn = pawn;
            panel.SetActive(true);
        }

        public void Hide()
        {
            panel.SetActive(false);
            currentPawn = null;
        }

        public void PromoteToKnight() => Promote("Knight");
        public void PromoteToRook() => Promote("Rook");
        public void PromoteToBishop() => Promote("Bishop");
        public void PromoteToQueen() => Promote("Queen");

        private void Promote(string type)
        {
            if (currentPawn != null)
            {
                currentPawn.Board.ProcessPawnPromotion(currentPawn, type);
            }
            Hide();
        }
    }
}
