using UnityEngine;

namespace Crognard
{
    public enum GameState { Board, Combat }
    public enum CombatState { Start, ChooseW, ChooseB, Commence, White, Black, End }
    public enum Faction { White, Black }
    public enum ActionType { Light, Medium, Heavy, Defend, Counter, Item1, Item2, Item3 }

    public enum Character { Pawn, Rook, Knight, Bishop, Queen, King }
}
