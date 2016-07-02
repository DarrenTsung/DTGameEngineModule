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
      this._addedIdQuantities = new List<IIdQuantity>();
      this._removedIdQuantities = new List<IIdQuantity>();
    }

    public IIdQuantity[] GetAllAdded() {
      return this._addedIdQuantities.ToArray();
    }

    public IIdQuantity[] GetAllRemoved() {
      return this._removedIdQuantities.ToArray();
    }


    // PRAGMA MARK - Internal
    private List<IIdQuantity> _addedIdQuantities = new List<IIdQuantity>();
    private List<IIdQuantity> _removedIdQuantities = new List<IIdQuantity>();

    private bool _active = false;

    private void HandleAddedIdQuantity(IIdQuantity addedQuantity) {
      this._addedIdQuantities.Add(addedQuantity);
    }

    private void HandleRemovedIdQuantity(IIdQuantity removedQuantity) {
      this._removedIdQuantities.Add(removedQuantity);
    }

    private void AddListeners() {
      UserIdInventory.OnAddedIdQuantity += this.HandleAddedIdQuantity;
      UserIdInventory.OnRemovedIdQuantity += this.HandleRemovedIdQuantity;
    }

    private void RemoveListeners() {
      UserIdInventory.OnAddedIdQuantity -= this.HandleAddedIdQuantity;
      UserIdInventory.OnRemovedIdQuantity -= this.HandleRemovedIdQuantity;
    }
  }
}