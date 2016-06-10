using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public class LootManager {
    // PRAGMA MARK - Static
    public static ILootReward[] SelectRewardsForLootDropGroupId(int lootDropGroupId) {
      List<ILootReward> rewards = new List<ILootReward>();

      LootDropGroup dropGroup = IdList<LootDropGroup>.Instance.LoadById(lootDropGroupId);
      int selectedLootDropId = WeightedSelectionUtil.SelectWeightedObject(dropGroup.dropList).lootDropId;

      IIdList<LootDrop> lootDropList = ListFactory<LootDrop>.Instance.GetList();
      LootDrop selectedLootDrop = lootDropList.LoadById(selectedLootDropId);

      if (selectedLootDrop == null) {
        Debug.LogError("SelectRewardsForLootDropGroupId: failed to select lootDrop with group id: " + lootDropGroupId);
        return null;
      }

      foreach (int rewardedLootDropGroupId in selectedLootDrop.rewardedLootDropGroupIds) {
        rewards.AddRange(LootManager.SelectRewardsForLootDropGroupId(rewardedLootDropGroupId));
      }

      rewards.AddRange(selectedLootDrop.RewardedLootRewards);

      return rewards.ToArray();
    }
  }
}