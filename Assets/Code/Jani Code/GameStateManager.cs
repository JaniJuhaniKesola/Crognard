using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    [System.Serializable]
    public class PieceSaveData
    {
        public string prefabName;
        public Vector2Int position;
        public PieceTeam team;
    }

    public class GameStateManager : MonoBehaviour
    {
        public static GameStateManager Instance;

        public List<PieceSaveData> savedPieces = new List<PieceSaveData>();
        public PieceTeam currentTurn = PieceTeam.White;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SaveBoardState(BoardManager board)
        {
            savedPieces.Clear();

            foreach (var kvp in board.GetAllPieces())
            {
                var piece = kvp.Value;
                var data = new PieceSaveData
                {
                    prefabName = piece.name.Replace("(Clone)", "").Trim(),
                    position = piece.gridPosition,
                    team = piece.team
                };
                savedPieces.Add(data);
            }

            currentTurn = board.GetCurrentTurn();
            Debug.Log($"[GameStateManager] Saved {savedPieces.Count} pieces.");
        }

        public void LoadBoardState(BoardManager board)
        {
            if (savedPieces.Count == 0)
            {
                Debug.LogWarning("[GameStateManager] No saved board to load.");
                return;
            }

            board.ClearBoard();

            foreach (var data in savedPieces)
            {
                GameObject prefab = Resources.Load<GameObject>(data.prefabName);
                if (prefab == null)
                {
                    Debug.LogWarning($"[GameStateManager] Could not find prefab {data.prefabName} in Resources.");
                    continue;
                }

                var pieceGO = GameObject.Instantiate(prefab, board.transform);
                var piece = pieceGO.GetComponent<Piece>();
                piece.team = data.team;
                piece.Initialize(data.position, board);

                board.RegisterPiece(data.position, piece);
            }

            board.SetTurn(currentTurn);
            Debug.Log("[GameStateManager] Board state restored.");
        }

        public void ClearSavedState()
        {
            savedPieces.Clear();
        }
    }
}
