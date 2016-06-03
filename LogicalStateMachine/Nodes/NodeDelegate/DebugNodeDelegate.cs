using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugNodeDelegate : INodeDelegate {
    public int publicField;

    public void HandleEnter() {
      Debug.Log("Enter");
    }

    public void HandleExit() {
      Debug.Log("Exit");
    }


    // PRAGMA MARK - Internal
    [SerializeField] private int serializedPrivateField = 3;
    private int nonSerializedPrivateField;
  }
}
