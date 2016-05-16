using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  [Serializable]
  public class NodeId : IComparable<NodeId> {
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


    // PRAGMA MARK - IComparable<NodeId> Implementation
    public int CompareTo(NodeId other) {
      return this.intValue.CompareTo(other.intValue);
    }
  }
}