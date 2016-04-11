using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public partial class UserIdInventory<TEntity> : IEnumerable<IdQuantity<TEntity>> where TEntity : DTEntity {
    // PRAGMA MARK - IEnumerable<IdQuantity<TEntity>> Implementation
    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    public IEnumerator<IdQuantity<TEntity>> GetEnumerator() {
      return this._idQuantityInventory.GetEnumerator();
    }
  }
}