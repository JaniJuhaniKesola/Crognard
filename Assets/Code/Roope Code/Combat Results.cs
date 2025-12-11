using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crognard
{
    public class CombatResults : MonoBehaviour
    {
        [SerializeField] private ParticleEffect _whiteConfetti, _blackConfetti;
        [SerializeField] private GameObject _neutralScreen, _doubleKillScreen;

        private Announcement _announcement;

        private void Start()
        {
            _announcement = GetComponent<Announcement>();
        }

        public void AttackerWins(Unit winner, Unit loser)
        {
            HandleUnit(winner);
            OnVictory(winner);
            Loser(loser);
            // Play Confetti
            _announcement.Attacker(winner.Name, loser.Name);

            switch (BattleData.AttackerTeam)
            {
                case PieceTeam.White:
                    _whiteConfetti.StartConfetti();
                    break;
                case PieceTeam.Black:
                    _blackConfetti.StartConfetti();
                    break;
            }
        }

        public void DefenderWins(Unit winner, Unit loser)
        {
            HandleUnit(winner);
            // Play Confetti
            Loser(loser);
            _announcement.Defender(winner.Name, loser.Name);

            switch (BattleData.DefenderTeam)
            {
                case PieceTeam.White:
                    _whiteConfetti.StartConfetti();
                    break;
                case PieceTeam.Black:
                    _blackConfetti.StartConfetti();
                    break;
            }
        }

        public void Winner(Unit winner, Unit loser)
        {
            if ((int)BattleData.AttackerTeam == (int)winner.Faction)
            {
                AttackerWins(winner, loser);
            }
            else
            {
                DefenderWins(winner, loser);
            }
            //
            StartCoroutine(VictoryCycle(winner));
        }

        public void Loser(Unit loser)
        {
            HandleUnit(loser);
        }

        private IEnumerator VictoryCycle(Unit winner)
        {
            yield return new WaitForSeconds(2.5f);

            SceneManager.LoadScene("JaniTest");
        }

        public void NeutralEnd(Unit white, Unit black)
        {
            // Exit Combat
            //_neutralScreen.SetActive(true);
            
            _announcement.Neutral();

            HandleUnit(white);
            HandleUnit(black);

            StartCoroutine(VictoryCycle(null));
        }

        public void DoubleKill(Unit white, Unit black)
        {
            // Set Active Asset for Double Kill
            _doubleKillScreen.SetActive(true);
            HandleUnit(white);
            HandleUnit(black);
        }

        public void Restart()
        {
            _neutralScreen.SetActive(false);
            _doubleKillScreen.SetActive(false);
            _whiteConfetti.StopConfetti();
            _blackConfetti.StopConfetti();
        }

        private void HandleUnit(Unit unit)
        {
            for (int i = 0; i < GameStateManager.Instance.savedPieces.Count; i++)
            {
                if (GameStateManager.Instance.savedPieces[i].prefabName == unit.Name)
                {
                    if (unit.CurrentHP <= 0)
                    {
                        GameStateManager.Instance.savedPieces[i] = null;
                        break;
                    }
                    GameStateManager.Instance.savedPieces[i].hp = unit.CurrentHP;
                    GameStateManager.Instance.savedPieces[i].stamina = unit.Stamina;
                    // GameStateManager.Instance.savedPieces[i].restrained = unit.restrained;
                    break;
                }
            }
        }

        private void OnVictory(Unit winner)
        {
            for (int i = 0; i < GameStateManager.Instance.savedPieces.Count; i++)
            {
                if (GameStateManager.Instance.savedPieces[i].prefabName == winner.Name)
                {
                    GameStateManager.Instance.savedPieces[i].position = BattleData.DefenderPos;
                    break;
                }
            }
        }
    }
}
