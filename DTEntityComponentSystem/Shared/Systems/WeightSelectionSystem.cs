using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public static class WeightedSelectionSystem {
    public static T SelectWeightedObject<T>(IEnumerable<T> collection) where T : DTEntity {
      return null;
      // WeightedEntityWrapper chosenWrapper = WeightedSelectionUtil.SelectWeightedObject((new WeightedEntityWrapper(entity) for entity in collection));
      //
      // if (chosenWrapper == null) {
      //   return null;
      // }
      //
      // return chosenWrapper.wrappedEntity;
    }

    // NOTE (darren): we can pool these if they become a performance issue
    // for the moment, it seems like unnecessary work
    private class WeightedEntityWrapper : IWeightedObject {
      public DTEntity wrappedEntity;
      private WeightComponent _weightComponent;

      public WeightedEntityWrapper(DTEntity entity) {
        this.wrappedEntity = entity;
        this._weightComponent = entity.GetComponent<WeightComponent>();
        if (this._weightComponent == null) {
          Debug.LogError("WeightedEntityWrapper - failed to get weight component from entity!");
        }
      }

      // PRAGMA MARK - IWeightedObject Implementation
      public int Weight {
        get {
          return this._weightComponent == null ? 0 : this._weightComponent.weight;
        }
      }
    }
  }
}