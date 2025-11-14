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
                // Attack check FIRST — ignore turn restrictions
                if (clickedPiece != null && clickedPiece.team != selectedPiece.team)
                {
                    Debug.Log($"Attempting attack on {clickedPiece.name}");
                    if (MovePiece(selectedPiece, grid))
                    {
                        DeselectPiece();
                    }
                    return;
                }

                // Normal movement
                if (clickedPiece == null)
                {
                    if (MovePiece(selectedPiece, grid))
                    {
                        DeselectPiece();
                    }
                    return;
                }

                // Switch selected piece (if same team)
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

            // --- Attack handling ---
            if (targetPiece != null && targetPiece.team != piece.team)
            {
                Debug.Log($"Battle will commence between {piece.name} ({piece.team}) and {targetPiece.name} ({targetPiece.team})");

                // KNIGHT: knights do NOT change position when initiating battle
                if (piece is KnightPiece)
                {
                    // Knight remains in place; we stage battle and switch turn
                    StartCombatScene(piece, targetPiece);
                    SwitchTurn();
                    return true;
                }

                // For non-knight pieces: calculate approach tile (one step before defender along direction)
                Vector2Int dir = target - piece.gridPosition;
                // normalize to -1, 0 or 1
                dir.x = (dir.x > 0) ? 1 : (dir.x < 0) ? -1 : 0;
                dir.y = (dir.y > 0) ? 1 : (dir.y < 0) ? -1 : 0;

                Vector2Int stopBefore = target - dir;

                // If attacker is already adjacent (stopBefore == current pos) -> allow battle without moving
                if (stopBefore == piece.gridPosition)
                {
                    // Already in approach position; do not move, just trigger battle
                    StartCombatScene(piece, targetPiece);
                    SwitchTurn();
                    return true;
                }

                // Otherwise, move to the approach tile if it's valid and free
                if (InBounds(stopBefore) && !IsOccupied(stopBefore))
                {
                    pieces.Remove(piece.gridPosition);
                    pieces[stopBefore] = piece;
                    piece.MoveTo(stopBefore);

                    SwitchTurn();
                    StartCombatScene(piece, targetPiece);
                    return true;
                }

                // Approach tile blocked -> cannot stage the attack
                Debug.Log("Cannot approach — path blocked or out of bounds.");
                return false;
            }

            // --- Normal move ---
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

        private void StartCombatScene(Piece attacker, Piece defender)
        {
            if (attacker == null || defender == null)
            {
                Debug.LogError("StartCombatScene called with missing attacker or defender!");
                return;
            }

            // SAVING BATTLE INFORMATION
            BattleData.AttackerType = attacker.name.Replace("(Clone)", "").Trim();
            BattleData.DefenderType = defender.name.Replace("(Clone)", "").Trim();

            BattleData.AttackerTeam = attacker.team;
            BattleData.DefenderTeam = defender.team;

            BattleData.DefenderPosition = defender.gridPosition;

            Debug.Log($"Battle Data Set — {BattleData.AttackerType} ({BattleData.AttackerTeam}) vs {BattleData.DefenderType} ({BattleData.DefenderTeam})");

            // Save board state before leaving
            GameStateManager.Instance?.SaveBoardState(this);

            // Load the combat scene
            // SceneManager.LoadScene("CombatScene");
            SceneManager.LoadScene("Roope Test");
        }

        // Promotion system and helpers (unchanged)
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

        private void UpdatePositions(ChessPiece combatant, Vector2Int newPlace)
        {
            GameSetter.boardOccupiers[combatant.Position] = null;
            combatant.Position = newPlace;
            GameSetter.boardOccupiers[combatant.Position] = combatant;
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
            {
                if (p != null)
                    Destroy(p.gameObject);
            }
            pieces.Clear();
        }
    }
}
