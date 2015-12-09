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
    
    protected virtual void RegisterNotifications() {
      Toolbox.GetInstance<IPlayerInputManager>().GetPrimaryDirectionEvent(_playerIndex).AddListener(this.HandlePrimaryDirection);
      Toolbox.GetInstance<IPlayerInputManager>().GetSecondaryDirectionEvent(_playerIndex).AddListener(this.HandleSecondaryDirection);
    }
    
    protected virtual void CleanupNotifications() {
      Toolbox.GetInstance<IPlayerInputManager>().GetPrimaryDirectionEvent(_playerIndex).RemoveListener(this.HandlePrimaryDirection);
      Toolbox.GetInstance<IPlayerInputManager>().GetSecondaryDirectionEvent(_playerIndex).RemoveListener(this.HandleSecondaryDirection);
    }
    
    protected virtual void HandlePrimaryDirection(Vector2 primaryDirection) {
      // do nothing for now
    }
    
    protected virtual void HandleSecondaryDirection(Vector2 secondaryDirection) {
      // do nothing for now
    }
  }
}