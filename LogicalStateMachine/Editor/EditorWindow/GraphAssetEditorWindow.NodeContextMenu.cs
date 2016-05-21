using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private void OpenNodeContextMenu(Event currentEvent, Node node) {
      GenericMenu nodeCreationMenu = new GenericMenu();
      nodeCreationMenu.AddItem(new GUIContent("Make Transition"), false, this.MakeTransition, node);
      nodeCreationMenu.AddSeparator("");
      if (this.TargetGraph.IsStartingNode(node)) {
        nodeCreationMenu.AddItem(new GUIContent("Unset Starting"), false, this.RemoveFromStartingNodes, node);
      } else {
        nodeCreationMenu.AddItem(new GUIContent("Set Starting"), false, this.AddToStartingNodes, node);
      }
      nodeCreationMenu.ShowAsContext();
    }

    private void MakeTransition(object nodeAsObject) {
      // TODO (darren): make transition logic here
      // consider how to make a transition to multiple nodes
    }

    private void AddToStartingNodes(object nodeAsObject) {
      Node node = nodeAsObject as Node;
      List<Node> startingNodes = this.TargetGraph.GetStartingNodes().ToList();
      startingNodes.Add(node);

      this.TargetGraph.SetStartingNodes(startingNodes);
      this.SetTargetDirty();
    }

    private void RemoveFromStartingNodes(object nodeAsObject) {
      Node node = nodeAsObject as Node;
      List<Node> startingNodes = this.TargetGraph.GetStartingNodes().ToList();
      bool successful = startingNodes.Remove(node);
      if (!successful) {
        Debug.LogError("RemoveFromStartingNodes - failed to remove node from starting nodes!");
        return;
      }

      this.TargetGraph.SetStartingNodes(startingNodes);
      this.SetTargetDirty();
    }
  }
}