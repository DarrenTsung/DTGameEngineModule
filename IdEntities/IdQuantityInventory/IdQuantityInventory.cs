using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  [Serializable]
  public class IdQuantityInventory<TEntity> : IEnumerable<IdQuantity<TEntity>> where TEntity : DTEntity, new() {
    // PRAGMA MARK - Public Interface
    public void GainIdQuantity(IdQuantity<TEntity> gainQuantity) {
      IdQuantity<TEntity> idQuantity = this.GetIdQuantityForId(gainQuantity.id);
      idQuantity.quantity += gainQuantity.quantity;
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
      IdQuantity<TEntity> idQuantity = this.GetIdQuantityForId(neededQuantity.id);
      return idQuantity.quantity >= neededQuantity.quantity;
    }

    public void SpendIdQuantityList(IEnumerable<IdQuantity<TEntity>> spendQuantities) {
      foreach (IdQuantity<TEntity> spendQuantity in spendQuantities) {
        this.SpendIdQuantity(spendQuantity);
      }
    }

    public void SpendIdQuantity(IdQuantity<TEntity> spendQuantity) {
      IdQuantity<TEntity> idQuantity = this.GetIdQuantityForId(spendQuantity.id);
      if (idQuantity.quantity < spendQuantity.quantity) {
        Debug.LogWarning("SpendIdQuantity: Can't spend " + spendQuantity.quantity + " of " + typeof(TEntity).Name + " id: " + spendQuantity.id + "!");
        return;
      }

      idQuantity.quantity -= spendQuantity.quantity;
    }

    public int GetCountOfId(int id) {
      return this.GetIdQuantityForId(id).quantity;
    }


    // PRAGMA MARK - IEnumerable<IdQuantity<TEntity>> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<IdQuantity<TEntity>> GetEnumerator() {
      return this._idList.GetEnumerator();
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    private List<IdQuantity<TEntity>> _idList = new List<IdQuantity<TEntity>>();

    private Dictionary<int, IdQuantity<TEntity>> _idMap = new Dictionary<int, IdQuantity<TEntity>>();
    private bool _initialized = false;

    private void RefreshMap() {
      this._idMap.Clear();

      foreach (IdQuantity<TEntity> idQuantity in this._idList) {
        this._idMap[idQuantity.id] = idQuantity;
      }
    }

    private IdQuantity<TEntity> GetIdQuantityForId(int id) {
      if (!this._initialized) {
        this.RefreshMap();
        this._initialized = true;
      }

      if (!this._idMap.ContainsKey(id)) {
        DTEntity entity = IdList<TEntity>.Instance.LoadById(id);
        if (entity == null) {
          Debug.LogWarning("GetIdQuantityForId - called with invalid " + typeof(TEntity).Name + " id: " + id + "!");
          return null;
        }

        IdQuantity<TEntity> idQuantity = new IdQuantity<TEntity>(id, quantity: 0);
        this._idList.Add(idQuantity);
        this._idMap[id] = idQuantity;
      }

      return this._idMap[id];
    }
  }
}