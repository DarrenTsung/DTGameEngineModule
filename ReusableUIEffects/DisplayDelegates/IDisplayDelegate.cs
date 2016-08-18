using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public interface IDisplayDelegate {
    void Display(bool instant = false);
  }
}