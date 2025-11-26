using UnityEngine;
using TMPro;

public class TurnIndicator : MonoBehaviour
{
    public Crognard.BoardManager boardManager;
    public TMP_Text text;

    void Start()
    {
        UpdateIndicator();
    }

    void Update()
    {
        UpdateIndicator();
    }

    void UpdateIndicator()
    {
        if (boardManager == null || text == null) return;

        var turn = boardManager.GetCurrentTurn();
        text.text = (turn == Crognard.PieceTeam.White)
            ? "White's Turn"
            : "Black's Turn";
    }
}
