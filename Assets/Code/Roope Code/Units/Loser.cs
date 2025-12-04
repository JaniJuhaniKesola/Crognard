using System.Collections;
using UnityEngine;

namespace Crognard
{
    public class Loser : MonoBehaviour
    {
        [SerializeField] private int _deathFrames = 2;
        [SerializeField] private Vector2 _direction = new Vector2(2, 2);
        [SerializeField] private float _launchSpeed = 5;
        [SerializeField] private float _rotationSpeed = 180;
        [SerializeField] private float _fallSpeed = 5;
        [SerializeField] private float _vanishSpeed = 0.5f;
        private SpriteRenderer _spriteRend;
        private int _option = 0;

        private void Start()
        {
            _spriteRend = GetComponent<SpriteRenderer>();
            // StartCoroutine(Death());
        }

        private IEnumerator Death()
        {
            for (float i = 0; i < _deathFrames;)
            {
                switch (_option)
                {
                    case 1: Launch(); break;
                    case 2: Fall(); break;
                    case 3: Vanish(); break;
                    /*
                    case 4: Explode(); break;
                    case 5: CutOut(); break;
                    */
                    default: break;
                }
                Debug.Log(i +" Time has passed");
                i += Time.deltaTime;
                yield return null;
            }
            Debug.Log("End of Death");
        }

        public void YouLose()
        {
            _option = Random.Range(1, 4);
            StartCoroutine(Death());
        }

        /// <summary>
        /// Piece shatters tomillion pieces
        /// </summary>
        private void Explode()
        {
            
        }

        /// <summary>
        /// Piece is cut from the middle and the top falls
        /// </summary>
        private void CutOut()
        {
            
        }

        /// <summary>
        /// Piece is launched out of camera
        /// </summary>
        private void Launch()
        {
            Vector2 position = transform.position;
            if (transform.position.x > 0)
            {
                position += _direction * _launchSpeed  * Time.deltaTime;
                transform.position = position;

                transform.Rotate(Vector3.back * _launchSpeed * _rotationSpeed * Time.deltaTime);
            }
            else if (transform.position.x < 0)
            {
                position += new Vector2(-_direction.x, _direction.y) * _launchSpeed * Time.deltaTime;
                transform.position = position;

                transform.Rotate(Vector3.forward * _launchSpeed * _rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Piece falls down like in pokemon.
        /// </summary>
        private void Fall()
        {
            Vector2 position = transform.position;
            position += Vector2.down * _fallSpeed * Time.deltaTime;
            transform.position = position;
        }

        /// <summary>
        /// Piece becomes transparent like in FF7
        /// </summary>
        private void Vanish()
        {
            float aValue = _spriteRend.color.a;

            aValue -= _vanishSpeed * Time.deltaTime;
            _spriteRend.color = new Color(1, aValue, aValue, aValue);
        }
    }
}
