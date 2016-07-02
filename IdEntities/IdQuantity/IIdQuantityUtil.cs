using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class IIdQuantityUtil {
    public static IList<IIdQuantity> Combine(IList<IIdQuantity> idQuantities) {
      Dictionary<Type, CountMap<int>> typeIdCount = new Dictionary<Type, CountMap<int>>();

      foreach (IIdQuantity idQuantity in idQuantities) {
        CountMap<int> idQuantityCount = typeIdCount.GetAndCreateIfNotFound(idQuantity.EntityType);
        idQuantityCount.Increment(idQuantity.Id, amount: idQuantity.Quantity);
      }

      List<IIdQuantity> combinedIdQuantities = new List<IIdQuantity>();
      foreach (KeyValuePair<Type, CountMap<int>> pair in typeIdCount) {
        Type type = pair.Key;

        CountMap<int> idQuantityCount = pair.Value;
        foreach (KeyValuePair<int, int> p in idQuantityCount) {
          int id = p.Key;
          int quantity = p.Value;

          combinedIdQuantities.Add(IdQuantityFactory.Create(type, id, quantity));
        }
      }

      return combinedIdQuantities;
    }
  }
}