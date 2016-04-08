using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  [Serializable]
  public partial class UserIdInventory<TEntity> where TEntity : DTEntity {
    // PRAGMA MARK - Static
    public static Action<UserIdInventory<TEntity>> OnUserInventoryFirstCreated = delegate {};

    public static UserIdInventory<TEntity> Instance {
      get { return InstanceUtil.Instance; }
    }


    // PRAGMA MARK - Public Interface
    [field: NonSerialized]
    public Action OnInventoryUpdated = delegate {};
    [field: NonSerialized]
    public Action<IdQuantity<TEntity>> OnAddedIdQuantity = delegate {};
    [field: NonSerialized]
    public Action<IdQuantity<TEntity>> OnRemovedIdQuantity = delegate {};

    public void AddIdQuantity(IdQuantity<TEntity> addQuantity) {
      this._idQuantityInventory.AddIdQuantity(addQuantity);
      this.OnInventoryUpdated.Invoke();
      InstanceUtil.DirtyInstance();
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
      InstanceUtil.DirtyInstance();
      this.OnRemovedIdQuantity.Invoke(removeQuantity);
    }

    public int GetCountOfId(int id) {
      return this._idQuantityInventory.GetCountOfId(id);
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    private IdQuantityInventory<TEntity> _idQuantityInventory = new IdQuantityInventory<TEntity>();

    // after deserialization from formatter, we want to re-initialize any fields
    // that have the [NonSerialized] attribute
    [OnDeserializing]
    private void CreateEventsOnDeserialization(StreamingContext context) {
      this.OnInventoryUpdated = delegate {};
      this.OnAddedIdQuantity = delegate {};
      this.OnRemovedIdQuantity = delegate {};
    }
  }
}