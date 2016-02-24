using DT;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class LootManager : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public ILootReward[] SelectRewardsForLootDropGroupId(int lootDropGroupId) {
      List<ILootReward> rewards = new List<ILootReward>();

      IEnumerable<LootDrop> lootDrops = Toolbox.GetInstance<LootDropGroupList>().GetLootDropsForId(lootDropGroupId);
      LootDrop selectedLootDrop = WeightedSelectionUtil.SelectWeightedObject(lootDrops);

      if (selectedLootDrop == null) {
        Debug.LogError("SelectRewardsForLootDropGroupId: failed to select lootDrop with group id: " + lootDropGroupId);
        return null;
      }

      foreach (int rewardedLootDropGroupId in selectedLootDrop.rewardedLootDropGroupIds) {
        rewards.AddRange(this.SelectRewardsForLootDropGroupId(rewardedLootDropGroupId));
      }

      rewards.AddRange(selectedLootDrop.RewardedLootRewards);

      return rewards.ToArray();
    }


    // PRAGMA MARK - Internal

  }
}