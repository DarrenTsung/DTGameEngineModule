using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public class UserIdInventoryObserver {
    // PRAGMA MARK - Public Interface
    public UserIdInventoryObserver(bool startImmediately = false) {
      this._active = false;
      if (startImmediately) {
        this.StartObserving();
      }
    }

    public void StartObserving() {
      if (this._active) {
        Debug.LogWarning("StartObserving - called when already active!");
        return;
      }

      this._active = true;
      this.ClearRecordings();
      this.AddListeners();
    }

    public void StopObserving() {
      if (!this._active) {
        Debug.LogWarning("StopObserving - called with not active, wasn't observing!");
        return;
      }

      this._active = false;
      this.RemoveListeners();
    }

    public void ClearRecordings() {
      this._addedEntityQuantities = new List<EntityQuantity>();
      this._removedEntityQuantities = new List<EntityQuantity>();
    }

    public EntityQuantity[] GetAllAdded() {
      return this._addedEntityQuantities.ToArray();
    }

    public EntityQuantity[] GetAllRemoved() {
      return this._removedEntityQuantities.ToArray();
    }


    // PRAGMA MARK - Internal
    private List<EntityQuantity> _addedEntityQuantities = new List<EntityQuantity>();
    private List<EntityQuantity> _removedEntityQuantities = new List<EntityQuantity>();

    private bool _active = false;

    private void HandleAddedEntityQuantity(EntityQuantity addedQuantity) {
      this._addedEntityQuantities.Add(addedQuantity);
    }

    private void HandleRemovedEntityQuantity(EntityQuantity removedQuantity) {
      this._removedEntityQuantities.Add(removedQuantity);
    }

    private void AddListeners() {
      UserIdInventory.OnAddedEntityQuantity += this.HandleAddedEntityQuantity;
      UserIdInventory.OnRemovedEntityQuantity += this.HandleRemovedEntityQuantity;
    }

    private void RemoveListeners() {
      UserIdInventory.OnAddedEntityQuantity -= this.HandleAddedEntityQuantity;
      UserIdInventory.OnRemovedEntityQuantity -= this.HandleRemovedEntityQuantity;
    }
  }
}