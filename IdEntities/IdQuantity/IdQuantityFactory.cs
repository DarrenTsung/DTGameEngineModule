using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class IdQuantityFactory {
    private static Dictionary<Type, MethodInfo> _createMethodMap = new Dictionary<Type, MethodInfo>();

    private static MethodInfo GetCreateMethodForType(Type type) {
      if (!IdQuantityFactory._createMethodMap.ContainsKey(type)) {
        Type utilType = typeof(IdQuantityFactory.Util<>).MakeGenericType(type);
        IdQuantityFactory._createMethodMap[type] = utilType.GetMethod("Create", BindingFlags.NonPublic | BindingFlags.Static);
      }

      return IdQuantityFactory._createMethodMap[type];
    }

    public static IIdQuantity Create(Type type, int id, int quantity = 1) {
      MethodInfo method = IdQuantityFactory.GetCreateMethodForType(type);
      return (IIdQuantity)method.Invoke(null, new object[] { id, quantity });
    }

    private static class Util<TEntity> where TEntity : DTEntity {
      private static IdQuantity<TEntity> Create(int id, int quantity = 1) {
        return new IdQuantity<TEntity>(id, quantity);
      }
    }
	}
}