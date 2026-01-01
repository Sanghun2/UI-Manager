using System;
using System.Collections.Generic;
using UnityEngine;

public partial class UIManager
{    
    public int OpenedUICount
    {
        get
        {
            foreach (UIBase item in _openedUIStack) {
                Debug.LogAssertion($"열린 UI: {item.name}");
            }
            return _openedUIStack.Count;
        }
    }

    private Dictionary<Type, UIBase> uiDict = new Dictionary<Type, UIBase>();
    private Stack<UIBase> _openedUIStack = new Stack<UIBase>();

    public void ClearUIs() {
        CloseAllUIs();
        uiDict.Clear();
        _openedUIStack.Clear();
    }
    public void RemoveUI<T>(T uiBase) where T : UIBase {
        if (uiDict.ContainsKey(typeof(T))) {
            uiDict.Remove(typeof(T));
        }
    }

    public T GetUI<T>() where T : UIBase {
        return TryFindUI<T>(out T resultUI) ? resultUI : null;
    }
    public T OpenUI<T>(T targetUI) where T : UIBase {
        if (targetUI != null) {
            if (!targetUI.IsInit) targetUI.InitUI();
            targetUI.OpenUI();
            _openedUIStack.Push(targetUI);
        }
        
        return targetUI;
    }
    public T GetUI<T>(string prefabPath, Transform parent) where T : UIBase {
        if (TryFindUI<T>(out T resultUI)) {
            return resultUI;
        }

        if (resultUI == null && !string.IsNullOrEmpty(prefabPath)) {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            resultUI = InstantiateUI<T>(prefab, parent);
        }

        if (resultUI != null) {
            uiDict.Add(typeof(T), resultUI);
            return resultUI as T;
        }

        Debug.LogError($"{typeof(T)}에 해당하는 UI를 찾을 수 없음");
        return null;
    }
    public T GetUI<T>(GameObject uiPrefab, Transform parent) where T : UIBase {
        if (TryFindUI<T>(out T resultUI)) {
            return resultUI;
        }

        resultUI = InstantiateUI<T>(uiPrefab, parent);
        if (resultUI != null) {
            return resultUI;
        }

        Debug.LogError($"{typeof(T)}에 해당하는 UI를 찾을 수 없음");
        return null;
    }


    public T OpenUI<T>() where T : UIBase {
        return OpenUI(GetUI<T>());
    }
    public T OpenUI<T>(string prefabPath, Transform parent) where T : UIBase {
        return OpenUI(GetUI<T>(prefabPath, parent));
    }
    public T OpenUI<T>(GameObject uiPrefab, Transform parent) where T : UIBase {
        return OpenUI(GetUI<T>(uiPrefab, parent));
    }

    public void CloseUI<T>() where T : UIBase {
        if (TryRemoveUIFromStack(out T targetUI)) {
            targetUI.CloseUI();
        }
        else {
            Debug.LogError($"<color=red>close 하려는 ui({typeof(T)})가 stack에 존재하지 않음. OpenUI로 먼저 UI를 열어야 함</color>");
        }
    }
    public void CloseUI<T>(T targetUI) where T : UIBase {
        targetUI?.CloseUI();
        if (!TryRemoveUIFromStack(targetUI)) {
            Debug.LogWarning($"<color=yellow>close 하려는 ui({typeof(T)})가 stack에 존재하지 않음. 의도한 작업이 아니라면 OpenUI로 먼저 UI open 필요</color>");
        }
    }


    public void CloseTopUI() {
        if (_openedUIStack.Count > 0) {
            var targetUI = _openedUIStack.Pop();
            targetUI?.CloseUI();
        }
    }
    public void CloseAllUIs() {
        while (_openedUIStack.Count > 0) {
            CloseTopUI();
        }
    }

    public void ToggleUI<T>(T targetUI) where T : UIBase{
        if (targetUI.IsOpened) CloseUI<T>();
        else OpenUI<T>();
    }


    private bool TryFindUI<T>(out T resultUI) where T : UIBase {

        if (uiDict.TryGetValue(typeof(T), out UIBase findUI)) {
            resultUI = findUI as T;
            return true;
        }

        resultUI = GameObject.FindAnyObjectByType<T>(FindObjectsInactive.Include);
        return resultUI != null;
    }
    private T InstantiateUI<T>(GameObject prefab, Transform parent) where T : UIBase {
        GameObject loadedUIObj = GameObject.Instantiate(prefab, parent);
        if (loadedUIObj.GetComponentInChildren<UIBase>() is T resultUI) {
            return resultUI;
        }

        Debug.LogError($"<color=red>UI 생성 후 {typeof(T)} 찾기 시도했으나 찾을 수 없음</color>");
        return null;
    }
    private bool TryRemoveUIFromStack<T>(out T foundUI) where T : UIBase {
        bool success = false;
        foundUI = null;
        Stack<UIBase> tempUIStack = new Stack<UIBase>(_openedUIStack.Count);
        while (_openedUIStack.Count > 0) {
            var ui = _openedUIStack.Pop();
            if (typeof(T) == ui.GetType()) {
                foundUI = ui as T;
                success = true;
            }
            else {
                tempUIStack.Push(ui);
            }
        }

        while (tempUIStack.Count > 0) {
            _openedUIStack.Push(tempUIStack.Pop());
        }

        return success;
    }
    private bool TryRemoveUIFromStack<T>(T targetUI) where T : UIBase {
        bool success = false;
        Stack<UIBase> tempUIStack = new Stack<UIBase>(_openedUIStack.Count);
        while (_openedUIStack.Count > 0) {
            UIBase ui = _openedUIStack.Pop();
            if (ui.Equals(targetUI)) {
                ui.CloseUI();
                success = true;
            }
            else {
                tempUIStack.Push(ui);
            }
        }

        while (tempUIStack.Count > 0) {
            _openedUIStack.Push(tempUIStack.Pop());
        }

        return success;
    }
}
