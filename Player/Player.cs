using DT;
using System.Collections;
﻿using UnityEngine;

public class Player : MonoBehaviour {
  #region mark - Interface 
  
  public int PlayerIndex {
    set { _playerIndex = value; }
    get { return _playerIndex; }
  }
  
  #endregion
  
  // PRAGMA MARK - INTERNAL
  [SerializeField, ReadOnly]
  protected int _playerIndex = 0;
  
  protected virtual void Awake() {
    this.RegisterNotifications();
  }
  
  protected void OnDisable() {
    this.RemoveNotifications();
  }
  
  protected virtual void RegisterNotifications() {
    DTNotifications.HandlePrimaryDirection.AddListener(this.HandlePrimaryDirectionWrapper);
    DTNotifications.HandleSecondaryDirection.AddListener(this.HandleSecondaryDirectionWrapper);
  }
  
  protected virtual void RemoveNotifications() {
    DTNotifications.HandlePrimaryDirection.RemoveListener(this.HandlePrimaryDirectionWrapper);
    DTNotifications.HandleSecondaryDirection.RemoveListener(this.HandleSecondaryDirectionWrapper);
  }
  
  protected void HandlePrimaryDirectionWrapper(int playerIndex, Vector2 primaryDirection) {
    if (playerIndex == _playerIndex) this.HandlePrimaryDirection(primaryDirection);
  }
  
  protected virtual void HandlePrimaryDirection(Vector2 primaryDirection) {
    // do nothing for now
  }
  
  protected void HandleSecondaryDirectionWrapper(int playerIndex, Vector2 secondaryDirection) {
    if (playerIndex == _playerIndex) this.HandleSecondaryDirection(secondaryDirection);
  }
  
  protected virtual void HandleSecondaryDirection(Vector2 secondaryDirection) {
    // do nothing for now
  }
}
