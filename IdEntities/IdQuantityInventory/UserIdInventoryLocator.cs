using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
﻿using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class UserIdInventoryLocator {
    // PRAGMA MARK - Static
    public static IUserIdInventory GetInventoryForType(Type entityType) {
      if (!DTEntityUtil.EntitySubclasses.Contains(entityType)) {
        Debug.LogError("UserIdInventoryLocator - GetInventoryForType passed with invalid entity type: " + entityType.Name + "!");
        return null;
      }

      PropertyInfo instanceProperty = UserIdInventoryLocator.GetUserIdInventoryInstanceMethodForType(entityType);
      return (IUserIdInventory)instanceProperty.GetValue(obj: null);
    }


    // PRAGMA MARK - Static Internal
    private static Dictionary<Type, PropertyInfo> _cachedUserIdInventoryInstanceMap = new Dictionary<Type, PropertyInfo>();
    private static PropertyInfo GetUserIdInventoryInstanceMethodForType(Type type) {
      if (UserIdInventoryLocator._cachedUserIdInventoryInstanceMap.DoesntContainKey(type)) {
        Type userInventoryType = typeof(UserIdInventory<>).MakeGenericType(type);
        PropertyInfo instanceProperty = userInventoryType.GetProperty("Instance", userInventoryType);
        if (instanceProperty == null) {
          Debug.LogError("GetUserIdInventoryInstanceMethodForType - Failed to find instance method for type: " + type.Name + "!");
          return null;
        }

        UserIdInventoryLocator._cachedUserIdInventoryInstanceMap[type] = instanceProperty;
      }

      return UserIdInventoryLocator._cachedUserIdInventoryInstanceMap[type];
    }
  }
}