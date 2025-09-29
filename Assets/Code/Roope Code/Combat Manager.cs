using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Crognard
{
    public enum CombatState { Start, ChooseW, ChooseB, Commence, White, Black, End }
    public enum Faction { White, Black }

    public class CombatManager : MonoBehaviour
    {
        public CombatState _currentState;
        public Faction _initiater; // Who started the fight

        public GameObject _whitePrefab, _blackPrefab;

        public Transform _whiteSpawnPoint, _blackSpawnPoint;
        public CombatUI _whiteCombatUI, _blackCombatUI;

        private Unit _whiteUnit, _blackUnit;

        private int _actionsChosen = 0;

        private UIDisplay _uiDisplay;
        private Actions _actions;
        public Act[] _acts = new Act[2];



        private void Start()
        {
            _uiDisplay = GetComponent<UIDisplay>();
            _actions = GetComponent<Actions>();
            _initiater = GameSetter.attacker;

            _currentState = CombatState.Start;
            Setup();
        }

        public void Setup()
        {
            // GameObject whiteGO = Instantiate(GameSetter.whiteCombatant, _whiteSpawnPoint.position, Quaternion.identity);
            GameObject whiteGO = Instantiate(_whitePrefab, _whiteSpawnPoint.position, Quaternion.identity);
            _whiteUnit = whiteGO.GetComponent<Unit>();

            // GameObject blackGO = Instantiate(GameSetter.blackCombatant, _blackSpawnPoint.position, Quaternion.identity);
            GameObject blackGO = Instantiate(_blackPrefab, _blackSpawnPoint.position, Quaternion.identity);
            _blackUnit = blackGO.GetComponent<Unit>();

            Debug.Log("White HP: " + _whiteUnit.CurrentHP);
            Debug.Log("Black HP: " + _blackUnit.CurrentHP);
            _whiteCombatUI.SetupInfo(_whiteUnit);
            _blackCombatUI.SetupInfo(_blackUnit);

            Initiative();

            OpenCommands();
        }

        private void Initiative()
        {
            if (_initiater == Faction.White)
            {
                _currentState = CombatState.ChooseW;  // Testing Damaging
            }
            else if (_initiater == Faction.Black)
            {
                _currentState = CombatState.ChooseB;
            }
        }

        public void OpenCommands()
        {
            if (_currentState == CombatState.ChooseW)
            {
                _uiDisplay.WhiteCommands(true);
                _uiDisplay.BlackCommands(false);
            }
            else if (_currentState == CombatState.ChooseB)
            {
                _uiDisplay.WhiteCommands(false);
                _uiDisplay.BlackCommands(true);
            }
            else
            {
                _uiDisplay.WhiteCommands(false);
                _uiDisplay.BlackCommands(false);
            }
        }

        public void ActionChosen(Action action, int priority)
        {
            SaveAction(_actionsChosen, action, priority);

            if (_actionsChosen >= 1)
            {
                StartCoroutine(Round());
                _currentState = CombatState.Commence;
                _actionsChosen = 0;
            }
            else if (_actionsChosen == 0)
            {
                if (_currentState == CombatState.ChooseW)
                {
                    _currentState = CombatState.ChooseB;
                }
                else if (_currentState == CombatState.ChooseB)
                {
                    _currentState = CombatState.ChooseW;
                }
                _actionsChosen++;
            }
            OpenCommands();

        }

        private void SaveAction(int index, Action action, int priority)
        {
            Act act = null;

            if (_currentState == CombatState.ChooseW)
            { act = new Act(Faction.White, action, priority); }
            else
            { act = new Act(Faction.Black, action, priority); }

            _acts[index] = act;

            if (index >= 1) { PriorityCheck(); }
        }

        private void PriorityCheck()
        {
            if (_acts[0].priority < _acts[1].priority)
            {
                Act act = _acts[0];
                _acts[0] = _acts[1];
                _acts[1] = act;
            }
        }

        private IEnumerator Round()
        {

            yield return new WaitForSeconds(1);
            Turn(_acts[0]);

            yield return new WaitForSeconds(1);
            Turn(_acts[1]);

            yield return new WaitForSeconds(1);

            _currentState = CombatState.End;
            StartOver();
        }

        public void Turn(Act act)
        {
            if (act.faction == Faction.White) { _currentState = CombatState.White; }
            else if (act.faction == Faction.Black) { _currentState = CombatState.Black; }

            Unit attacker, defender;
            CombatUI defenderUI;
            if (_currentState == CombatState.White) { attacker = _whiteUnit; defender = _blackUnit; defenderUI = _blackCombatUI; }
            else if (_currentState == CombatState.Black) { attacker = _blackUnit; defender = _whiteUnit; defenderUI = _whiteCombatUI; }
            else { return; }

            switch (act.action)
            {
                case Action.Attack:
                    _actions.Attack(attacker, defender, defenderUI);
                    break;

                case Action.Defend:
                    break;

                case Action.Item1:
                    break;
            }

            if (defender.CurrentHP <= 0 && attacker.CurrentHP <= 0)
            {
                // DrawKill();
            }
            else if (defender.CurrentHP <= 0 && attacker.CurrentHP > 0)
            {
                // AttackerVictory();
            }
            else if (attacker.CurrentHP <= 0 && defender.CurrentHP > 0)
            {
                // DefenderVictory():
            }

        }

        private void AttackerVictory()
        {
            switch (_currentState)
            {
                case CombatState.White:
                    break;

                case CombatState.Black:
                    break;
            }
            // Defeated unit shall be erased from combat and board.
        }

        private void DefenderVictory()
        {
            switch (_currentState)
            {
                case CombatState.White:
                    break;

                case CombatState.Black:
                    break;
            }
        }

        private void DrawKill()
        {
            // Both Units will be erased from combat and board.
        }

        private void StartOver()
        {
            Initiative();
            OpenCommands();
        }
    }

    public class Act
    {
        public Faction faction;
        public Action action;
        public int priority;

        public Act(Faction faction, Action action, int priority)
        {
            this.faction = faction;
            this.action = action;
            this.priority = priority;
        }
    }
}
