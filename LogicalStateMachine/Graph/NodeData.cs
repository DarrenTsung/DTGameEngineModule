using DT;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  [Serializable]
  public class NodeData {
    public Node node;
    public List<NodeTransition> outgoingTransitions;

    public NodeData(Node node) {
      this.node = node;
      this.outgoingTransitions = new List<NodeTransition>();
    }
  }

  [Serializable]
  public class NodeTransition {
    public NodeId target;
    public Transition transition;
  }
}