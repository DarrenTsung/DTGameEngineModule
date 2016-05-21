using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kInspectorWindowWidth = 150.0f;
    private static readonly Vector2 kInspectorWindowPosition = new Vector2(10.0f, 20.0f);

    private const float kInspectorHideButtonHeight = 25.0f;
    private const float kInspectorViewHeight = 200.0f;

    // PRAGMA MARK - Internal
    private bool _inspectorCollapsed = false;
    private Vector2 _inspectorScrollPos;

    private void DrawInspectorWindow() {
      Node selectedNode = this.GetSelectedNode();
      if (selectedNode == null) {
        return;
      }

      NodeViewData selectedNodeViewData = this.GetViewDataForNode(selectedNode);

      float heightSoFar = 0.0f;

      Rect inspectorHideButtonRect = new Rect(kInspectorWindowPosition, new Vector2(kInspectorWindowWidth, kInspectorHideButtonHeight));
      EditorGUIUtility.AddCursorRect(inspectorHideButtonRect, MouseCursor.Link);
      if (GUI.Button(inspectorHideButtonRect, "")) {
        this._inspectorCollapsed = !this._inspectorCollapsed;
      }
      heightSoFar += kInspectorHideButtonHeight;

      if (!this._inspectorCollapsed) {
        Vector2 inspectorRectPosition = kInspectorWindowPosition + new Vector2(0.0f, heightSoFar);
        Rect inspectorViewRect = new Rect(inspectorRectPosition, new Vector2(kInspectorWindowWidth, kInspectorViewHeight));

        Rect inspectorRect = new Rect(inspectorRectPosition, new Vector2(kInspectorWindowWidth, kInspectorViewHeight));
				this._inspectorScrollPos = GUI.BeginScrollView(inspectorRect, this._inspectorScrollPos, inspectorViewRect);
        // Scroll View
  				GUILayout.BeginArea(inspectorRect, "", (GUIStyle)"InspectorWindow");
            this.DrawNodeInspector(selectedNode, selectedNodeViewData);
  				GUILayout.EndArea();
        // End Scroll View
				GUI.EndScrollView();
      }
    }

    private void DrawNodeInspector(Node node, NodeViewData nodeViewData) {
			nodeViewData.name = EditorGUILayout.TextField(nodeViewData.name);

      if (GUILayout.Button("Add INodeDelegate")) {
        GenericMenu nodeDelegateMenu = new GenericMenu();
        foreach (Type nodeDelegateType in INodeDelegateUtil.ImplementationTypes) {
          nodeDelegateMenu.AddItem(new GUIContent(nodeDelegateType.Name), false, this.AddNodeDelegateToNode, Tuple.Create(node, nodeDelegateType));
        }
        nodeDelegateMenu.ShowAsContext();
      }

      foreach (INodeDelegate nodeDelegate in node.GetNodeDelegates()) {
        Type nodeDelegateType = nodeDelegate.GetType();
        EditorGUILayout.LabelField(nodeDelegateType.Name);
      }
    }

    private void AddNodeDelegateToNode(object tupleAsObject) {
      Tuple<Node, Type> data = tupleAsObject as Tuple<Node, Type>;
      Node node = data.Item1;
      Type type = data.Item2;

      INodeDelegate nodeDelegate = Activator.CreateInstance(type) as INodeDelegate;
      if (nodeDelegate == null) {
        Debug.LogError("AddNodeDelegateToNode - Failed to cast created type as INodeDelgate!");
        return;
      }

      node.AddNodeDelegate(nodeDelegate);
      this.SetTargetDirty();
    }
  }
}