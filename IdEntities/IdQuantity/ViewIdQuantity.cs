using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class ViewIdQuantity<TEntity> : IViewIdQuantity where TEntity : DTEntity {
    public ViewIdQuantity(IdQuantity<TEntity> idQuantity) {
      this._idQuantity = idQuantity;
      UserIdInventory<TEntity>.Instance.OnInventoryUpdated += this.HandleUserInventoryUpdated;
    }

    ~ViewIdQuantity() {
      UserIdInventory<TEntity>.Instance.OnInventoryUpdated -= this.HandleUserInventoryUpdated;
    }


    // PRAGMA MARK - IViewIdQuantity Implementation
    public event Action OnUserInventoryUpdated = delegate {};

    public int Id {
      get { return this._idQuantity.id; }
    }

    public int Quantity {
      get { return this._idQuantity.quantity; }
    }

    public int UserQuantity {
      get {
        return UserIdInventory<TEntity>.Instance.GetCountOfId(this._idQuantity.id);
      }
    }

    public DTEntity Entity {
      get {
        DTEntity entity = IdList<TEntity>.Instance.LoadById(this._idQuantity.id);
        if (entity == null) {
          Debug.LogError("Failed to load entity for id: " + this._idQuantity.id);
        }

        return entity;
      }
    }


    // PRAGMA MARK - Internal
    private IdQuantity<TEntity> _idQuantity;

    private void HandleUserInventoryUpdated() {
      this.OnUserInventoryUpdated.Invoke();
    }
	}
}