using DT;

namespace DT.GameEngine {
  public enum ILootRewardType {
    ID_QUANTITY
  }

  public interface ILootReward {
    void Apply();
    ILootRewardType Type {
      get;
    }
  }
}