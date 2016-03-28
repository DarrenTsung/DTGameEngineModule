using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DT.GameEngine {
  public static class DTEntityInitializer {
    // PRAGMA MARK - Static
    public static void Initialize<TEntity>(TEntity entity) where TEntity : DTEntity {
      DTEntityInitializer.Initialize(typeof(TEntity), entity);
    }

    public static void Initialize(Type entityType, DTEntity entity) {
      if (entity.Initialized) {
        return;
      }

      FieldInfo[] componentFields = DTEntityInitializer.GetComponentFields(entityType);
      foreach (FieldInfo componentField in componentFields) {
        object component = componentField.GetValue(entity);
        entity.AddComponent(componentField.FieldType, component);
      }

      entity.FinishInitializing();
    }


    // PRAGMA MARK - Static Internal
    private static Dictionary<Type, FieldInfo[]> _classComponentMap = new Dictionary<Type, FieldInfo[]>();

    private static FieldInfo[] GetComponentFields(Type entityType) {
      if (!DTEntityInitializer._classComponentMap.ContainsKey(entityType)) {
        FieldInfo[] fields = entityType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        DTEntityInitializer._classComponentMap[entityType] = (from field in fields where typeof(IDTComponent).IsAssignableFrom(field.FieldType) select field).ToArray();
      }

      return DTEntityInitializer._classComponentMap[entityType];
    }
  }
}