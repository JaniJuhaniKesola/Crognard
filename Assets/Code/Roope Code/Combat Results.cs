using System.Collections;
using TMPro;
using UnityEngine;

namespace Crognard
{
    public class CombatResults : MonoBehaviour
    {
        [SerializeField] private VictoryConfetti _whiteConfetti, _blackConfetti;
        [SerializeField] private GameObject _resultScreen, _neutralScreen, _doubleKillScreen;
        [SerializeField] private TextMeshProUGUI _winnerText;

        public void AttackerWins(Unit winner)
        {
            // Play Confetti
            switch (GameSetter.attacker)
            {
                case Faction.White:
                    _whiteConfetti.StartConfetti();
                    GameSetter.whiteCombatant.HP = winner.CurrentHP;
                    GameSetter.whiteCombatant.Stamina = winner.Stamina;
                    GameSetter.whiteCombatant.Position = GameSetter.blackCombatant.Position;
                    GameSetter.blackCombatant.Alive = false;
                    break;

                case Faction.Black:
                    _blackConfetti.StartConfetti();
                    GameSetter.blackCombatant.HP = winner.CurrentHP;
                    GameSetter.blackCombatant.Stamina = winner.Stamina;
                    GameSetter.blackCombatant.Position = GameSetter.whiteCombatant.Position;
                    GameSetter.blackCombatant.Alive = false;
                    break;
            }
            UpdatePieces();
        }

        public void DefenderWins(Unit winner)
        {
            // Play Confetti
            switch (GameSetter.attacker)
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
            UpdatePieces();
        }

        public void Winner(Unit winner)
        {
            if (GameSetter.attacker == winner.Faction)
            {
                AttackerWins(winner);
                // Attacker takes loser's place on the board.
            }
            else
            {
                DefenderWins(winner);
            }
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

            if (GameSetter.whiteCombatant != null)
            {
                GameSetter.whiteCombatant.HP = white.CurrentHP;
                GameSetter.whiteCombatant.Stamina = white.Stamina;
            }

            if (GameSetter.blackCombatant != null)
            {
                GameSetter.blackCombatant.HP = black.CurrentHP;
                GameSetter.blackCombatant.Stamina = black.Stamina;
            }

            UpdatePieces();
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
    }
}
