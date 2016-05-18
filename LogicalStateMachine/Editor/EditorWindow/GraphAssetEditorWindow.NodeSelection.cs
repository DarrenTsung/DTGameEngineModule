using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private Node _selectedNode;

    private Node FindNodeContainingPoint(Vector2 canvasPoint) {
      foreach (Node node in this.TargetGraph.GetAllNodes().Reverse()) {
        Rect nodeRect = this.GetNodeRect(node);
        if (nodeRect.Contains(canvasPoint)) {
          return node;
        }
      }

      return null;
    }

    private Node GetSelectedNode() {
      return this._selectedNode;
    }

    private void SelectNode(Node node) {
      this._selectedNode = node;
    }

    private bool IsNodeSelected(Node node) {
      return this._selectedNode == node;
    }

    private void DeselectCurrentNode() {
      this._selectedNode = null;
    }
  }
}