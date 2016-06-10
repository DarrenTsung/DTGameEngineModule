using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class LootDropGroup : DTEntity {
    // PRAGMA MARK - Public Interface
    public IdComponent idComponent = new IdComponent();
#if UNITY_EDITOR
    public EditorDisplayComponent editorDisplayComponent = new EditorDisplayComponent();
#endif

    [Multiline]
    public string notes;
    public LootDropIdWeightPair[] dropList;

    public bool IsValid() {
      HashSet<int> validSet = new HashSet<int>();
      // HashSet.Add returns false if the item was not added (non-unique)
      bool allLootDropIdsUnique = this.dropList.All(pair => validSet.Add(pair.lootDropId));
      if (!allLootDropIdsUnique) {
        Debug.LogWarning("LootDropGroup.IsValid(): loot drop ids are not unique!");
        return false;
      }

      return true;
    }
  }

  [Serializable]
  public class LootDropIdWeightPair : IWeightedObject {
    [Id(typeof(LootDrop))]
    public int lootDropId;
    public int weight;

    // PRAGMA MARK - IWeightedObject Implementation
    public int Weight {
      get { return this.weight; }
    }
  }
}