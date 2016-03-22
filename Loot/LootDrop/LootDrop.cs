using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class LootDrop : DTEntity, IWeightedObject {
    // PRAGMA MARK - Public Interface
    public IdComponent idComponent = new IdComponent();
#if UNITY_EDITOR
    public EditorDisplayComponent editorDisplayComponent = new EditorDisplayComponent();
#endif

    public string notes;
    public int weight;
    public int[] rewardedLootDropGroupIds = new int[0];
    // TODO (darren): remake this with a hook or provider to get rewards?
    // Need to figure out how we can reward App specific stuff
    public ItemQuantity[] rewardedItemQuantities = new ItemQuantity[0];

    public ILootReward[] RewardedLootRewards {
      get {
        if (this._lootRewards == null) {
          this.CreateLootRewards();
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
      get { return this.weight; }
    }


    // PRAGMA MARK - Internal
    private ILootReward[] _lootRewards;

    private void CreateLootRewards() {
      List<ILootReward> lootRewards = new List<ILootReward>();

      // lootRewards.AddRange(LootRewardFactory.CreateLootRewardsFrom(this.rewardedItemQuantities));

      this._lootRewards = lootRewards.ToArray();
    }
  }
}