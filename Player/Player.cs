using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class Player : MonoBehaviour {
    // PRAGMA MARK - Interface 
    public int PlayerIndex {
      set { _playerIndex = value; }
      get { return _playerIndex; }
    }
    
    // PRAGMA MARK - Internal
    [SerializeField, ReadOnly]
    protected int _playerIndex = 0;
    
    protected virtual void Awake() {
      this.RegisterNotifications();
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