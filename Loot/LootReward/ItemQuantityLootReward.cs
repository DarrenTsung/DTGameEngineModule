using DT;

namespace DT.GameEngine {
  public class ItemQuantityLootReward : ILootReward {
    // PRAGMA MARK - Public Interface
    public ItemQuantityLootReward(ItemQuantity itemQuantity) {
      this._itemQuantity = itemQuantity;
    }

    // PRAGMA MARK - ILootReward Implementation
    public void Apply() {
      UserItemInventory.Instance.GainItemQuantity(this._itemQuantity);
    }

    public ILootRewardType Type {
      get {
        return ILootRewardType.ITEM_QUANTITY;
      }
    }


    // PRAGMA MARK - Internal
    private ItemQuantity _itemQuantity;
  }
}