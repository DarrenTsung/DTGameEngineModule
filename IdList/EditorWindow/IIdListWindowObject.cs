using DT;
using UnityEngine;

namespace DT.GameEngine {
  public interface IIdListWindowObject {
    Sprite DisplaySprite {
      get;
    }

    string DisplayName {
      get;
    }
  }
}