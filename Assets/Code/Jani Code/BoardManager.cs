using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crognard
{
    public class BoardManager : MonoBehaviour
    {
        [Header("Board Settings")]
        public int boardSize = 8;
        public float tileSize = 1f;
        public GameObject tilePrefab;

        [Header("Visuals")]
        public Color lightColor = new Color(0.9f, 0.9f, 0.9f);
        public Color darkColor = new Color(0.2f, 0.2f, 0.2f);

        [Header("Piece Settings")]
        public GameObject piecePrefab;
        [Range(0.1f, 2f)]
        public float pieceScale = 0.9f;

        [Serializable]
        public struct PieceSpawnData
        {
            public Vector2Int position;
            public GameObject prefab;
        }

        public List<PieceSpawnData> startingPieces = new List<PieceSpawnData>();

        private Tile[,] tiles;
        private Dictionary<Vector2Int, Piece> pieces = new Dictionary<Vector2Int, Piece>();

        public Dictionary<Vector2Int, Piece> GetAllPieces() => pieces;
        public PieceTeam GetCurrentTurn() => currentTurn;
        private Piece selectedPiece;

        private PieceTeam currentTurn = PieceTeam.White;

        [Header("Promotion Prefabs — WHITE")]
        public GameObject whiteQueenPrefab;
        public GameObject whiteRookPrefab;
        public GameObject whiteBishopPrefab;
        public GameObject whiteKnightPrefab;

        [Header("Promotion Prefabs — BLACK")]
        public GameObject blackQueenPrefab;
        public GameObject blackRookPrefab;
        public GameObject blackBishopPrefab;
        public GameObject blackKnightPrefab;
        /*  ------------------------------ */



        void Start()
        {
            GenerateBoard();

            if (GameStateManager.Instance != null && GameStateManager.Instance.savedPieces.Count > 0)
            {
                GameStateManager.Instance.LoadBoardState(this);
            }
            else
            {
                SpawnStartingPieces();
            }

            Debug.Log($"Game Start — {currentTurn} goes first.");
        }

        void GenerateBoard()
        {
            tiles = new Tile[boardSize, boardSize];
            Vector2 origin = new Vector2(-boardSize / 2f + 0.5f, -boardSize / 2f + 0.5f);

            for (int x = 0; x < boardSize; x++)
            {
                for (int y = 0; y < boardSize; y++)
                {
                    GameObject go = Instantiate(tilePrefab, transform);
                    go.name = $"Tile_{x}_{y}";

                    Vector3 worldPos = new Vector3((x + origin.x) * tileSize, (y + origin.y) * tileSize, 0f);
                    go.transform.position = worldPos;
                    go.transform.localScale = Vector3.one * tileSize;

                    SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                    sr.color = ((x + y) % 2 == 0) ? lightColor : darkColor;
                    sr.sortingLayerName = "Board";

                    Tile tile = go.GetComponent<Tile>();
                    if (tile == null) tile = go.AddComponent<Tile>();
                    tile.Setup(new Vector2Int(x, y), this);

                    tiles[x, y] = tile;
                }
            }
        }

        void SpawnStartingPieces()
        {
            foreach (var spawn in startingPieces)
            {
                if (!InBounds(spawn.position))
                {
                    Debug.LogWarning($"Spawn: {spawn.position} out of bounds, skipping.");
                    continue;
                }

                GameObject prefabToUse = spawn.prefab != null ? spawn.prefab : piecePrefab;
                if (prefabToUse == null)
                {
                    Debug.LogWarning("Spawn: No prefab assigned.");
                    continue;
                }

                SpawnPiece(spawn.position, prefabToUse);
            }
        }

        public void SpawnPiece(Vector2Int gridPos, GameObject prefabOverride = null)
        {
            if (!InBounds(gridPos)) return;
            if (IsOccupied(gridPos)) return;

            GameObject prefabToUse = prefabOverride != null ? prefabOverride : piecePrefab;
            if (prefabToUse == null) return;

            GameObject go = Instantiate(prefabToUse, transform);
            go.transform.position = GridToWorld(gridPos);
            go.transform.localScale = Vector3.one * (tileSize * pieceScale);

            Piece p = go.GetComponent<Piece>();
            if (p == null) p = go.AddComponent<Piece>();
            p.Initialize(gridPos, this);

            pieces[gridPos] = p;
        }

        public bool InBounds(Vector2Int p) => p.x >= 0 && p.x < boardSize && p.y >= 0 && p.y < boardSize;
        public Tile GetTile(Vector2Int grid) => InBounds(grid) ? tiles[grid.x, grid.y] : null;
        public Vector3 GridToWorld(Vector2Int grid)
        {
            Vector2 origin = new Vector2(-boardSize / 2f + 0.5f, -boardSize / 2f + 0.5f);
            return new Vector3((grid.x + origin.x) * tileSize, (grid.y + origin.y) * tileSize, 0f);
        }
        public bool IsOccupied(Vector2Int pos) => pieces.ContainsKey(pos);
        public Piece GetPieceAt(Vector2Int pos) => pieces.TryGetValue(pos, out Piece p) ? p : null;

        private void SwitchTurn()
        {
            currentTurn = (currentTurn == PieceTeam.White) ? PieceTeam.Black : PieceTeam.White;
            Debug.Log($"Turn: {currentTurn}");
        }

        private bool CanSelect(Piece piece)
        {
            return piece != null && piece.team == currentTurn;
        }

        public void SelectPiece(Piece p)
        {
            if (p == null) return;
            if (!CanSelect(p))
            {
                Debug.Log($"Wrong turn: Cannot select {p.team}");
                return;
            }

            if (selectedPiece != null) selectedPiece.SetSelected(false);
            selectedPiece = p;
            p.SetSelected(true);

            Debug.Log($"Selected: {p.name} at {p.gridPosition}");
        }

        public void DeselectPiece()
        {
            if (selectedPiece != null)
                selectedPiece.SetSelected(false);
            selectedPiece = null;
        }

        public void OnTileClicked(Tile tile)
        {
            Vector2Int grid = tile.gridPosition;
            Piece clickedPiece = GetPieceAt(grid);

            if (selectedPiece != null)
            {
                if (clickedPiece != null && clickedPiece.team != selectedPiece.team)
                {
                    if (MovePiece(selectedPiece, grid))
                        DeselectPiece();
                    return;
                }

                if (clickedPiece == null)
                {
                    if (MovePiece(selectedPiece, grid))
                        DeselectPiece();
                    return;
                }

                if (clickedPiece != null && CanSelect(clickedPiece))
                {
                    SelectPiece(clickedPiece);
                    return;
                }
            }
            else if (clickedPiece != null)
            {
                SelectPiece(clickedPiece);
            }
        }

        public bool MovePiece(Piece piece, Vector2Int target)
        {
            if (!InBounds(target))
                return false;

            List<Vector2Int> legalMoves = piece.GetValidMoves();
            if (!legalMoves.Contains(target))
                return false;

            Piece targetPiece = GetPieceAt(target);

            // Normal move
            if (targetPiece == null)
            {
                pieces.Remove(piece.gridPosition);
                pieces[target] = piece;
                piece.MoveTo(target);
                SwitchTurn();
                return true;
            }

            return false;
        }

        public void ProcessPawnPromotion(PawnPiece pawn, string type)
        {
            Vector2Int pos = pawn.gridPosition;

            pieces.Remove(pos);
            Destroy(pawn.gameObject);

            GameObject prefab = null;

            bool isWhite = pawn.team == PieceTeam.White;

            switch (type)
            {
                case "Knight":
                    prefab = isWhite ? whiteKnightPrefab : blackKnightPrefab;
                    break;
                case "Rook":
                    prefab = isWhite ? whiteRookPrefab : blackRookPrefab;
                    break;
                case "Bishop":
                    prefab = isWhite ? whiteBishopPrefab : blackBishopPrefab;
                    break;
                case "Queen":
                default:
                    prefab = isWhite ? whiteQueenPrefab : blackQueenPrefab;
                    break;
            }

            if (prefab == null)
            {
                Debug.LogError($"Missing promotion prefab for {type} ({pawn.team})!");
                return;
            }

            GameObject newGO = Instantiate(prefab, transform);
            Piece newPiece = newGO.GetComponent<Piece>();

            newPiece.team = pawn.team;
            newPiece.Initialize(pos, this);
            pieces[pos] = newPiece;

            Debug.Log($"Pawn promoted to: {type} ({pawn.team})");
        }

        public void SetTurn(PieceTeam team)
        {
            currentTurn = team;
        }

        public void RegisterPiece(Vector2Int pos, Piece piece)
        {
            pieces[pos] = piece;
        }

        public void ClearBoard()
        {
            foreach (var p in pieces.Values)
                if (p != null)
                    Destroy(p.gameObject);

            pieces.Clear();
        }
    }
}
