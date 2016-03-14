using DT;
using UnityEngine;

namespace DT.GameEngine {
  public interface IIdListDisplayObject {
    Texture2D IconTexture {
      get;
    }

    string Title {
      get;
    }
  }
}