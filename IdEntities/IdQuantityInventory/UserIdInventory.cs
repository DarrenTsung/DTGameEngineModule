using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  [Serializable]
  public partial class UserIdInventory<TEntity> : IEnumerable<IdQuantity<TEntity>> where TEntity : DTEntity {
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

    public bool RemoveIdQuantityList(IEnumerable<IdQuantity<TEntity>> removeQuantities) {
      return this._idQuantityInventory.RemoveIdQuantityList(removeQuantities);
    }

    public bool RemoveIdQuantity(IdQuantity<TEntity> removeQuantity) {
      return this._idQuantityInventory.RemoveIdQuantity(removeQuantity);
    }

    public int GetCountOfId(int id) {
      return this._idQuantityInventory.GetCountOfId(id);
    }


    public UserIdInventory() {
      this._idQuantityInventory.OnInventoryUpdated += this.HandleInventoryUpdated;
      this._idQuantityInventory.OnAddedIdQuantity += this.HandleAddedIdQuantity;
      this._idQuantityInventory.OnRemovedIdQuantity += this.HandleRemovedIdQuantity;
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

    private void HandleInventoryUpdated() {
      InstanceUtil.DirtyInstance();
      this.OnInventoryUpdated.Invoke();
    }

    private void HandleAddedIdQuantity(IdQuantity<TEntity> addQuantity) {
      this.OnAddedIdQuantity.Invoke(addQuantity);
    }

    private void HandleRemovedIdQuantity(IdQuantity<TEntity> removeQuantity) {
      this.OnRemovedIdQuantity.Invoke(removeQuantity);
    }
  }
}