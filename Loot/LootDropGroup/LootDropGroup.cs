using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class LootDropGroup : IIdObject {
    // PRAGMA MARK - Public Interface
    public int lootDropGroupId;
    public string notes;
    [LootDropId]
    public int[] lootDropIds;

    public bool IsValid() {
      HashSet<int> validSet = new HashSet<int>();
      // HashSet.Add returns false if the item was not added (non-unique)
      bool allLootDropIdsUnique = this.lootDropIds.All(id => validSet.Add(id));
      if (!allLootDropIdsUnique) {
        Debug.LogWarning("LootDropGroup.IsValid(): loot drop ids are not unique!");
        return false;
      }

      return true;
    }


    // PRAGMA MARK - IIdObject Implementation
    public int Id {
      get { return this.lootDropGroupId; }
    }
  }
}