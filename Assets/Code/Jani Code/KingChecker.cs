using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Crognard
{
    public class KingChecker : MonoBehaviour
    {
        private BoardManager board;

        private void Start()
        {
            board = FindObjectOfType<BoardManager>();
        }

        private void Update()
        {
            if (board == null)
                return;

            CheckKingsAlive();
        }

        private void CheckKingsAlive()
        {
            var pieces = board.GetAllPieces().Values;

            bool whiteKingAlive = pieces.Any(p =>
                p != null &&
                p.team == PieceTeam.White &&
                p.GetComponent<KingPiece>() != null);

            bool blackKingAlive = pieces.Any(p =>
                p != null &&
                p.team == PieceTeam.Black &&
                p.GetComponent<KingPiece>() != null);

            if (!whiteKingAlive)
            {
                Debug.Log("Black wins!");
                TriggerGameOver(PieceTeam.Black);
            }

            if (!blackKingAlive)
            {
                Debug.Log("White wins!");
                TriggerGameOver(PieceTeam.White);
            }
        }

        private void TriggerGameOver(PieceTeam winner)
        {
            Debug.Log("GAME OVER — Winner: " + winner);


            SceneManager.LoadScene("GameOver");

            // Prevent spam
            enabled = false;
        }
    }
}
