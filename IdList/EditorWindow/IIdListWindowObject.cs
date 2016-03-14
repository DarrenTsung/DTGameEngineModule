using DT;
using UnityEngine;

namespace DT.GameEngine {
  public interface IIdListWindowObject {
    Texture2D IconTexture {
      get;
    }

    string Title {
      get;
    }
  }
}