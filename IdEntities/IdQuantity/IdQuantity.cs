using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class IdQuantity<TEntity> : IIdQuantity where TEntity : DTEntity {
    [Id(hidePrefixLabel = true)]
    public int id;
    public int quantity;

    // NOTE (darren): this exists for subclasses to not have to define constructors
    public IdQuantity() {
      this.id = 0;
      this.quantity = 1;
    }

    public IdQuantity(int id, int quantity = 1) {
      this.id = id;
      this.quantity = quantity;
    }

    public TEntity GetEntity() { return IdList<TEntity>.Instance.LoadById(this.id); }


    // PRAGMA MARK - IIdQuantity Implementation
    public IUserIdInventory UserInventory { get { return UserIdInventory<TEntity>.Instance; } }

    public bool CanRemoveFromUserInventory() {
      return UserIdInventory<TEntity>.Instance.CanRemoveIdQuantity(this);
    }

    public void RemoveFromUserInventory() {
      UserIdInventory<TEntity>.Instance.RemoveIdQuantity(this);
    }

    public void AddToUserInventory() {
      UserIdInventory<TEntity>.Instance.AddIdQuantity(this);
    }

    public DTEntity Entity { get { return this.GetEntity(); } }
    public Type EntityType { get { return typeof(TEntity); } }

    public int Id { get { return this.id; } }
    public int Quantity { get { return this.quantity; } }
  }
}