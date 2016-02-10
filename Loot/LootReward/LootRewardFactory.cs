using DT;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  public static class LootRewardFactory {
    // PRAGMA MARK - Static
    public static ILootReward[] CreateLootRewardsFrom(IEnumerable<ItemQuantity> itemQuantityCollection) {
      List<ILootReward> lootRewards = new List<ILootReward>();

      foreach (ItemQuantity itemQuantity in itemQuantityCollection) {
        lootRewards.Add(new ItemQuantityLootReward(itemQuantity));
      }

      // TODO (darren): look into making sure IEnumerable is the most generic way to pass in a collection
      return lootRewards.ToArray();
    }
  }
}