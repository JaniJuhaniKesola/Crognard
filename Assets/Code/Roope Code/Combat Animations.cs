using UnityEngine;

namespace Crognard
{
    public class CombatAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _whiteAttacks, _whiteEffects;
        [SerializeField] private Animator _blackAttacks, _blackEffects;

        public ActionType _whiteAction, _blackAction;

        void Start()
        {
            float speed = 0;
            if (Options.combatAnimationsOn) { speed = 1; }
            else { speed = 0; }
            _whiteAttacks.speed = speed;
            _blackAttacks.speed = speed;
            _whiteEffects.speed = speed;
            _blackEffects.speed = speed;

            _whiteAction = ActionType.Null;
            _blackAction = ActionType.Null;
        }

        public void SetAction(ActionType action, Faction faction)
        {
            if (Options.combatAnimationsOn)
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
