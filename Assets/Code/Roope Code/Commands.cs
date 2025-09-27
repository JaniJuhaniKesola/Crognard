using UnityEngine;

public enum Action { Attack, Defend, Item1, Item2, Item3 }
public class Commands : MonoBehaviour
{
    private CombatManager _manager;

    private void Start()
    {
        _manager = GetComponent<CombatManager>();
    }

    public void OnAttackButton()
    {
        // Deal damage to enemy
        _manager.Turn(Action.Attack);
    }

    public void OnDefendButton()
    {

    }

    public void OnItemButton()
    {
        // Open item selection
    }

    // Add buttons for Items.
}
