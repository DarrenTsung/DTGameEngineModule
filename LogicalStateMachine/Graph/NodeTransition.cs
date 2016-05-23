using DT;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  [Serializable]
  public class NodeTransition {
    public NodeId[] targets = new NodeId[0];
    public Transition transition = new Transition();
  }
}