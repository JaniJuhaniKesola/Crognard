using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
[RequireComponent(typeof(SpriteRenderer))]
public class Piece : MonoBehaviour
{
    [HideInInspector] public Vector2Int gridPosition;
    protected BoardManager board;
    private Coroutine moveCoroutine;
    private SpriteRenderer sr;
    private bool selected;

    [Tooltip("Seconds to move between tiles")]
    public float moveDuration = 0.12f;

    public virtual void Initialize(Vector2Int startGrid, BoardManager manager)
    {
        board = manager;
        gridPosition = startGrid;

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Pieces";
        sr.sortingOrder = 1;

        transform.position = board.GridToWorld(startGrid);
        transform.localScale = Vector3.one * (board.tileSize * board.pieceScale);
    }

    void OnMouseDown()
    {
        if (board != null)
            board.SelectPiece(this);
    }

    public void SetSelected(bool sel)
    {
        selected = sel;
        float baseScale = board.tileSize * board.pieceScale;
        transform.localScale = sel ? Vector3.one * baseScale * 1.12f : Vector3.one * baseScale;
    }

    // Keep the existing UpdateGridPosition API (keeps smooth movement)
    public void UpdateGridPosition(Vector2Int newGrid)
    {
        gridPosition = newGrid;
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(SmoothMove(board.GridToWorld(newGrid)));
    }

    IEnumerator SmoothMove(Vector3 targetWorld)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(start, targetWorld, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetWorld;
        moveCoroutine = null;
    }

    /// <summary>
    /// New: virtual MoveTo that BoardManager should call when moving a piece.
    /// Subclasses (Pawn) can override this to run extra logic on move.
    /// Default behaviour simply updates the grid position & smooth-moves the piece.
    /// </summary>
    public virtual void MoveTo(Vector2Int newGrid)
    {
        // Default behavior: just update and animate
        UpdateGridPosition(newGrid);
    }

    /// <summary>
    /// Override to provide legal move logic for each piece type
    /// </summary>
    public virtual List<Vector2Int> GetValidMoves()
    {
        return new List<Vector2Int>();
    }
}
}
