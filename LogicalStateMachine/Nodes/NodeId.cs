using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  [Serializable]
  public struct NodeId : IComparable<NodeId> {
    // PRAGMA MARK - Static
    public static implicit operator NodeId(int value) {
      return new NodeId(value);
    }

    public static implicit operator int(NodeId id) {
      return id.intValue;
    }

    public static bool operator ==(NodeId a, NodeId b) {
      // If one is null, but not both, return false.
      if (((object)a == null) || ((object)b == null)) {
        return false;
      }

      return a.intValue == b.intValue;
    }

    public static bool operator !=(NodeId a, NodeId b) {
      return !(a == b);
    }


    // PRAGMA MARK - Public Interface
    public int intValue;

    public NodeId(int intValue) {
      this.intValue = intValue;
    }

    public override string ToString() {
      return this.intValue.ToString();
    }

    public override bool Equals(System.Object obj) {
      if (obj == null) {
        return false;
      }

      NodeId otherNodeId;
      try {
        otherNodeId = (NodeId)obj;
      } catch (InvalidCastException) {
        return false;
      }

      return otherNodeId.intValue == this.intValue;
    }

    public bool Equals(NodeId otherNodeId) {
      return otherNodeId.intValue == this.intValue;
    }

    public override int GetHashCode() {
      return base.GetHashCode() ^ this.intValue;
    }


    // PRAGMA MARK - IComparable<NodeId> Implementation
    public int CompareTo(NodeId other) {
      return this.intValue.CompareTo(other.intValue);
    }
  }
}