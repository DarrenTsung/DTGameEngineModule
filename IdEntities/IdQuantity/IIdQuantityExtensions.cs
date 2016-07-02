using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class IIdQuantityExtensions {
    public static int UserQuantity(this IIdQuantity a) {
      return a.UserInventory.GetCountOfId(a.Id);
    }

    public static bool CanRemoveFromUserInventory(this IList<IIdQuantity> idQuantities) {
      IList<IIdQuantity> combinedIdQuantities = IIdQuantityUtil.Combine(idQuantities);
      foreach (IIdQuantity idQuantity in combinedIdQuantities) {
        if (!idQuantity.CanRemoveFromUserInventory()) {
          return false;
        }
      }

      return true;
    }

    public static void RemoveFromUserInventory(this IList<IIdQuantity> idQuantities) {
      foreach (IIdQuantity idQuantity in idQuantities) {
        idQuantity.RemoveFromUserInventory();
      }
    }

    public static void AddToUserInventory(this IList<IIdQuantity> idQuantities) {
      foreach (IIdQuantity idQuantity in idQuantities) {
        idQuantity.AddToUserInventory();
      }
    }
  }
}