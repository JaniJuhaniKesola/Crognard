using UnityEngine;

namespace Crognard
{
    public class CombatResults : MonoBehaviour
    {
        [SerializeField] private VictoryConfetti _whiteConfetti, _blackConfetti;
        [SerializeField] private GameObject _neutralScreen, _doubleKillScreen;

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
