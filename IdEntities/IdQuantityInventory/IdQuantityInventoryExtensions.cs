using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public static class IdQuantityInventoryExtensions {
    public static void AddIdQuantityList<TEntity>(this IdQuantityInventory<TEntity> inventory, IList<IdQuantity<TEntity>> idQuantities) where TEntity : DTEntity {
      foreach (IdQuantity<TEntity> idQuantity in idQuantities) {
        inventory.AddIdQuantity(idQuantity);
      }
    }

    public static bool CanRemoveIdQuantityList<TEntity>(this IdQuantityInventory<TEntity> inventory, IList<IdQuantity<TEntity>> idQuantities) where TEntity : DTEntity {
      IList<IdQuantity<TEntity>> combinedIdQuantities = IdQuantityUtil.Combine(idQuantities);
      foreach (IdQuantity<TEntity> idQuantity in combinedIdQuantities) {
        if (!inventory.CanRemoveIdQuantity(idQuantity)) {
          return false;
        }
      }

      return true;
    }

    public static void RemoveIdQuantityList<TEntity>(this IdQuantityInventory<TEntity> inventory, IList<IdQuantity<TEntity>> idQuantities) where TEntity : DTEntity {
      foreach (IdQuantity<TEntity> idQuantity in idQuantities) {
        inventory.RemoveIdQuantity(idQuantity);
      }
    }
  }
}