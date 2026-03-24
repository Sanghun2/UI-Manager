using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BilliotGames
{
    public abstract class ButtonBase : UIBase
    {
        [SerializeField] protected Button targetButton;

        public override void InitUI() {
            if (IsInit) return;
            base.InitUI();
            if (targetButton != null) {
                SetButtonAction(ButtonAction);
            }
            _isInit = true;
        }

        public void SetButtonAction(Action buttonAction) {
            targetButton.onClick.RemoveAllListeners();
            targetButton.onClick.AddListener(() => buttonAction?.Invoke());
        }
        protected abstract void ButtonAction();

        protected virtual void Reset() {
            if (targetButton == null) {
                targetButton = GetComponentInChildren<Button>();
            }
        }
    }
}

