using System.Collections;
using UnityEngine;

namespace Crognard
{
    public class CombatManager : MonoBehaviour
    {
        public CombatState _currentState;
        public Faction _initiater; // Who started the fight

        public GameObject _whitePrefab, _blackPrefab;

        public Transform _whiteSpawnPoint, _blackSpawnPoint;
        public CombatUI _whiteCombatUI, _blackCombatUI;

        private Unit _whiteUnit, _blackUnit;

        private int _actionsChosen = 0;

        private CombatResults _results;
        private UIDisplay _uiDisplay;
        private Actions _actions;
        public Act[] _acts = new Act[2];



        private void Start()
        {
            _results = GetComponent<CombatResults>();
            _uiDisplay = GetComponent<UIDisplay>();
            _actions = GetComponent<Actions>();
            
            
            Setup();
        }

        #region Start
        public void Setup()
        {
            _currentState = CombatState.Start;

            if (_whiteUnit == null)
            {
                // GameObject whiteGO = Instantiate(GameSetter.whiteCombatant, _whiteSpawnPoint.position, Quaternion.identity);
                GameObject whiteGO = Instantiate(_whitePrefab, _whiteSpawnPoint.position, Quaternion.identity);
                _whiteUnit = whiteGO.GetComponent<Unit>();
            }

            if (_blackUnit == null)
            {
                // GameObject blackGO = Instantiate(GameSetter.blackCombatant, _blackSpawnPoint.position, Quaternion.identity);
                GameObject blackGO = Instantiate(_blackPrefab, _blackSpawnPoint.position, Quaternion.identity);
                _blackUnit = blackGO.GetComponent<Unit>();
            }
            

            _whiteUnit.Defending = false; _whiteUnit.Damaged = false;
            _blackUnit.Defending = false; _blackUnit.Damaged = false;

            Debug.Log("White HP: " + _whiteUnit.CurrentHP);
            Debug.Log("Black HP: " + _blackUnit.CurrentHP);
            _whiteCombatUI.SetupInfo(_whiteUnit);
            _blackCombatUI.SetupInfo(_blackUnit);

            Initiative();

            OpenCommands();
        }

        private void Initiative()
        {
            if (GameSetter.attacker == Faction.White)
            {
                _currentState = CombatState.ChooseW;  // Testing Damaging
            }
            else if (GameSetter.attacker == Faction.Black)
            {
                _currentState = CombatState.ChooseB;
            }
        }
        #endregion

        #region ChoiseState
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

        /*public void SelectionNavigate(Action action)
        {
            if (action = Action.Item)
            {
                _uiDisplay.ActivateOptions
            }
        }*/

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
        #endregion

        #region Battle
        private IEnumerator Round()
        {

            yield return new WaitForSeconds(1);
            Turn(_acts[0]);

            yield return new WaitForSeconds(1);

            if (!DidSomeoneDie())
            {
                Turn(_acts[1]);
                yield return new WaitForSeconds(1);
            }

            _currentState = CombatState.End;
            Results();

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
                case Action.Light:
                    _actions.Attack(Action.Light, attacker, defender, defenderUI);
                    break;

                case Action.Medium:
                    _actions.Attack(Action.Medium, attacker, defender, defenderUI);
                    break;

                case Action.Heavy:
                    _actions.Attack(Action.Heavy, attacker, defender, defenderUI);
                    break;

                case Action.Defend:
                    _actions.Defend(attacker);
                    break;

                case Action.Counter:
                    _actions.Counter(attacker, defender, defenderUI);
                    break;

                case Action.Item1:
                    break;

                case Action.Item2:
                    break;

                case Action.Item3:
                    break;
                    
            }
        }

        private bool DidSomeoneDie()
        {
            if (_whiteUnit.CurrentHP <= 0 || _blackUnit.CurrentHP <= 0)
            { return true; }
            return false;
        }
        #endregion

        private void Results()
        {
            if (_whiteUnit.CurrentHP <= 0 && _blackUnit.CurrentHP > 0)
            {
                if (GameSetter.attacker == Faction.White) { _results.DefenderWins(); }
                else if (GameSetter.attacker == Faction.Black) { _results.AttackerWins(); }
            }
            else if (_whiteUnit.CurrentHP > 0 && _blackUnit.CurrentHP <= 0)
            {
                if (GameSetter.attacker == Faction.White) { _results.AttackerWins(); }
                else if (GameSetter.attacker == Faction.Black) { _results.DefenderWins(); }
            }
            else if (_whiteUnit.CurrentHP <= 0 && _blackUnit.CurrentHP <= 0)
            {
                _results.DoubleKill();
            }
            else
            {
                _results.NeutralEnd();
            }
            // Proceed to the board with following information:
            // Result: Attacker wins, Defender wins, Double kill, Neutral
            // Attacker wins: Attacker conquers the defender's space
            // Defender wins: Defender stays in their space
            // Double kill: Both paricipants are captured.
            // Neutral: Attacker occupies adjacent spacefrom the direction it attacked.
        }

        #region Endings
        
        #endregion

        public void StartOver()
        {
            _results.Restart();
            Setup();
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
