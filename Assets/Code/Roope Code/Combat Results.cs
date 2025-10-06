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

        public void AttackerWins()
        {
            // Play Confetti
            switch (GameSetter.attacker)
            {
                case Faction.White:
                    _whiteConfetti.StartConfetti();
                    break;

                case Faction.Black:
                    _blackConfetti.StartConfetti();
                    break;
            }
        }

        public void DefenderWins()
        {
            // Play Confetti
            switch (GameSetter.attacker)
            {
                case Faction.White:
                    _blackConfetti.StartConfetti();
                    break;

                case Faction.Black:
                    _whiteConfetti.StartConfetti();
                    break;
            }
        }

        public void Winner(Unit winner, Unit loser)
        {
            if (GameSetter.attacker == winner.Faction)
            {
                AttackerWins();
                // Attacker takes loser's place on the board.
            }
            else
            {
                DefenderWins();
            }
            StartCoroutine(VictoryCycle(winner, loser));
        }

        private IEnumerator VictoryCycle(Unit winner, Unit loser)
        {
            yield return new WaitForSeconds(1);
            Results(winner);
            yield return new WaitForSeconds(1);
            // Load Scene to the board
            /*
            GameSetter.defeated = loser
            */
        }

        public void Results(Unit unit)
        {
            _resultScreen.SetActive(true);
            _winnerText.text = unit.Name;
        }

        public void NeutralEnd()
        {
            // Exit Combat
            _neutralScreen.SetActive(true);
        }

        public void DoubleKill()
        {
            // Set Active Asset for Double Kill
            _doubleKillScreen.SetActive(true);
        }

        public void Restart()
        {
            _neutralScreen.SetActive(false);
            _doubleKillScreen.SetActive(false);
            _whiteConfetti.StopConfetti();
            _blackConfetti.StopConfetti();
        }
    }
}
