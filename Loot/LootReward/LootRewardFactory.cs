using DT;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  public static class LootRewardFactory {
    // PRAGMA MARK - Static
    public static void CreateLootRewardsFrom<TEntity>(IEnumerable<IdQuantity<TEntity>> idQuantityCollection, List<ILootReward> lootRewards) where TEntity : DTEntity {
      foreach (IdQuantity<TEntity> idQuantity in idQuantityCollection) {
        lootRewards.Add(new IdQuantityLootReward<TEntity>(idQuantity));
      }
    }
  }
}