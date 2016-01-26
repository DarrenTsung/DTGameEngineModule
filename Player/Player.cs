using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class Player : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public int PlayerIndex {
      set {
        this.CleanupNotifications();
        _playerIndex = value;
        this.RegisterNotifications();
      }
      get { return _playerIndex; }
    }

    public void DisableInputAfterDelay(float delay) {
      this.DoAfterDelay(delay, () => { this.DisableInput(); });
    }

    public void DisableInputForTime(float time) {
      this.DisableInput();
      this.EnableInputAfterDelay(time);
    }

    public void DisableInput() {
      Toolbox.GetInstance<IPlayerInputManager>().SetInputDisabledForPlayer(_playerIndex, true);
    }

    public void EnableInputAfterDelay(float delay) {
      this.DoAfterDelay(delay, () => { this.EnableInput(); });
    }

    public void EnableInput() {
      Toolbox.GetInstance<IPlayerInputManager>().SetInputDisabledForPlayer(_playerIndex, false);
    }

    // PRAGMA MARK - Internal
    [SerializeField, ReadOnly]
    protected int _playerIndex = 0;

    protected virtual void Awake() {
      // do nothing
    }

    protected void OnDisable() {
      this.CleanupNotifications();
    }

    protected T CreateModule<T>() where T : PlayerModule {
      GameObject moduleObject = new GameObject();
      moduleObject.name = typeof(T).Name;
      moduleObject.transform.SetParent(this.transform);

      T moduleComponent = moduleObject.AddComponent<T>();
      moduleComponent.SetupWithContext(this);
      return moduleComponent;
    }

    protected virtual void RegisterNotifications() { }
    protected virtual void CleanupNotifications() { }
  }
}