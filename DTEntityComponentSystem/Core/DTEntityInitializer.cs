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
      if (entity.Initialized) {
        Debug.LogWarning("Initialize - called on an entity that is already initialized!");
        return;
      }

      FieldInfo[] componentFields = DTEntityInitializer.GetComponentFields<TEntity>();
      foreach (FieldInfo componentField in componentFields) {
        object component = componentField.GetValue(entity);
        entity.AddComponent(componentField.FieldType, component);
      }

      entity.FinishInitializing();
    }


    // PRAGMA MARK - Static Internal
    private static Dictionary<Type, FieldInfo[]> _classComponentMap = new Dictionary<Type, FieldInfo[]>();

    private static FieldInfo[] GetComponentFields<TEntity>() {
      Type entityType = typeof(TEntity);
      if (!DTEntityInitializer._classComponentMap.ContainsKey(entityType)) {
        FieldInfo[] fields = entityType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        DTEntityInitializer._classComponentMap[entityType] = (from field in fields where typeof(IDTComponent).IsAssignableFrom(field.FieldType) select field).ToArray();
      }

      return DTEntityInitializer._classComponentMap[entityType];
    }
  }
}