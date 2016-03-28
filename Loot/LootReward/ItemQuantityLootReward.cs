using DT;

namespace DT.GameEngine {
  public class IdQuantityLootReward<TEntity> : ILootReward where TEntity : DTEntity {
    // PRAGMA MARK - Public Interface
    public IdQuantityLootReward(IdQuantity<TEntity> idQuantity) {
      this.idQuantity = idQuantity;
    }

    // PRAGMA MARK - ILootReward Implementation
    public void Apply() {
      UserIdInventory<TEntity>.Instance.AddIdQuantity(this.idQuantity);
    }

    public ILootRewardType Type {
      get {
        return ILootRewardType.ID_QUANTITY;
      }
    }

    public IdQuantity<TEntity> idQuantity;
  }
}