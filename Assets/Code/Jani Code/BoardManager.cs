using System;
using System.Collections.Generic;
using UnityEngine;

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
        private Piece selectedPiece;

        // Turn system
        private PieceTeam currentTurn = PieceTeam.White;

        void Start()
        {
            GenerateBoard();
            SpawnStartingPieces();

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
                        selectedPiece = null;
                    return;
                }

                if (clickedPiece == null)
                {
                    if (MovePiece(selectedPiece, grid))
                        selectedPiece = null;
                    return;
                }

                if (clickedPiece != null && CanSelect(clickedPiece))
                {
                    SelectPiece(clickedPiece);
                }
            }
            else if (clickedPiece != null)
            {
                SelectPiece(clickedPiece);
            }
        }

        // Updated combat-enabled MovePiece
        public bool MovePiece(Piece piece, Vector2Int target)
        {
            if (!InBounds(target))
                return false;

            List<Vector2Int> legalMoves = piece.GetValidMoves();
            if (!legalMoves.Contains(target))
                return false;

            Piece targetPiece = GetPieceAt(target);

            // Combat handling BEFORE turn switching
            if (targetPiece != null && targetPiece.team != piece.team)
            {
                Debug.Log($"⚔ Battle will commence: {piece.name} vs {targetPiece.name}");

                if (piece is KnightPiece)
                {
                    pieces.Remove(piece.gridPosition);
                    pieces[target] = piece;
                    piece.MoveTo(target);
                }
                else
                {
                    Vector2Int dir = target - piece.gridPosition;
                    if (dir.x != 0) dir.x = Math.Sign(dir.x);
                    if (dir.y != 0) dir.y = Math.Sign(dir.y);

                    Vector2Int stopBefore = target - dir;

                    if (InBounds(stopBefore) && !IsOccupied(stopBefore))
                    {
                        pieces.Remove(piece.gridPosition);
                        pieces[stopBefore] = piece;
                        piece.MoveTo(stopBefore);
                    }
                }

                SwitchTurn();
                return true;
            }

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

        // Promotion System
        public void ProcessPawnPromotion(PawnPiece pawn, string type)
        {
            Vector2Int pos = pawn.gridPosition;
            pieces.Remove(pos);
            Destroy(pawn.gameObject);

            GameObject promotedGO = null;

            switch (type)
            {
                case "Knight":
                    promotedGO = Instantiate(GetPrefabFor<KnightPiece>(), transform);
                    break;
                case "Rook":
                    promotedGO = Instantiate(GetPrefabFor<RookPiece>(), transform);
                    break;
                case "Bishop":
                    promotedGO = Instantiate(GetPrefabFor<BishopPiece>(), transform);
                    break;
                case "Queen":
                default:
                    promotedGO = Instantiate(GetPrefabFor<QueenPiece>(), transform);
                    break;
            }

            Piece newPiece = promotedGO.GetComponent<Piece>();
            newPiece.team = pawn.team;
            newPiece.Initialize(pos, this);

            pieces[pos] = newPiece;
            Debug.Log($"Pawn promoted to {type}!");
        }

        private GameObject GetPrefabFor<T>() where T : Piece
        {
            T example = FindObjectOfType<T>();
            return example != null ? example.gameObject : piecePrefab;
        }
    }
}
