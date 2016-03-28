using DT;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  public static class LootRewardFactory {
    // PRAGMA MARK - Static
    public static ILootReward[] CreateLootRewardsFrom<TEntity>(IEnumerable<IdQuantity<TEntity>> idQuantityCollection) where TEntity : DTEntity {
      List<ILootReward> lootRewards = new List<ILootReward>();

      foreach (IdQuantity<TEntity> idQuantity in idQuantityCollection) {
        lootRewards.Add(new IdQuantityLootReward<TEntity>(idQuantity));
      }

      return lootRewards.ToArray();
    }
  }
}