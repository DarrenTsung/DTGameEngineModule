using DT;
using System.Collections;
﻿using UnityEngine;

public abstract class Player : MonoBehaviour {
  // PRAGMA MARK - INTERNAL
  protected void Awake() {
    this.RegisterNotifications();
  }
  
  protected virtual void RegisterNotifications() {
    DTNotifications.HandlePrimaryDirection.AddListener(this.HandlePrimaryDirection);
    DTNotifications.HandleSecondaryDirection.AddListener(this.HandleSecondaryDirection);
  }
  
  protected virtual void HandlePrimaryDirection(Vector2 primaryDirection) {
    // do nothing
  }
  
  protected virtual void HandleSecondaryDirection(Vector2 secondaryDirection) {
    // do nothing
  }
}
