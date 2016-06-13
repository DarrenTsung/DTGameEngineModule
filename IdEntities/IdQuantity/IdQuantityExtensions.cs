using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class IdQuantityExtensions {
    public static TEntity GetEntity<TEntity>(this IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      return IdList<TEntity>.Instance.LoadById(idQuantity.id);
    }

    public static bool CanRemoveFromUserInventory<TEntity>(this IEnumerable<IdQuantity<TEntity>> idQuantityList) where TEntity : DTEntity {
      return UserIdInventory<TEntity>.Instance.CanRemoveIdQuantityList(idQuantityList);
    }

    public static bool CanRemoveFromUserInventory<TEntity>(this IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      return UserIdInventory<TEntity>.Instance.CanRemoveIdQuantity(idQuantity);
    }

    public static void RemoveFromUserInventory<TEntity>(this IEnumerable<IdQuantity<TEntity>> idQuantityList) where TEntity : DTEntity {
      UserIdInventory<TEntity>.Instance.RemoveIdQuantityList(idQuantityList);
    }

    public static void RemoveFromUserInventory<TEntity>(this IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      UserIdInventory<TEntity>.Instance.RemoveIdQuantity(idQuantity);
    }

    public static void AddToUserInventory<TEntity>(this IEnumerable<IdQuantity<TEntity>> idQuantityList) where TEntity : DTEntity {
      UserIdInventory<TEntity>.Instance.AddIdQuantityList(idQuantityList);
    }

    public static void AddToUserInventory<TEntity>(this IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      UserIdInventory<TEntity>.Instance.AddIdQuantity(idQuantity);
    }
	}
}