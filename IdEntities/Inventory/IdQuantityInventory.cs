using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  [Serializable]
  public class IdQuantityInventory<TEntity> : IEnumerable<IdQuantity<TEntity>> where TEntity : DTEntity {
    // PRAGMA MARK - Public Interface
    public void AddIdQuantity(IdQuantity<TEntity> addQuantity) {
      IdQuantity<TEntity> idQuantity = this.GetIdQuantityForId(addQuantity.id);
      idQuantity.quantity += addQuantity.quantity;
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
      IdQuantity<TEntity> idQuantity = this.GetIdQuantityForId(removeQuantity.id);
      return idQuantity.quantity >= removeQuantity.quantity;
    }

    public void RemoveIdQuantityList(IEnumerable<IdQuantity<TEntity>> removeQuantities) {
      foreach (IdQuantity<TEntity> removeQuantity in removeQuantities) {
        this.RemoveIdQuantity(removeQuantity);
      }
    }

    public void RemoveIdQuantity(IdQuantity<TEntity> removeQuantity) {
      IdQuantity<TEntity> idQuantity = this.GetIdQuantityForId(removeQuantity.id);
      if (idQuantity.quantity < removeQuantity.quantity) {
        Debug.LogWarning("RemoveIdQuantity: Can't remove " + removeQuantity.quantity + " of " + typeof(TEntity).Name + " id: " + removeQuantity.id + "!");
        return;
      }

      idQuantity.quantity -= removeQuantity.quantity;
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