using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class LootDrop : IIdObject, IWeightedObject {
    // PRAGMA MARK - Public Interface
    public int GroupId {
      get { return this._lootDropGroupId; }
    }

    public int[] RewardedLootDropGroupIds {
      get { return this._rewardedLootDropGroupIds; }
    }

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
      bool allLootDropGroupIdsUnique = this._rewardedLootDropGroupIds.All(text => validSet.Add(text));
      if (!allLootDropGroupIdsUnique) {
        Debug.LogWarning("LootDrop::IsValid - loot drop ids are not unique!");
        return false;
      }

      return true;
    }


    // PRAGMA MARK - IIdObject Implementation
    public int Id {
      get { return this._lootDropId; }
    }


    // PRAGMA MARK - IWeightedObject Implementation
    public int Weight {
      get { return this._weight; }
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    private int _lootDropId;
    [SerializeField]
    private int _lootDropGroupId;
    [SerializeField]
    private int[] _rewardedLootDropGroupIds = new int[0];
    [SerializeField]
    private ItemQuantity[] _rewardedItemQuantities = new ItemQuantity[0];
    [SerializeField]
    private int _weight;

    private ILootReward[] _lootRewards;

    private void CreateLootRewards() {
      List<ILootReward> lootRewards = new List<ILootReward>();

      lootRewards.AddRange(LootRewardFactory.CreateLootRewardsFrom(this._rewardedItemQuantities));

      this._lootRewards = lootRewards.ToArray();
    }
  }
}