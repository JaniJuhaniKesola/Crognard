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
            _minihub.SetActive(true);
            if (_piece.gridPosition.y > 6 || _minihub.transform.position.y > 0)
            {
                _minihub.transform.position = new Vector2(_minihub.transform.position.x, -_minihub.transform.position.y);
            }
            else if (_piece.gridPosition.y < 6 || _minihub.transform.position.y < 0)
            {
                _minihub.transform.position = new Vector2(_minihub.transform.position.x, -_minihub.transform.position.y);
            }
        }

        private void OnMouseExit()
        {
            _minihub.SetActive(false);
        }
    }
}
