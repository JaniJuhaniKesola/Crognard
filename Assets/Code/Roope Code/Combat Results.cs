using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crognard
{
    public class CombatResults : MonoBehaviour
    {
        [SerializeField] private ParticleEffect _whiteConfetti, _blackConfetti;
        [SerializeField] private GameObject _resultScreen, _neutralScreen, _doubleKillScreen;
        [SerializeField] private TextMeshProUGUI _winnerText;

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

        private void UpdatePositions(ChessPiece combatant, Vector2Int newPlace)
        {
            GameSetter.boardOccupiers[combatant.Position] = null;
            combatant.Position = newPlace;
            GameSetter.boardOccupiers[combatant.Position] = combatant;
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
            yield return new WaitForSeconds(1);
            if (winner != null)
            {
                Results(winner);
            }
            else
            {
                Results(null);
            }
            yield return new WaitForSeconds(1);

            SceneManager.LoadScene("JaniTest");
        }

        public void Results(Unit winner)
        {
            _resultScreen.SetActive(true);
            if (winner != null)
            {
                _winnerText.text = winner.Name;
            }
            else
            {
                _winnerText.text = "Not This Time";
            }
        }

        public void NeutralEnd(Unit white, Unit black)
        {
            // Exit Combat
            _neutralScreen.SetActive(true);

            HandleUnit(white);
            HandleUnit(black);

            StartCoroutine(VictoryCycle(null));

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
