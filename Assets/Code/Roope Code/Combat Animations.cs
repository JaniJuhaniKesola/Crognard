using UnityEngine;

namespace Crognard
{
    public class CombatAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _whiteAttacks, _whiteEffects;
        [SerializeField] private Animator _blackAttacks, _blackEffects;

        public ActionType _whiteAction, _blackAction;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _whiteAction = ActionType.Null;
            _blackAction = ActionType.Null;
        }

        public void SetAction(ActionType action, Faction faction)
        {
            if (faction == Faction.White)
            {
                UpdateAnimation(_whiteAttacks, _whiteEffects, (int)action, _blackEffects);
            }
            else if (faction == Faction.Black)
            {
                UpdateAnimation(_blackAttacks, _blackEffects, (int)action, _whiteEffects);
            }
            
        }

        private void UpdateAnimation(Animator attacks, Animator effects, int actionId, Animator enemyEffects)
        {
            if ((ActionType)actionId != ActionType.Item2)
            {
                attacks.SetInteger("Attack", actionId);

                effects.SetInteger("Effect", actionId);
            }
            else
            {
                enemyEffects.SetInteger("Effect", actionId);
            }
            
        }
    }
}
