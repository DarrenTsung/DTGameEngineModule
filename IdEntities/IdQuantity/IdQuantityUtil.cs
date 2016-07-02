using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class IdQuantityUtil {
    public static IList<IdQuantity<TEntity>> Combine<TEntity>(IList<IdQuantity<TEntity>> idQuantities) where TEntity : DTEntity {
      CountMap<int> idQuantityCount = new CountMap<int>();

      foreach (IdQuantity<TEntity> idQuantity in idQuantities) {
        idQuantityCount.Increment(idQuantity.Id, amount: idQuantity.Quantity);
      }

      List<IdQuantity<TEntity>> combinedIdQuantities = new List<IdQuantity<TEntity>>();
      foreach (KeyValuePair<int, int> p in idQuantityCount) {
        int id = p.Key;
        int quantity = p.Value;

        combinedIdQuantities.Add(new IdQuantity<TEntity>(id, quantity));
      }

      return combinedIdQuantities;
    }
  }
}