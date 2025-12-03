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

        // Update is called once per frame
        private void Update()
        {
            _whiteAttacks.SetInteger("Attack", (int)_whiteAction);
            _blackAttacks.SetInteger("Attack", (int)_blackAction);

            _whiteEffects.SetInteger("Effect", (int)_whiteAction);
            _blackEffects.SetInteger("Effect", (int)_blackAction);
        }
    }
}
