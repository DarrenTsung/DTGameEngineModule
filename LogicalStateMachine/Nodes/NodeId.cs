using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  [Serializable]
  public class NodeId {
    // PRAGMA MARK - Static
    public static implicit operator NodeId(int value) {
      return new NodeId(value);
    }

    public static implicit operator int(NodeId id) {
      return id.intValue;
    }


    // PRAGMA MARK - Public Interface
    public int intValue;

    public NodeId(int intValue) {
      this.intValue = intValue;
    }

    public override string ToString() {
      return this.intValue.ToString();
    }
  }
}