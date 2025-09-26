using UnityEngine;

public enum CombatState { Start, Choose, Attacker, Defender, Defeat, End }

public class CombatManager : MonoBehaviour
{


    public GameObject _whitePrefab, _blackPrefab;

    public Transform _whiteSpawnPoint, _blackSpawnPoint;
    public CombatUI _whiteCombatUI, _blackCombatUI;
    private Unit _whiteUnit, _blackUnit;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        GameObject whiteGO = Instantiate(_whitePrefab, _whiteSpawnPoint.position, Quaternion.identity);
        _whiteUnit = whiteGO.GetComponent<Unit>();

        GameObject blackGO = Instantiate(_blackPrefab, _blackSpawnPoint.position, Quaternion.identity);
        _blackUnit = blackGO.GetComponent<Unit>();

        _whiteCombatUI.SetupInfo(_whiteUnit);
        _blackCombatUI.SetupInfo(_blackUnit);
    }

}
