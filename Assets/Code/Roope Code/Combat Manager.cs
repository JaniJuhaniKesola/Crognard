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
        [HideInInspector]
        public bool _escaped;



        private void Start()
        {
            _results = GetComponent<CombatResults>();
            _uiDisplay = GetComponent<UIDisplay>();
            _actions = GetComponent<Actions>();

            _escaped = false;   // make sure escape is not happening automatically.

            Setup();
        }

        #region Start
        public void Setup()
        {
            _currentState = CombatState.Start;

            if (_whiteUnit == null)
            {
                GameObject whiteGO;

                if (BattleData.AttackerTeam == PieceTeam.White)
                {
                    whiteGO = InitializeCombatant(BattleData.AttackerType, _whiteSpawnPoint);
                }
                else
                {
                    whiteGO = InitializeCombatant(BattleData.DefenderType, _whiteSpawnPoint);
                }

                // GameObject prefab = Resources.Load<GameObject>(data.prefabName);
                //GameObject whiteGO = Instantiate(GameSetter.whiteCombatant.CombatPrefab, _whiteSpawnPoint.position, Quaternion.identity);
                if (whiteGO == null)
                {
                    whiteGO = Instantiate(_whitePrefab, _whiteSpawnPoint.position, Quaternion.identity);
                }
                _whiteUnit = whiteGO.GetComponent<Unit>();

                //SetUnitData(_whiteUnit, GameSetter.whiteCombatant);
            }

            if (_blackUnit == null)
            {
                GameObject blackGO;

                if (BattleData.AttackerTeam == PieceTeam.Black)
                {
                    blackGO = InitializeCombatant(BattleData.AttackerType, _blackSpawnPoint);
                }
                else
                {
                    blackGO = InitializeCombatant(BattleData.DefenderType, _blackSpawnPoint);
                }
                // GameObject blackGO = Instantiate(GameSetter.blackCombatant, _blackSpawnPoint.position, Quaternion.identity);
                if (blackGO == null)
                {
                    blackGO = Instantiate(_blackPrefab, _blackSpawnPoint.position, Quaternion.identity);
                }
                _blackUnit = blackGO.GetComponent<Unit>();

                //SetUnitData(_whiteUnit, GameSetter.whiteCombatant);
            }
            
            _whiteUnit.Defending = false; _whiteUnit.Damaged = false; _whiteUnit.Restrained = false;

            _blackUnit.Defending = false; _blackUnit.Damaged = false; _blackUnit.Restrained = false;

            //Debug.Log("White HP: " + _whiteUnit.CurrentHP);
            //Debug.Log("Black HP: " + _blackUnit.CurrentHP);
            _whiteCombatUI.SetupInfo(_whiteUnit);
            _blackCombatUI.SetupInfo(_blackUnit);

            Initiative();

            OpenCommands();
        }

        private GameObject InitializeCombatant(string name, Transform spawnPoint)
        {
            Debug.Log(GameStateManager.Instance.savedPieces.Count);
            for (int i = 0; i < GameStateManager.Instance.savedPieces.Count; i++)
            {
                if (GameStateManager.Instance.savedPieces[i].prefabName == name)
                {
                    Debug.Log("Found Right Item");
                    Debug.Log(GameStateManager.Instance.savedPieces[i].combatPrefab);
                    if (GameStateManager.Instance.savedPieces[i].combatPrefab != null)
                    {
                        return Instantiate(GameStateManager.Instance.savedPieces[i].combatPrefab, spawnPoint.position, Quaternion.identity);
                    }
                }
            }
            Debug.Log("Combatant is Null");
            return null;
        }

        private void SetUnitData(Unit unit, ChessPiece piece)
        {
            unit.Name = piece.Name; unit.CurrentHP = piece.HP; unit.Stamina = piece.Stamina;
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

        public void ActionChosen(ActionType action)
        {
            if (_currentState == CombatState.ChooseW)
            {
                if (_actions.GetCost(action) > _whiteUnit.Stamina) { return; }
            }
            else if (_currentState == CombatState.ChooseB)
            {
                if (_actions.GetCost(action) > _blackUnit.Stamina) { return; }
            }

            int priority = _actions.GetPriority(action);
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

        private void SaveAction(int index, ActionType action, int priority)
        {
            Act act = null;

            if (_currentState == CombatState.ChooseW)
            { act = new Act(Faction.White, action, priority); }
            else if (_currentState == CombatState.ChooseB)
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

            if (!EarlyFinish())
            {
                Turn(_acts[1]);
                yield return new WaitForSeconds(1);
            }

            Debug.Log("White HP: " + _whiteUnit.CurrentHP + ", Black HP: " + _blackUnit.CurrentHP);
            _currentState = CombatState.End;
            Results();

        }

        public void Turn(Act act)
        {
            if (act.faction == Faction.White) { _currentState = CombatState.White; }
            else if (act.faction == Faction.Black) { _currentState = CombatState.Black; }

            Unit attacker, defender;
            CombatUI attackerUI, defenderUI;
            if (_currentState == CombatState.White)
            {
                attacker = _whiteUnit;
                defender = _blackUnit;
                attackerUI = _whiteCombatUI;
                defenderUI = _blackCombatUI;
            }
            else if (_currentState == CombatState.Black)
            {
                attacker = _blackUnit;
                defender = _whiteUnit;
                attackerUI = _blackCombatUI;
                defenderUI = _whiteCombatUI;
            }
            else { return; }

            /*switch (act.action)
            {
                case ActionType.Light:
                    _actions.Attack(ActionType.Light, attacker, defender, attackerUI, defenderUI);
                    break;

                case ActionType.Medium:
                    _actions.Attack(ActionType.Medium, attacker, defender, attackerUI, defenderUI);
                    break;

                case ActionType.Heavy:
                    _actions.Attack(ActionType.Heavy, attacker, defender, attackerUI, defenderUI);
                    break;

                case ActionType.Defend:
                    _actions.Defend(attacker);
                    break;

                case ActionType.Counter:
                    _actions.Counter(attacker, defender, defenderUI);
                    break;

                case ActionType.Item1:
                    _actions.Recover(attacker, attackerUI);
                    break;

                case ActionType.Item2:
                    _actions.Restrain(defender);
                    break;

                case ActionType.Item3:
                    _actions.Escape();
                    _escaped = true;
                    break;

            }*/
            
            if (_actions.GetCost(act.action) > attacker.Stamina) { return; }
            
            _actions.GetAction(act.action, attacker, defender, attackerUI, defenderUI);
        }

        private bool EarlyFinish()
        {
            if (_whiteUnit.CurrentHP <= 0 || _blackUnit.CurrentHP <= 0)
            { return true; }
            if (_escaped)
            { return true; }
            return false;
        }
        #endregion

        private void Results()
        {
            if (_whiteUnit.CurrentHP <= 0 && _blackUnit.CurrentHP > 0)
            {
                _results.Winner(_blackUnit);
                
            }
            else if (_whiteUnit.CurrentHP > 0 && _blackUnit.CurrentHP <= 0)
            {
                _results.Winner(_whiteUnit);
                
            }
            else if (_whiteUnit.CurrentHP <= 0 && _blackUnit.CurrentHP <= 0)
            {
                _results.DoubleKill();
            }
            else
            {
                _results.NeutralEnd(_whiteUnit, _blackUnit);
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
        
        private void TryDictionary()
        {
            ChessPiece guy = null;
            GameSetter.boardOccupiers.Add(new Vector2Int(0, 0), guy);
            GameSetter.boardOccupiers.Remove(new Vector2Int(0, 0));
        }
    }

    
}
