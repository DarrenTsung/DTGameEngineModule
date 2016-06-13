using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public static class UserIdInventoryExtensions {
    public static void AddIdQuantityList<TEntity>(this UserIdInventory<TEntity> inventory, IEnumerable<IdQuantity<TEntity>> idQuantityList) where TEntity : DTEntity {
      foreach (IdQuantity<TEntity> idQuantity in idQuantityList) {
        inventory.AddIdQuantity(idQuantity);
      }
    }
  }
}