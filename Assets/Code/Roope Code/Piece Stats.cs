using UnityEngine;

namespace Crognard
{
    public class PieceStats : MonoBehaviour
    {
        [Tooltip("Here is listed all the pieces. At the start of the game they are uploaded to static GameSetter script. Here you developer should assign following data: Name, PieceType, PiecePrefab, CombatPrefab")]
        [SerializeField] private ChessPiece[] _pieces;
        private void Start()
        {
            if (GameSetter.pieces.Count < 1)
            {
                for (int i = 0; i < _pieces.Length; i++)
                {
                    GameSetter.pieces.Add(_pieces[i]);
                    Unit unit = _pieces[i].CombatPrefab.GetComponent<Unit>();
                    if (unit == null)
                    {
                        unit = _pieces[i].BoardPrefab.GetComponent<Unit>();
                    }
                    if (unit != null)
                    {
                        GameSetter.pieces[i].HP = unit.MaxHP;
                        GameSetter.pieces[i].Stamina = unit.Stamina;
                    }

                    if (!GameSetter.participants.ContainsKey(_pieces[i].Name))
                    {
                        GameSetter.participants.Add(_pieces[i].Name, _pieces[i]);
                    }
                }
            }
            
        }
    }
}
