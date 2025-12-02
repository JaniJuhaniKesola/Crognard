using UnityEngine;

namespace Crognard
{
    public class PiecePointer : MonoBehaviour
    {
        [SerializeField] private GameObject _minihub;

        private Piece _piece;

        private void Start()
        {
            _piece = GetComponent<Piece>();
            _minihub.SetActive(false);
        }
        
        private void OnMouseEnter()
        {
            Debug.Log(_piece.name + " minihub opened");
            Debug.Log(_piece.gridPosition);
            _minihub.SetActive(true);
            if (_piece.gridPosition.y >= _piece.Board.boardSize - 2 && _minihub.transform.localPosition.y > 0)
            {
                _minihub.transform.localPosition = new Vector2(_minihub.transform.localPosition.x, -_minihub.transform.localPosition.y);
            }
            else if (_piece.gridPosition.y < _piece.Board.boardSize - 2 && _minihub.transform.localPosition.y < 0)
            {
                _minihub.transform.localPosition = new Vector2(_minihub.transform.localPosition.x, -_minihub.transform.localPosition.y);
            }
        }

        private void OnMouseExit()
        {
            _minihub.SetActive(false);
        }

        private void OnMouseDown()
        {
            _piece.Board.OnPieceClicked(_piece.gridPosition);
        }
    }
}
