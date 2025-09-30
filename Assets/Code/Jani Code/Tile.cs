using UnityEngine;

namespace Crognard
{
    [RequireComponent(typeof(SpriteRenderer))]
public class Tile : MonoBehaviour
{
    [HideInInspector] public Vector2Int gridPosition;
    private BoardManager board;

    public void Setup(Vector2Int pos, BoardManager manager)
    {
        gridPosition = pos;
        board = manager;

        // Ensure a BoxCollider2D exists and is sized to 1x1 in local space.
        // We rely on transform.localScale (set by BoardManager) to scale the collider to tileSize.
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        if (bc == null) bc = gameObject.AddComponent<BoxCollider2D>();
        bc.size = Vector2.one;
        bc.offset = Vector2.zero;
        // Make sure it's a trigger only if you want clicks to pass through to raycasts:
        // bc.isTrigger = true; // optional
    }

    void OnMouseDown()
    {
        // Delegate to board manager
        if (board != null) board.OnTileClicked(this);
    }
}
}