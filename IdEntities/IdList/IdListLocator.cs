using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
﻿using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class IdListLocator {
    // PRAGMA MARK - Static
    public static IIdList GetListForType(Type entityType) {
      if (!DTEntityUtil.EntitySubclasses.Contains(entityType)) {
        Debug.LogError("IdListLocator - ListForType passed with invalid entity type: " + entityType.Name + "!");
        return null;
      }

      PropertyInfo instanceProperty = IdListLocator.GetIdListInstanceMethodForType(entityType);
      return (IIdList)instanceProperty.GetValue(obj: null);
    }


    // PRAGMA MARK - Static Internal
    private static Dictionary<Type, PropertyInfo> _cachedIdListInstanceMap = new Dictionary<Type, PropertyInfo>();
    private static PropertyInfo GetIdListInstanceMethodForType(Type type) {
      if (IdListLocator._cachedIdListInstanceMap.DoesntContainKey(type)) {
        Type idListType = typeof(IdList<>).MakeGenericType(type);
        PropertyInfo instanceProperty = idListType.GetProperty("Instance", idListType);
        if (instanceProperty == null) {
          Debug.LogError("GetIdListInstanceMethodForType - Failed to find instance method for type: " + type.Name + "!");
          return null;
        }

        IdListLocator._cachedIdListInstanceMap[type] = instanceProperty;
      }

      return IdListLocator._cachedIdListInstanceMap[type];
    }
  }
}