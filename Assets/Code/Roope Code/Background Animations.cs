using UnityEngine;

namespace Crognard
{
    public class BackgroundAnimations : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        public int _background = 0;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetInteger("ID", _background);
        }
    }
}
