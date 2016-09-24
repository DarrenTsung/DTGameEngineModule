using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public static class IUserIdInventoryExtensions {
    public static void SetIdQuantity(this IUserIdInventory inventory, int id, int setQuantity) {
      int currentQuantity = inventory.GetCountOfId(id);

      int diff = setQuantity - currentQuantity;
      if (diff > 0) {
        inventory.AddIdQuantity(id, diff);
      } else {
        inventory.RemoveIdQuantity(id, -diff);
      }
    }
  }
}