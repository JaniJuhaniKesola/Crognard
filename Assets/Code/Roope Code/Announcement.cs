using TMPro;
using UnityEngine;

namespace Crognard
{
    public class Announcement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _box; 

        #region Start
        public void Challenge(string attacker, string defender)
        {
            _box.text = attacker + " has challenged " + defender + " for a fight!";
        }

        public void Select()
        {
            _box.text = "What will you do?";
        }

        public void Stamina()
        {
            _box.text = "You don't have enough Stamina for this!";
        }

        public void TooLong()
        {
            _box.text = "Can You please HURRY UP!";
        }
        #endregion

        #region Actions
        public void Attacks(string attacker, string target)
        {
            Debug.Log("Attack Announcement");
            _box.text = attacker + " attacks " + target + "!";
        }

        public void Defending(string defender)
        {
            _box.text = defender + " raised their guard!";
        }

        public void Counter(string user)
        {
            _box.text = user + " prepares for an attack.";
        }

        public void Healed(string user)
        {
            _box.text = user + " drank a Healing Potion.";
        }

        public void Sticky(string attacker, string target)
        {
            _box.text = attacker + " threw a sticky bomb at " + target + "!";
        }

        public void Smoke(string user)
        {
            _box.text = user + " set a Smoke Bomb!";
        }
        #endregion

        #region Consequenses
        public void Miss(string attacker)
        {
            _box.text += " But " + attacker + " missed!";
        }

        public void Critical(string attacker)
        {
            _box.text += " And " + attacker + " landed a CRITICAL HIT";
        }

        public void Kill(string target)
        {
            _box.text = target + " is defeated.";
        }

        public void Blocked(string defender)
        {
            _box.text = "But " + defender + " blocked it!";
        }

        public void CounterAttack(string attacker, string target)
        {
            _box.text = attacker + " unleashed a Counter Attack towards " + target;
        }

        public void Escape(string user)
        {
            _box.text = user + " has fled the fight!";
        }

        public void OutOfStamina(string user)
        {
            _box.text = user + " is out of Stamina";
        }
        #endregion

        #region End
        public void Neutral()
        {
            _box.text = "The battle is at stalemate. No Victory within sight.";
        }

        public void Attacker(string attacker, string target)
        {
            _box.text = attacker + " has captured " + target;
        }

        public void Defender(string defender, string target)
        {
            _box.text = defender + " has defeated the invader " + target;
        }
        #endregion
    }
}
