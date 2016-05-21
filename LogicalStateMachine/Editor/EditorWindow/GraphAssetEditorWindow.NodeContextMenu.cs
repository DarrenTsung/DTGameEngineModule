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
      nodeCreationMenu.ShowAsContext();
    }

    private void MakeTransition(object nodeAsObject) {
      // TODO (darren): make transition logic here
      // consider how to make a transition to multiple nodes
    }
  }
}