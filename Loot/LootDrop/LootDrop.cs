using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public abstract class LootDrop : DTEntity, IWeightedObject {
    // PRAGMA MARK - Public Interface
    public IdComponent idComponent = new IdComponent();
    public WeightComponent weightComponent = new WeightComponent();

#if UNITY_EDITOR
    public EditorDisplayComponent editorDisplayComponent = new EditorDisplayComponent();
#endif

    [Id(typeof(LootDropGroup))]
    public int[] rewardedLootDropGroupIds = new int[0];

    public ILootReward[] RewardedLootRewards {
      get {
        if (this._lootRewards == null) {
          this._lootRewards = this.CreateLootRewards();
        }
        return this._lootRewards;
      }
    }

    public bool IsValid() {
      HashSet<int> validSet = new HashSet<int>();
      // HashSet.Add returns false if the item was not added (non-unique)
      bool allLootDropGroupIdsUnique = this.rewardedLootDropGroupIds.All(text => validSet.Add(text));
      if (!allLootDropGroupIdsUnique) {
        Debug.LogWarning("LootDrop.IsValid(): rewarded loot drop group ids are not unique!");
        return false;
      }

      return true;
    }


    // PRAGMA MARK - IWeightedObject Implementation
    public int Weight {
      get { return this.weightComponent.weight; }
    }


    // PRAGMA MARK - Internal
    private ILootReward[] _lootRewards;

    protected abstract ILootReward[] CreateLootRewards();
  }
}