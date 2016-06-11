using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  /// <summary>
  /// This class is very similar to IdQuantity except that it holds
  /// a DTEntity so that listeners who don't care what type of entity
  /// it is can filter based on the appropriate components
  /// </summary>
  public struct EntityQuantity {
    // PRAGMA MARK - Static Interface
    public static EntityQuantity From<TEntity>(IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      TEntity entity = idQuantity.GetEntity();
      return new EntityQuantity(entity, idQuantity.quantity);
    }


    // PRAGMA MARK - Public Interface
    public DTEntity entity;
    public int quantity;

    private EntityQuantity(DTEntity entity, int quantity = 1) {
      this.entity = entity;
      this.quantity = quantity;
    }
	}
}