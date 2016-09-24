using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public static class UserIdInventory {
    public static event Action<IIdQuantity> OnAddedIdQuantity = delegate {};
    public static event Action<IIdQuantity> OnRemovedIdQuantity = delegate {};

    public static void NotifyListenersOfAddedIdQuantity(IIdQuantity idQuantity) {
      UserIdInventory.OnAddedIdQuantity.Invoke(idQuantity);
    }

    public static void NotifyListenersOfRemovedIdQuantity(IIdQuantity idQuantity) {
      UserIdInventory.OnRemovedIdQuantity.Invoke(idQuantity);
    }
  }

  [Serializable]
  public partial class UserIdInventory<TEntity> : IUserIdInventory, IEnumerable<IdQuantity<TEntity>> where TEntity : DTEntity {
    // PRAGMA MARK - Static
    public static Action<UserIdInventory<TEntity>> OnUserInventoryFirstCreated = delegate {};

    public static UserIdInventory<TEntity> Instance {
      get { return InstanceUtil.Instance; }
    }


    // PRAGMA MARK - Public Interface
    [field: NonSerialized] public event Action OnInventoryUpdated = delegate {};
    [field: NonSerialized] public event Action<IdQuantity<TEntity>> OnAddedIdQuantity = delegate {};
    [field: NonSerialized] public event Action<IdQuantity<TEntity>> OnRemovedIdQuantity = delegate {};

    public UserIdInventory() {
      this._idQuantityInventory.OnInventoryUpdated += this.HandleInventoryUpdated;
      this._idQuantityInventory.OnAddedIdQuantity += this.HandleAddedIdQuantity;
      this._idQuantityInventory.OnRemovedIdQuantity += this.HandleRemovedIdQuantity;
    }

    public void AddIdQuantity(IdQuantity<TEntity> addQuantity) {
      this._idQuantityInventory.AddIdQuantity(addQuantity);
    }

    public bool CanRemoveIdQuantity(IdQuantity<TEntity> removeQuantity) {
      return this._idQuantityInventory.CanRemoveIdQuantity(removeQuantity);
    }

    public bool RemoveIdQuantity(IdQuantity<TEntity> removeQuantity) {
      return this._idQuantityInventory.RemoveIdQuantity(removeQuantity);
    }

    public int GetCountOfId(int id) {
      return this._idQuantityInventory.GetCountOfId(id);
    }


    // PRAGMA MARK - IUserIdInventory Implementation
    void IUserIdInventory.AddIdQuantity(int id, int quantity) {
      this.AddIdQuantity(new IdQuantity<TEntity>(id, quantity));
    }

    void IUserIdInventory.RemoveIdQuantity(int id, int quantity) {
      this.RemoveIdQuantity(new IdQuantity<TEntity>(id, quantity));
    }


    // PRAGMA MARK - Internal
    [SerializeField] private IdQuantityInventory<TEntity> _idQuantityInventory = new IdQuantityInventory<TEntity>();

    // after deserialization from formatter, we want to re-initialize any fields
    // that have the [NonSerialized] attribute
    [OnDeserializing]
    private void CreateEventsOnDeserialization(StreamingContext context) {
      this.OnInventoryUpdated = delegate {};
      this.OnAddedIdQuantity = delegate {};
      this.OnRemovedIdQuantity = delegate {};
    }

    [OnDeserialized]
    private void ListenToEventsAfterDeserialization(StreamingContext context) {
      this._idQuantityInventory.OnInventoryUpdated += this.HandleInventoryUpdated;
      this._idQuantityInventory.OnAddedIdQuantity += this.HandleAddedIdQuantity;
      this._idQuantityInventory.OnRemovedIdQuantity += this.HandleRemovedIdQuantity;
    }

    private void HandleInventoryUpdated() {
      InstanceUtil.DirtyInstance();
      this.OnInventoryUpdated.Invoke();
    }

    private void HandleAddedIdQuantity(IdQuantity<TEntity> addQuantity) {
      this.OnAddedIdQuantity.Invoke(addQuantity);
      UserIdInventory.NotifyListenersOfAddedIdQuantity(addQuantity);
    }

    private void HandleRemovedIdQuantity(IdQuantity<TEntity> removeQuantity) {
      this.OnRemovedIdQuantity.Invoke(removeQuantity);
      UserIdInventory.NotifyListenersOfRemovedIdQuantity(removeQuantity);
    }
  }
}