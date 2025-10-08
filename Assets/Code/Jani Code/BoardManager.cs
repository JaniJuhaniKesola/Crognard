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

            if (startingPieces.Count == 0 && piecePrefab != null)
            {
                if (!IsOccupied(new Vector2Int(0, 0)) && InBounds(new Vector2Int(0, 0)))
                    SpawnPiece(new Vector2Int(0, 0), piecePrefab);
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
                    if (sr == null) sr = go.AddComponent<SpriteRenderer>();
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
                    Debug.LogWarning($"BoardManager: spawn position {spawn.position} out of bounds, skipping.");
                    continue;
                }

                GameObject prefabToUse = spawn.prefab != null ? spawn.prefab : piecePrefab;
                if (prefabToUse == null)
                {
                    Debug.LogWarning("BoardManager: no prefab provided for starting piece and no default piecePrefab assigned.");
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
            if (prefabToUse == null)
            {
                Debug.LogWarning("SpawnPiece: no prefab assigned.");
                return;
            }

            GameObject go = Instantiate(prefabToUse, transform);
            go.transform.position = GridToWorld(gridPos);
            go.transform.localScale = Vector3.one * (tileSize * pieceScale);

            Piece p = go.GetComponent<Piece>();
            if (p == null) p = go.AddComponent<Piece>();
            p.Initialize(gridPos, this);

            pieces[gridPos] = p;
        }

        public bool InBounds(Vector2Int p) => p.x >= 0 && p.x < boardSize && p.y >= 0 && p.y < boardSize;
        public Tile GetTile(Vector2Int gridPos) => InBounds(gridPos) ? tiles[gridPos.x, gridPos.y] : null;
        public Vector3 GridToWorld(Vector2Int grid)
        {
            Vector2 origin = new Vector2(-boardSize / 2f + 0.5f, -boardSize / 2f + 0.5f);
            return new Vector3((grid.x + origin.x) * tileSize, (grid.y + origin.y) * tileSize, 0f);
        }
        public bool IsOccupied(Vector2Int pos) => pieces.ContainsKey(pos);
        public Piece GetPieceAt(Vector2Int pos) => pieces.TryGetValue(pos, out Piece p) ? p : null;

        // TURN SYSTEM
        private void SwitchTurn()
        {
            currentTurn = (currentTurn == PieceTeam.White) ? PieceTeam.Black : PieceTeam.White;
            Debug.Log($"Turn switched: It is now {currentTurn}'s turn.");
        }

        private bool CanSelect(Piece piece)
        {
            return piece != null && piece.team == currentTurn;
        }

        // -------------------------
        // Selection helpers (RESTORED)
        // -------------------------
        public void SelectPiece(Piece p)
        {
            if (p == null) return;
            if (!CanSelect(p))
            {
                Debug.Log($"It's {currentTurn}'s turn — cannot select {p.team} piece.");
                return;
            }

            if (selectedPiece != null) selectedPiece.SetSelected(false);
            selectedPiece = p;
            if (selectedPiece != null) selectedPiece.SetSelected(true);
            Debug.Log($"Selected {p.name} at {p.gridPosition}");
        }

        public void DeselectPiece()
        {
            if (selectedPiece != null) selectedPiece.SetSelected(false);
            selectedPiece = null;
        }

        // -------------------------------------------
        // MAIN INPUT LOGIC (attacks checked first)
        // -------------------------------------------
        public void OnTileClicked(Tile tile)
        {
            Vector2Int grid = tile.gridPosition;
            Piece clickedPiece = GetPieceAt(grid);

            // If there is a selected piece, try attack first (if enemy)
            if (selectedPiece != null)
            {
                // CASE 1: clicked on enemy piece -> attempt attack
                if (clickedPiece != null && clickedPiece.team != selectedPiece.team)
                {
                    List<Vector2Int> legalMoves = selectedPiece.GetValidMoves();
                    if (legalMoves.Contains(grid))
                    {
                        if (MovePiece(selectedPiece, grid))
                        {
                            selectedPiece.SetSelected(false);
                            selectedPiece = null;
                            SwitchTurn();
                        }
                        return;
                    }
                }

                // CASE 2: clicked on empty tile -> attempt normal move
                if (clickedPiece == null)
                {
                    if (MovePiece(selectedPiece, grid))
                    {
                        selectedPiece.SetSelected(false);
                        selectedPiece = null;
                        SwitchTurn();
                    }
                    return;
                }

                // CASE 3: clicked on ally piece -> select it (only if same team)
                if (clickedPiece != null && CanSelect(clickedPiece))
                {
                    SelectPiece(clickedPiece);
                }
            }
            else
            {
                // No piece selected yet: select the clicked piece if it's your turn
                if (clickedPiece != null && CanSelect(clickedPiece))
                {
                    SelectPiece(clickedPiece);
                }
            }
        }

        // Attempt to move a piece; returns true if moved
        public bool MovePiece(Piece piece, Vector2Int target)
        {
            if (!InBounds(target))
                return false;

            List<Vector2Int> legalMoves = piece.GetValidMoves();
            if (!legalMoves.Contains(target))
                return false;

            Piece targetPiece = GetPieceAt(target);

            // ---- Combat check ----
            if (targetPiece != null)
            {
                if (targetPiece.team != piece.team)
                {
                    Debug.Log($"Combat triggered! {piece.name} ({piece.team}) attacks {targetPiece.name} ({targetPiece.team})");

                    // Knight exception: it jumps directly onto the target
                    if (piece is KnightPiece)
                    {
                        pieces.Remove(piece.gridPosition);
                        pieces[target] = piece;
                        piece.MoveTo(target);
                    }
                    else
                    {
                        // Stop one tile before target in same direction
                        Vector2Int dir = (target - piece.gridPosition);
                        if (dir.x != 0) dir.x = System.Math.Sign(dir.x);
                        if (dir.y != 0) dir.y = System.Math.Sign(dir.y);
                        Vector2Int stopBefore = target - dir;

                        if (InBounds(stopBefore) && !IsOccupied(stopBefore))
                        {
                            pieces.Remove(piece.gridPosition);
                            pieces[stopBefore] = piece;
                            piece.MoveTo(stopBefore);
                        }
                    }

                    Debug.Log($"--> Scene transition placeholder: battle between {piece.name} and {targetPiece.name}");
                    return true;
                }
                return false;
            }

            // ---- Normal movement (no combat) ----
            pieces.Remove(piece.gridPosition);
            pieces[target] = piece;
            piece.MoveTo(target);
            return true;
        }
    }
}
