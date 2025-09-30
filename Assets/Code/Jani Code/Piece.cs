using UnityEngine;
using System.Collections;

namespace Crognard
{
    [RequireComponent(typeof(SpriteRenderer))]
public class Piece : MonoBehaviour
{
    [HideInInspector] public Vector2Int gridPosition;
    private BoardManager board;
    private Coroutine moveCoroutine;
    private SpriteRenderer sr;
    private bool selected;

    [Tooltip("Seconds to move between tiles")]
    public float moveDuration = 0.12f;

    public void Initialize(Vector2Int startGrid, BoardManager manager)
    {
        board = manager;
        gridPosition = startGrid;

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sortingLayerName = "Pieces";
        sr.sortingOrder = 1;

        transform.position = board.GridToWorld(startGrid);
        // scale relative to tile size (board handles pieceScale convention)
        transform.localScale = Vector3.one * (board.tileSize * board.pieceScale);
    }

    void OnMouseDown()
    {
        if (board != null) board.SelectPiece(this);
    }

    public void SetSelected(bool sel)
    {
        selected = sel;
        float baseScale = board.tileSize * board.pieceScale;
        transform.localScale = sel ? Vector3.one * baseScale * 1.12f : Vector3.one * baseScale;
    }

    public void UpdateGridPosition(Vector2Int newGrid)
    {
        gridPosition = newGrid;
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
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
}
}
