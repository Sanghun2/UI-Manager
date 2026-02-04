using UnityEngine;
using TMPro;

namespace BilliotGames
{
    public class TextBase : UIBase
    {
        [SerializeField] TextMeshProUGUI targetText;

        public virtual void SetText(string text) {
            targetText.text = text; 
        }

        protected virtual void Reset() {
            if (targetText == null) {
                targetText = GetComponentInChildren<TextMeshProUGUI>();
            }
        }
    }
}
