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
    public Action<IdQuantity<TEntity>> OnGainedIdQuantity = delegate {};
    public Action<IdQuantity<TEntity>> OnSpentIdQuantity = delegate {};

    public void GainIdQuantity(IdQuantity<TEntity> gainQuantity) {
      this._idQuantityInventory.GainIdQuantity(gainQuantity);
      this.OnInventoryUpdated.Invoke();
      this.OnGainedIdQuantity.Invoke(gainQuantity);
    }

    public bool CanSpendIdQuantityList(IEnumerable<IdQuantity<TEntity>> neededQuantities) {
      foreach (IdQuantity<TEntity> neededQuantity in neededQuantities) {
        if (!this.CanSpendIdQuantity(neededQuantity)) {
          return false;
        }
      }

      return true;
    }

    public bool CanSpendIdQuantity(IdQuantity<TEntity> neededQuantity) {
      return this._idQuantityInventory.CanSpendIdQuantity(neededQuantity);
    }

    public void SpendIdQuantityList(IEnumerable<IdQuantity<TEntity>> spendQuantities) {
      foreach (IdQuantity<TEntity> spendQuantity in spendQuantities) {
        this.SpendIdQuantity(spendQuantity);
      }
    }

    public void SpendIdQuantity(IdQuantity<TEntity> spendQuantity) {
      this._idQuantityInventory.SpendIdQuantity(spendQuantity);
      this.OnInventoryUpdated.Invoke();
      this.OnSpentIdQuantity.Invoke(spendQuantity);
    }

    public int GetCountOfId(int id) {
      return this._idQuantityInventory.GetCountOfId(id);
    }


    // PRAGMA MARK - Internal
    private IdQuantityInventory<TEntity> _idQuantityInventory = new IdQuantityInventory<TEntity>();
  }
}