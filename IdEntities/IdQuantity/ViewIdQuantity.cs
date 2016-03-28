using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class ViewIdQuantity<TEntity> : IViewIdQuantity where TEntity : DTEntity {
    public ViewIdQuantity(IdQuantity<TEntity> idQuantity) {
      this._idQuantity = idQuantity;
    }


    // PRAGMA MARK - IViewIdQuantity Implementation
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
        return IdList<TEntity>.Instance.LoadById(this._idQuantity.id);
      }
    }


    // PRAGMA MARK - Internal
    IdQuantity<TEntity> _idQuantity;
	}
}