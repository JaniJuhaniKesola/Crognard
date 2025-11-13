using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crognard
{
    public class CombatResults : MonoBehaviour
    {
        [SerializeField] private VictoryConfetti _whiteConfetti, _blackConfetti;
        [SerializeField] private GameObject _resultScreen, _neutralScreen, _doubleKillScreen;
        [SerializeField] private TextMeshProUGUI _winnerText;

        public void AttackerWins(Unit winner)
        {
            HandleUnit(winner);
            // Play Confetti
            switch (BattleData.AttackerTeam)
            {
                case PieceTeam.White:
                    _whiteConfetti.StartConfetti();
                    break;
                case PieceTeam.Black:
                    _blackConfetti.StartConfetti();
                    break;
            }
            /*switch (GameSetter.attacker)
            {
                case Faction.White:
                    _whiteConfetti.StartConfetti();
                    GameSetter.whiteCombatant.HP = winner.CurrentHP;
                    GameSetter.whiteCombatant.Stamina = winner.Stamina;

                    UpdatePositions(GameSetter.whiteCombatant, GameSetter.blackCombatant.Position);

                    GameSetter.blackCombatant.Alive = false;
                    break;

                case Faction.Black:
                    _blackConfetti.StartConfetti();
                    GameSetter.blackCombatant.HP = winner.CurrentHP;
                    GameSetter.blackCombatant.Stamina = winner.Stamina;

                    UpdatePositions(GameSetter.blackCombatant, GameSetter.whiteCombatant.Position);

                    GameSetter.blackCombatant.Alive = false;
                    break;
            }*/
            //UpdatePieces();
        }

        private void UpdatePositions(ChessPiece combatant, Vector2Int newPlace)
        {
            GameSetter.boardOccupiers[combatant.Position] = null;
            combatant.Position = newPlace;
            GameSetter.boardOccupiers[combatant.Position] = combatant;
        }

        public void DefenderWins(Unit winner)
        {
            HandleUnit(winner);
            // Play Confetti
            switch (BattleData.DefenderTeam)
            {
                case PieceTeam.White:
                    _whiteConfetti.StartConfetti();
                    break;
                case PieceTeam.Black:
                    _blackConfetti.StartConfetti();
                    break;
            }

            // Play Confetti
            /*switch (GameSetter.attacker)
            {
                case Faction.White:
                    _blackConfetti.StartConfetti();
                    GameSetter.blackCombatant.HP = winner.CurrentHP;
                    GameSetter.blackCombatant.Stamina = winner.Stamina;
                    GameSetter.whiteCombatant.Alive = false;
                    break;

                case Faction.Black:
                    _whiteConfetti.StartConfetti();
                    GameSetter.whiteCombatant.HP = winner.CurrentHP;
                    GameSetter.whiteCombatant.Stamina = winner.Stamina;
                    GameSetter.blackCombatant.Alive = false;
                    break;
            }
            UpdatePieces();*/
        }

        public void Winner(Unit winner)
        {
            /*if (GameSetter.attacker == winner.Faction)
            { AttackerWins(winner); }   // Attacker takes loser's place on the board.
            else
            { DefenderWins(winner); }*/
            //
            if ((int)BattleData.AttackerTeam == (int)winner.Faction)
            { AttackerWins(winner); }
            else
            { DefenderWins(winner); }
            //
            StartCoroutine(VictoryCycle(winner));
        }

        private IEnumerator VictoryCycle(Unit winner)
        {
            yield return new WaitForSeconds(1);
            Results(winner);
            yield return new WaitForSeconds(1);
            // Load Scene to the board
            /*
            GameSetter.defeated = loser
            */
            SceneManager.LoadScene("JaniTest");
        }

        public void Results(Unit winner)
        {
            _resultScreen.SetActive(true);
            _winnerText.text = winner.Name;
        }

        public void NeutralEnd(Unit white, Unit black)
        {
            // Exit Combat
            _neutralScreen.SetActive(true);

            HandleUnit(white);
            HandleUnit(black);

            /*if (GameSetter.whiteCombatant != null)
            {
                GameSetter.whiteCombatant.HP = white.CurrentHP;
                GameSetter.whiteCombatant.Stamina = white.Stamina;
            }

            if (GameSetter.blackCombatant != null)
            {
                GameSetter.blackCombatant.HP = black.CurrentHP;
                GameSetter.blackCombatant.Stamina = black.Stamina;
            }

            UpdatePieces();*/
        }

        public void DoubleKill()
        {
            // Set Active Asset for Double Kill
            _doubleKillScreen.SetActive(true);
            GameSetter.whiteCombatant.Alive = false;
            GameSetter.blackCombatant.Alive = false;

            UpdatePieces();
        }

        public void Restart()
        {
            _neutralScreen.SetActive(false);
            _doubleKillScreen.SetActive(false);
            _whiteConfetti.StopConfetti();
            _blackConfetti.StopConfetti();
        }

        private void UpdatePieces()
        {
            if (GameSetter.whiteCombatant != null && GameSetter.blackCombatant != null)
            {
                GameSetter.pieces[GameSetter.whiteCombatID] = GameSetter.whiteCombatant;
                GameSetter.pieces[GameSetter.blackCombatID] = GameSetter.blackCombatant;
            }
        }

        private void HandleUnit(Unit unit)
        {
            for (int i = 0; i < GameStateManager.Instance.savedPieces.Count; i++)
            {
                if (GameStateManager.Instance.savedPieces[i].prefabName == unit.Name)
                {
                    GameStateManager.Instance.savedPieces[i].hp = unit.CurrentHP;
                    GameStateManager.Instance.savedPieces[i].stamina = unit.Stamina;
                    break;
                }
            }
        }
    }
}
