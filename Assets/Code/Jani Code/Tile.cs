using UnityEngine;

namespace Crognard
{
    public class Tile : MonoBehaviour
    {
        public Vector2Int gridPosition;
        private BoardManager board;

        public void Setup(Vector2Int pos, BoardManager manager)
        {
            gridPosition = pos;
            board = manager;
        }

        private void OnMouseDown()
        {
            if (board == null) return;
            board.OnTileClicked(this); // Always click through tile
        }
    }
}
