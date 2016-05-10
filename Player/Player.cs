using DT;
using System.Collections;
using System.Collections.Generic;
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

    protected List<PlayerModule> _playerModules = new List<PlayerModule>();

    protected virtual void Awake() {
      this.AwakeSetupModules();

      foreach (PlayerModule module in this._playerModules) {
        module.InitializeAfterAllModulesAdded();
      }
    }

    protected void OnDisable() {
      this.CleanupNotifications();
    }

    protected virtual void AwakeSetupModules() {}

    protected T CreateModule<T>() where T : PlayerModule {
      GameObject moduleObject = new GameObject();
      moduleObject.name = typeof(T).Name;
      moduleObject.transform.SetParent(this.transform);

      T moduleComponent = moduleObject.AddComponent<T>();
      moduleComponent.SetupWithContext(this);

      this._playerModules.Add(moduleComponent);

      return moduleComponent;
    }

    protected virtual void RegisterNotifications() { }
    protected virtual void CleanupNotifications() { }
  }
}