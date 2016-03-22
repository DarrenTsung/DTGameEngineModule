using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public class UserIdInventory<TEntity> where TEntity : DTEntity, new() {
    // PRAGMA MARK - Static
    private static UserIdInventory<TEntity> _instance;
		private static object _lock = new object();

    public static UserIdInventory<TEntity> Instance {
      get {
        lock (_lock) {
          if (_instance == null) {
            _instance = new UserIdInventory<TEntity>();
          }

          return _instance;
        }
      }
    }


    // PRAGMA MARK - Public Interface
    public Action OnInventoryUpdated = delegate {};
    public Action<IdQuantity<TEntity>> OnAddedIdQuantity = delegate {};
    public Action<IdQuantity<TEntity>> OnRemovedIdQuantity = delegate {};

    public void AddIdQuantity(IdQuantity<TEntity> addQuantity) {
      this._idQuantityInventory.AddIdQuantity(addQuantity);
      this.OnInventoryUpdated.Invoke();
      this.OnAddedIdQuantity.Invoke(addQuantity);
    }

    public bool CanRemoveIdQuantityList(IEnumerable<IdQuantity<TEntity>> removeQuantities) {
      foreach (IdQuantity<TEntity> removeQuantity in removeQuantities) {
        if (!this.CanRemoveIdQuantity(removeQuantity)) {
          return false;
        }
      }

      return true;
    }

    public bool CanRemoveIdQuantity(IdQuantity<TEntity> removeQuantity) {
      return this._idQuantityInventory.CanRemoveIdQuantity(removeQuantity);
    }

    public void RemoveIdQuantityList(IEnumerable<IdQuantity<TEntity>> removeQuantities) {
      foreach (IdQuantity<TEntity> removeQuantity in removeQuantities) {
        this.RemoveIdQuantity(removeQuantity);
      }
    }

    public void RemoveIdQuantity(IdQuantity<TEntity> removeQuantity) {
      this._idQuantityInventory.RemoveIdQuantity(removeQuantity);
      this.OnInventoryUpdated.Invoke();
      this.OnRemovedIdQuantity.Invoke(removeQuantity);
    }

    public int GetCountOfId(int id) {
      return this._idQuantityInventory.GetCountOfId(id);
    }


    // PRAGMA MARK - Internal
    private IdQuantityInventory<TEntity> _idQuantityInventory = new IdQuantityInventory<TEntity>();
  }
}