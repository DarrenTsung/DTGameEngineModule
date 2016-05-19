using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class NodeViewData {
    public NodeId nodeId;
    public string name;
    public Vector2 position;

    public NodeViewData(Node node) {
      this.nodeId = node.Id;
      this.name = string.Format("Node {0}", node.Id);
      this.position = Vector2.zero;
    }
  }
}