using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public class UserIdInventoryObserver<TEntity> where TEntity : DTEntity {
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
      this._addedIdQuantityInventory = new IdQuantityInventory<TEntity>();
      this._removedIdQuantityInventory = new IdQuantityInventory<TEntity>();
    }

    public IdQuantity<TEntity>[] GetAllAdded() {
      return this._addedIdQuantityInventory.ToArray();
    }

    public IdQuantity<TEntity>[] GetAllRemoved() {
      return this._removedIdQuantityInventory.ToArray();
    }


    // PRAGMA MARK - Internal
    private IdQuantityInventory<TEntity> _addedIdQuantityInventory = new IdQuantityInventory<TEntity>();
    private IdQuantityInventory<TEntity> _removedIdQuantityInventory = new IdQuantityInventory<TEntity>();

    private bool _active = false;

    private void HandleAddedIdQuantity(IdQuantity<TEntity> addedQuantity) {
      this._addedIdQuantityInventory.AddIdQuantity(addedQuantity);
    }

    private void HandleRemovedIdQuantity(IdQuantity<TEntity> removedQuantity) {
      this._removedIdQuantityInventory.AddIdQuantity(removedQuantity);
    }

    private void AddListeners() {
      UserIdInventory<TEntity>.Instance.OnAddedIdQuantity += this.HandleAddedIdQuantity;
      UserIdInventory<TEntity>.Instance.OnRemovedIdQuantity += this.HandleRemovedIdQuantity;
    }

    private void RemoveListeners() {
      UserIdInventory<TEntity>.Instance.OnAddedIdQuantity -= this.HandleAddedIdQuantity;
      UserIdInventory<TEntity>.Instance.OnRemovedIdQuantity -= this.HandleRemovedIdQuantity;
    }
  }
}