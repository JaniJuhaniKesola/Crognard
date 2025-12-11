using UnityEngine;

namespace Crognard
{
    public class BackgroundAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public int _background = 0;

        private void Start()
        {
            if (Options.backgroundAnimationsOn)
            { _animator.speed = 1; }
            else
            { _animator.speed = 0; }
            ChangeBackground(Random.Range(0, 12));
        }

        public void ChangeBackground(int id)
        {
            _animator.SetInteger("ID", id);
        }
    }
}
