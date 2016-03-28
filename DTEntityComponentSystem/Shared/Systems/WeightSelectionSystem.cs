using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public static class WeightedSelectionSystem {
    // PRAGMA MARK - Static
    public static TEntity SelectWeightedObject<TEntity>(IEnumerable<TEntity> collection) where TEntity : DTEntity {
      return WeightedSelectionUtil.SelectWeightedObject(collection, WeightedSelectionSystem.GetWeightForEntity);
    }

    private static int GetWeightForEntity(DTEntity entity) {
      if (entity == null) {
        return 0;
      }

      WeightComponent weightComponent = entity.GetComponent<WeightComponent>();
      if (weightComponent == null) {
        return 0;
      }

      return weightComponent.weight;
    }
  }
}