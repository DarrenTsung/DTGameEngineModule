using DT;

namespace DT.GameEngine {
  public enum ILootRewardType {
    ITEM_QUANTITY
  }

  public interface ILootReward {
    void Apply();
    ILootRewardType Type {
      get;
    }
  }
}