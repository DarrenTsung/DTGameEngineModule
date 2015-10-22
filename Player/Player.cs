using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class Player : MonoBehaviour {
    // PRAGMA MARK - Interface 
    public int PlayerIndex {
      set { 
        this.CleanupNotifications();
        _playerIndex = value; 
        this.RegisterNotifications();
      }
      get { return _playerIndex; }
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
    
    protected void DisableInputAfterDelay(float delay) {
      StartCoroutine(this.DisableInputAfterDelayCoroutine(delay));
    }
    
    protected IEnumerator DisableInputAfterDelayCoroutine(float delay) {
      yield return new WaitForSeconds(delay);
      this.DisableInput();
    }
    
    protected void DisableInput() {
      Toolbox.GetInstance<IPlayerInputManager>().SetInputDisabledForPlayer(_playerIndex, true);
    }
    
    protected void EnableInputAfterDelay(float delay) {
      StartCoroutine(this.EnableInputAfterDelayCoroutine(delay));
    }
    
    protected IEnumerator EnableInputAfterDelayCoroutine(float delay) {
      yield return new WaitForSeconds(delay);
      this.EnableInput();
    }
    
    protected void EnableInput() {
      Toolbox.GetInstance<IPlayerInputManager>().SetInputDisabledForPlayer(_playerIndex, false);
    }
  }
}