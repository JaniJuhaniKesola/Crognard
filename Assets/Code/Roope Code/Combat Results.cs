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
                    _whiteConfetti.StartConfetti();
                    break;

                case Faction.Black:
                    _blackConfetti.StartConfetti();
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
    }
}
