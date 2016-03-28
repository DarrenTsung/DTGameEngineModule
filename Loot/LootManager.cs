using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public class LootManager : MonoBehaviour {
    // PRAGMA MARK - Static
    private static List<LootDrop> GetLootDropsForId(int lootDropGroupId) {
      LootDropGroup dropGroup = IdList<LootDropGroup>.Instance.LoadById(lootDropGroupId);

      IIdList<LootDrop> lootDropList = ListFactory<LootDrop>.Instance.GetList();
      return (from lootDropId in dropGroup.lootDropIds select lootDropList.LoadById(lootDropId)).ToList();
    }


    // PRAGMA MARK - Public Interface
    public ILootReward[] SelectRewardsForLootDropGroupId(int lootDropGroupId) {
      List<ILootReward> rewards = new List<ILootReward>();

      List<LootDrop> lootDrops = LootManager.GetLootDropsForId(lootDropGroupId);
      LootDrop selectedLootDrop = WeightedSelectionSystem.SelectWeightedObject(lootDrops);

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
  }
}