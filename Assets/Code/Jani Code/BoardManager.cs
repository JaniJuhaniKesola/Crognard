using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    public class BoardManager : MonoBehaviour
{
    [Header("Board Settings")]
    public int boardSize = 8;          // Width & height of the board
    public float tileSize = 1f;        // Size of each tile (world units)
    public GameObject tilePrefab;      // Tile prefab (should be a 1x1 sprite)

    [Header("Visuals")]
    public Color lightColor = new Color(0.9f, 0.9f, 0.9f);
    public Color darkColor  = new Color(0.2f, 0.2f, 0.2f);

    [Header("Piece Settings")]
    public GameObject piecePrefab;     // Default piece prefab
    [Range(0.1f, 2f)]
    public float pieceScale = 0.9f;    // Fraction of tileSize (0.9 = 90% of tile)

    [System.Serializable]
    public struct PieceSpawnData
    {
        public Vector2Int position;    // Grid position to spawn at (x,y)
        public GameObject prefab;      // Optional prefab override (if null, uses piecePrefab)
    }

    public List<PieceSpawnData> startingPieces = new List<PieceSpawnData>();

    // internal state
    private Tile[,] tiles;
    private Dictionary<Vector2Int, Piece> pieces = new Dictionary<Vector2Int, Piece>();
    private Piece selectedPiece;

    void Start()
    {
        GenerateBoard();
        SpawnStartingPieces();

        // optional fallback: spawn a single piece at (0,0) if nothing configured
        if (startingPieces.Count == 0 && piecePrefab != null)
        {
            if (!IsOccupied(new Vector2Int(0, 0)) && InBounds(new Vector2Int(0, 0)))
                SpawnPiece(new Vector2Int(0, 0), piecePrefab);
        }
    }

    void GenerateBoard()
    {
        tiles = new Tile[boardSize, boardSize];

        // origin so board is centered around (0,0)
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
        if (!InBounds(gridPos))
        {
            Debug.LogWarning($"SpawnPiece: {gridPos} out of bounds.");
            return;
        }

        if (IsOccupied(gridPos))
        {
            Debug.LogWarning($"SpawnPiece: {gridPos} already occupied.");
            return;
        }

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

    public Tile GetTile(Vector2Int gridPos)
    {
        if (!InBounds(gridPos)) return null;
        return tiles[gridPos.x, gridPos.y];
    }

    public Vector3 GridToWorld(Vector2Int grid)
    {
        Vector2 origin = new Vector2(-boardSize / 2f + 0.5f, -boardSize / 2f + 0.5f);
        return new Vector3((grid.x + origin.x) * tileSize, (grid.y + origin.y) * tileSize, 0f);
    }

    public bool IsOccupied(Vector2Int pos) => pieces.ContainsKey(pos);

    public Piece GetPieceAt(Vector2Int pos)
    {
        pieces.TryGetValue(pos, out Piece p);
        return p;
    }

    // Selection API used by Piece.OnMouseDown
    public void SelectPiece(Piece p)
    {
        if (selectedPiece != null) selectedPiece.SetSelected(false);
        selectedPiece = p;
        if (selectedPiece != null) selectedPiece.SetSelected(true);
    }

    // Called by Tile when clicked
    public void OnTileClicked(Tile tile)
    {
        Vector2Int grid = tile.gridPosition;

        if (selectedPiece != null)
        {
            // try move selected piece to clicked tile
            if (MovePiece(selectedPiece, grid))
            {
                selectedPiece.SetSelected(false);
                selectedPiece = null;
            }
            else
            {
                // if occupied, select the piece on that tile
                Piece other = GetPieceAt(grid);
                if (other != null) SelectPiece(other);
            }
        }
        else
        {
            // if no piece selected, select piece on clicked tile (if exists)
            Piece p = GetPieceAt(grid);
            if (p != null) SelectPiece(p);
        }
    }

    // Attempt to move a piece; returns true if moved
    public bool MovePiece(Piece piece, Vector2Int target)
    {
        if (!InBounds(target)) return false;

        if (GetPieceAt(target) != null) return false; // simple rule: can't move into occupied

        Vector2Int from = piece.gridPosition;
        pieces.Remove(from);
        pieces[target] = piece;
        piece.UpdateGridPosition(target);
        return true;
    }
}
}