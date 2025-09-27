using UnityEngine;

public enum CombatState { Start, ChooseW, ChooseB, White, Black, End }

public class CombatManager : MonoBehaviour
{
    public CombatState _currentState;
    public Action _action;

    public GameObject _whitePrefab, _blackPrefab;

    public Transform _whiteSpawnPoint, _blackSpawnPoint;
    public CombatUI _whiteCombatUI, _blackCombatUI;

    private Unit _whiteUnit, _blackUnit;

    private void Start()
    {
        _currentState = CombatState.Start;
        Setup();
    }

    public void Setup()
    {
        GameObject whiteGO = Instantiate(_whitePrefab, _whiteSpawnPoint.position, Quaternion.identity);
        _whiteUnit = whiteGO.GetComponent<Unit>();

        GameObject blackGO = Instantiate(_blackPrefab, _blackSpawnPoint.position, Quaternion.identity);
        _blackUnit = blackGO.GetComponent<Unit>();

        Debug.Log("White HP: " + _whiteUnit.CurrentHP);
        Debug.Log("Black HP: " + _blackUnit.CurrentHP);
        _whiteCombatUI.SetupInfo(_whiteUnit);
        _blackCombatUI.SetupInfo(_blackUnit);

        _currentState = CombatState.White;  // Testing Damaging
    }

    public void ChooseAction()
    {

    }

    public void Turn(Action action)
    {
        Unit attacker, defender;
        CombatUI defenderUI;
        if (_currentState == CombatState.White) { attacker = _whiteUnit; defender = _blackUnit; defenderUI = _blackCombatUI; }
        else if (_currentState == CombatState.Black) { attacker = _blackUnit; defender = _whiteUnit; defenderUI = _whiteCombatUI; }
        else { return; }

        switch (action)
        {
            case Action.Attack:
                Attack(attacker, defender, defenderUI);
                break;

            case Action.Defend:
                break;

            case Action.Item1:
                break;
        }

        if (_currentState == CombatState.White) { _currentState = CombatState.Black; }
        else if (_currentState == CombatState.Black) { _currentState = CombatState.White; }
        else { return; }

    }

    public void Attack(Unit attacker, Unit defender, CombatUI hub)
    {
        defender.TakeDamage(attacker.Damage);

        hub.HealthBar(defender.CurrentHP, defender.MaxHP);
    }

}
