using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class ControllerPlayer : Player {
    // PRAGMA MARK - Public Interface 
    
    
    // PRAGMA MARK - Internal
    protected override void RegisterNotifications() {
      Toolbox.GetInstance<IControllerPlayerInputManager>().GetPrimaryDirectionEvent(_playerIndex).AddListener(this.HandlePrimaryDirection);
      Toolbox.GetInstance<IControllerPlayerInputManager>().GetSecondaryDirectionEvent(_playerIndex).AddListener(this.HandleSecondaryDirection);
    }
    
    protected override void CleanupNotifications() {
      Toolbox.GetInstance<IControllerPlayerInputManager>().GetPrimaryDirectionEvent(_playerIndex).RemoveListener(this.HandlePrimaryDirection);
      Toolbox.GetInstance<IControllerPlayerInputManager>().GetSecondaryDirectionEvent(_playerIndex).RemoveListener(this.HandleSecondaryDirection);
    }
    
    protected virtual void HandlePrimaryDirection(Vector2 primaryDirection) {
      // do nothing for now
    }
    
    protected virtual void HandleSecondaryDirection(Vector2 secondaryDirection) {
      // do nothing for now
    }
  }
}