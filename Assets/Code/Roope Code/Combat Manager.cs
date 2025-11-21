using System;
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

        private PieceSaveData _whiteData, _blackData;

        private CombatResults _results;
        private UIDisplay _uiDisplay;
        private Actions _actions;
        private Announcement _announcement;

        public Act[] _acts = new Act[2];
        [HideInInspector]
        public bool _escaped;



        private void Start()
        {
            _results = GetComponent<CombatResults>();
            _uiDisplay = GetComponent<UIDisplay>();
            _actions = GetComponent<Actions>();
            _announcement = GetComponent<Announcement>();

            _escaped = false;   // make sure escape is not happening automatically.

            Setup();
        }

        #region Start
        public void Setup()
        {
            _currentState = CombatState.Start;

            if (GameStateManager.Instance != null)
            {
                if (BattleData.AttackerTeam == PieceTeam.White)
                {
                    _whiteData = FindData(BattleData.AttackerType);
                    _blackData = FindData(BattleData.DefenderType);
                }
                else if (BattleData.AttackerTeam == PieceTeam.Black)
                {
                    _blackData = FindData(BattleData.AttackerType);
                    _whiteData = FindData(BattleData.DefenderType);
                }
            }

            if (_whiteUnit == null)
            {
                GameObject whiteGO = null;

                if (GameStateManager.Instance != null)
                {
                    if (BattleData.AttackerTeam == PieceTeam.White)
                    {
                        whiteGO = InitializeCombatant(BattleData.AttackerType, _whiteSpawnPoint);
                    }
                    else if (BattleData.AttackerTeam == PieceTeam.Black)
                    {
                        whiteGO = InitializeCombatant(BattleData.DefenderType, _whiteSpawnPoint);
                    }   
                }

                if (whiteGO == null)
                {
                    whiteGO = Instantiate(_whitePrefab, _whiteSpawnPoint.position, Quaternion.identity);
                }
                _whiteUnit = whiteGO.GetComponent<Unit>();

                SetUnitData(_whiteUnit, _whiteData);
            }

            if (_blackUnit == null)
            {
                GameObject blackGO = null;

                if (GameStateManager.Instance != null)
                {
                    if (BattleData.AttackerTeam == PieceTeam.Black)
                    {
                        blackGO = InitializeCombatant(BattleData.AttackerType, _blackSpawnPoint);
                    }
                    else if (BattleData.AttackerTeam == PieceTeam.White)
                    {
                        blackGO = InitializeCombatant(BattleData.DefenderType, _blackSpawnPoint);
                    }
                }

                if (blackGO == null)
                {
                    blackGO = Instantiate(_blackPrefab, _blackSpawnPoint.position, Quaternion.identity);
                }
                _blackUnit = blackGO.GetComponent<Unit>();

                SetUnitData(_blackUnit, _blackData);
            }
            
            _whiteUnit.Defending = false; _whiteUnit.Damaged = false; // _whiteUnit.Restrained = false;

            _blackUnit.Defending = false; _blackUnit.Damaged = false; // _blackUnit.Restrained = false;

            _whiteCombatUI.SetupInfo(_whiteUnit);
            _blackCombatUI.SetupInfo(_blackUnit);

            SetButtons(_whiteUnit);
            SetButtons(_blackUnit);

            if (BattleData.AttackerTeam == PieceTeam.White)
            { _announcement.Challenge(_whiteUnit.name, _blackUnit.name); }
            else if (BattleData.AttackerTeam == PieceTeam.Black)
            { _announcement.Challenge(_blackUnit.name, _whiteUnit.name); }
            else
            { _announcement.Challenge(_whiteUnit.name, _blackUnit.name); }

            StartCoroutine(BattleStarts());
        }

        private IEnumerator BattleStarts()
        {
            yield return new WaitForSeconds(1);
            Initiative();
            OpenCommands();
        }

        private PieceSaveData FindData(string name)
        {
            for (int i = 0; i < GameStateManager.Instance.savedPieces.Count; i++)
            {
                if (GameStateManager.Instance.savedPieces[i].prefabName == name)
                {
                    return GameStateManager.Instance.savedPieces[i];
                }
            }
            Debug.Log("Search Result Null");
            return null;
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

        private void SetUnitData(Unit unit, PieceSaveData piece)
        {
            if (GameStateManager.Instance != null)
            {
                Debug.Log(piece.hp);
                unit.Name = piece.prefabName; unit.CurrentHP = piece.hp; unit.Stamina = piece.stamina;
            }
        }

        private void SetButtons(Unit unit)
        {
            // Check the cost of an action and unit's stamina
            // If cost is higher than stamina, disable that button.
            for (int i = 0; i < Enum.GetValues(typeof(ActionType)).Length; i++)
            {
                if (_actions.GetCost((ActionType)i) > unit.Stamina)
                {
                    _uiDisplay.TurnOnOffButtons((ActionType)i, false, unit.Faction);
                }
                else
                {
                    _uiDisplay.TurnOnOffButtons((ActionType)i, true, unit.Faction);
                }
            }
        }

        private void Initiative()
        {
            if (BattleData.AttackerTeam == PieceTeam.White)
            {
                _currentState = CombatState.ChooseW;  // Testing Damaging
            }
            else if (BattleData.AttackerTeam == PieceTeam.Black)
            {
                _currentState = CombatState.ChooseB;
            }
        }
        #endregion

        #region ChoiseState
        public void OpenCommands()
        {
            _announcement.Select();
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
                _announcement.Stamina();
            }
            else if (_currentState == CombatState.ChooseB)
            {
                if (_actions.GetCost(action) > _blackUnit.Stamina) { return; }
                _announcement.Stamina();
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
            // Announce which action is gonna happen.
            AnnounceAction(_acts[0]);
            yield return new WaitForSeconds(1);
            Turn(_acts[0]);

            yield return new WaitForSeconds(1);

            if (!EarlyFinish())
            {
                Turn(_acts[1]);
                yield return new WaitForSeconds(1);
            }
            
            if (_escaped)
            {
                if (_acts[0].faction == Faction.White)
                {
                    _announcement.Escape(_whiteUnit.Name);
                }
                else
                {
                    _announcement.Escape(_blackUnit.Name);
                }
                
            }
            yield return new WaitForSeconds(1);

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
                _results.Winner(_blackUnit, _whiteUnit);
                
            }
            else if (_whiteUnit.CurrentHP > 0 && _blackUnit.CurrentHP <= 0)
            {
                _results.Winner(_whiteUnit, _blackUnit);
                
            }
            else if (_whiteUnit.CurrentHP <= 0 && _blackUnit.CurrentHP <= 0)
            {
                _results.DoubleKill(_whiteUnit, _blackUnit);
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

        private void AnnounceAction(Act act)
        {
            string user, target;
            if (act.faction == Faction.White) { user = _whiteUnit.Name; target = _blackUnit.Name; }
            else { user = _blackUnit.Name; target = _whiteUnit.Name; }

            switch (act.action)
            {
                case ActionType.Light:
                    _announcement.Attacks(user, target);
                    break;

                case ActionType.Medium:
                    _announcement.Attacks(user, target);
                    break;

                case ActionType.Heavy:
                    _announcement.Attacks(user, target);
                    break;

                case ActionType.Defend:
                    _announcement.Defending(user);
                    break;

                case ActionType.Counter:
                    _announcement.Counter(user);
                    break;

                case ActionType.Item1:
                    _announcement.Healed(user);
                    break;

                case ActionType.Item2:
                    _announcement.Sticky(user, target);
                    break;

                case ActionType.Item3:
                    _announcement.Smoke(user);
                    break;
            }
        }
    }

    
}
