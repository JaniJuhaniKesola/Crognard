using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Crognard
{
    public enum PieceTeam { White, Black }

    [RequireComponent(typeof(SpriteRenderer))]
    public class Piece : MonoBehaviour
    {
        [HideInInspector] public Vector2Int gridPosition;

        protected BoardManager board;
        public string Name;     // Will be added at the start of the game.
        public BoardManager Board => board;
        private Coroutine moveCoroutine;
        private SpriteRenderer sr;
        private bool selected;

        [Tooltip("Seconds to move between tiles")]
        public float moveDuration = 0.12f;

        [Header("Team Settings")]
        public PieceTeam team = PieceTeam.White;

        [Header("Prefab Variants (Set These On Prefabs)")]
        public GameObject whiteVersion;
        public GameObject blackVersion;

        public int hp, stamina;
        public GameObject combatPrefab;

        public virtual void Initialize(Vector2Int startGrid, BoardManager manager)
        {
            board = manager;
            gridPosition = startGrid;

            sr = GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = gameObject.AddComponent<SpriteRenderer>();

            sr.sortingLayerName = "Pieces";
            sr.sortingOrder = 1;

            transform.position = board.GridToWorld(startGrid);
            transform.localScale = Vector3.one * (board.tileSize * board.pieceScale);
        }

        /// <summary>
        /// Returns the correct prefab for this piece's team.
        /// Used for pawn promotion.
        /// </summary>
        public GameObject GetTeamPrefab()
        {
            return team == PieceTeam.White ? whiteVersion : blackVersion;
        }

        public void SetSelected(bool sel)
        {
            selected = sel;
            float baseScale = board.tileSize * board.pieceScale;
            transform.localScale = sel
                ? Vector3.one * baseScale * 1.12f
                : Vector3.one * baseScale;
        }

        public virtual void MoveTo(Vector2Int newGrid)
        {
            UpdateGridPosition(newGrid);
        }

        public void UpdateGridPosition(Vector2Int newGrid)
        {
            gridPosition = newGrid;

            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(SmoothMove(board.GridToWorld(newGrid)));
        }

        private IEnumerator SmoothMove(Vector3 targetWorld)
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

        public virtual List<Vector2Int> GetValidMoves() => new List<Vector2Int>();
    }
}
