using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BilliotGames
{
    public abstract class ButtonBase : UIBase
    {
        [SerializeField] Button targetButton;

        public void SetButtonAction(UnityAction buttonAction) {
            targetButton.onClick.RemoveAllListeners();
            targetButton.onClick.AddListener(buttonAction);
        }
        protected abstract void ButtonAction();

        protected override void Start() {
            if (targetButton != null && !_isInit) {
                SetButtonAction(ButtonAction);
                _isInit = true;
            }
        }

        protected virtual void Reset() {
            if (targetButton == null) {
                targetButton = GetComponentInChildren<Button>();
            }
        }
    }
}

