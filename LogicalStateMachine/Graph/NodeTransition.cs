using DT;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  [Serializable]
  public class NodeTransition {
    public NodeId target;
    public Transition transition;
  }
}