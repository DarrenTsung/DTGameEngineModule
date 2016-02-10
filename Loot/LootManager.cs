using DT;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class LootManager : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public ILootReward[] SelectRewardsForLootDropGroupId(int lootDropGroupId) {
      List<ILootReward> rewards = new List<ILootReward>();

      LootDrop[] lootDrops = Toolbox.GetInstance<LootDropList>().GetLootDropsForGroupId(lootDropGroupId);
      LootDrop selectedLootDrop = WeightedSelectionUtil.SelectWeightedObject(lootDrops);

      foreach (int rewardedLootDropGroupId in selectedLootDrop.RewardedLootDropGroupIds) {
        rewards.AddRange(this.SelectRewardsForLootDropGroupId(rewardedLootDropGroupId));
      }

      rewards.AddRange(selectedLootDrop.RewardedLootRewards);

      return rewards.ToArray();
    }


    // PRAGMA MARK - Internal

  }
}