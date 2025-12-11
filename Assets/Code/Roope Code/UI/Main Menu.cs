using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Crognard
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _menu, _manual, _options, _credits;
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private Sprite[] _clip;
        [SerializeField] private float _framerate = 0.02f;

        private GameObject _currentMenu;

        private void Start()
        {
            _currentMenu = _menu;
        } 

        public void OnPlay()
        {
            SceneManager.LoadScene("JaniTest");
        }

        public void OnManual()
        {
            Transition(_currentMenu, _manual);
        }

        public void OnSettings()
        {
            Transition(_currentMenu, _options);
        }

        public void OnCredits()
        {
            Transition(_currentMenu, _credits);
        }

        public void OnReturn()
        {
            Transition(_currentMenu, _menu);
        }

        private void Transition(GameObject start, GameObject goal)
        {
            start.SetActive(false);
            if (Options.backgroundAnimationsOn)
            {
                StartCoroutine(Animation(goal));
            }
            else
            {
                goal.SetActive(true);
            }
            _currentMenu = goal;
        }

        private IEnumerator Animation(GameObject goal)
        {
            if (goal != _menu)
            {
                for (int i = 0; i < _clip.Length; i++)
                {
                    for (float j = 0; j < _framerate;)
                    {
                        _background.sprite = _clip[i];
                        j += Time.deltaTime;
                        yield return null;
                    }
                    yield return null;
                }
            }
            else
            {
                for (int i = _clip.Length-1; i > 0; i--)
                {
                    for (float j = 0; j < _framerate;)
                    {
                        _background.sprite = _clip[i];
                        j += Time.deltaTime;
                        yield return null;
                    }
                    yield return null;
                }
            }
            goal.SetActive(true);
        }

        public void OnCombatAnimation()
        {
            Options.combatAnimationsOn = !Options.combatAnimationsOn;
        }

        public void OnBackgroundAnimation()
        {
            Options.backgroundAnimationsOn = !Options.backgroundAnimationsOn;
        }
    }
}
