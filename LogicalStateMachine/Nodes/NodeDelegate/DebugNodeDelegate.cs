using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugNodeDelegate : INodeDelegate {
    public void HandleEnter() {
      Debug.Log("Enter");
    }

    public void HandleExit() {
      Debug.Log("Exit");
    }
  }
}
