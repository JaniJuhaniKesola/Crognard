using UnityEngine;

namespace Crognard
{
    public class VictoryConfetti : MonoBehaviour
    {
        private ParticleSystem _particle;

        private void Start()
        {
            _particle = GetComponent<ParticleSystem>();
            StopConfetti();
        }

        public void StartConfetti()
        {
            _particle.Play();
        }

        public void StopConfetti()
        {
            _particle.Stop();
        }
    }
}
