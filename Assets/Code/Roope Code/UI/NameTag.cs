using UnityEngine;
using TMPro;

namespace Crognard
{
    public class NameTag : MonoBehaviour
    {
        TextMeshProUGUI myText;
        RectTransform rectTransform;

        private void Start()
        {
            myText = GetComponentInChildren<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }
        
        public void NewName(string name)
        {
            if (myText == null)
            { myText = GetComponentInChildren<TextMeshProUGUI>(); }
            myText.text = name;
            GetRenderedValues();
        }

        private void GetRenderedValues()
        {
            if (rectTransform == null)
            { rectTransform = GetComponent<RectTransform>(); }
            float newRender = myText.preferredWidth;
            newRender += 2 * Mathf.Abs(myText.rectTransform.localPosition.x);
            // Debug.Log(newRender);
            rectTransform.sizeDelta = new Vector2(newRender, rectTransform.sizeDelta.y);
        }
        
    }
}
