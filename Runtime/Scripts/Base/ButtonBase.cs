using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonBase : UIBase
{
    [SerializeField] Button targetButton;

    public void SetCustomButtonAction(UnityAction buttonAction) {
        targetButton.onClick.RemoveAllListeners();
        targetButton.onClick.AddListener(buttonAction);
    }
    protected abstract void ButtonAction();

    protected override void Start() {
        if (targetButton != null && !_isInit) {
            SetCustomButtonAction(ButtonAction);
            _isInit = true;
        }
    }
}
