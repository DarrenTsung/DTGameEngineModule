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
    private const float kInspectorWindowWidth = 250.0f;
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

      // Node Delegate Button + Fields
      EditorGUILayout.BeginVertical((GUIStyle)"InspectorBigBox");
        EditorGUILayout.LabelField("Node Delegates:");
        foreach (INodeDelegate nodeDelegate in node.GetNodeDelegates()) {
          Type nodeDelegateType = nodeDelegate.GetType();
          EditorGUILayout.BeginVertical((GUIStyle)"InspectorBox");
            EditorGUILayout.LabelField(nodeDelegateType.Name);
          EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("", (GUIStyle)"AddButton", GUILayout.Width(20.0f), GUILayout.Height(20.0f))) {
          GenericMenu nodeDelegateMenu = new GenericMenu();
          foreach (Type nodeDelegateType in INodeDelegateUtil.ImplementationTypes) {
            nodeDelegateMenu.AddItem(new GUIContent(nodeDelegateType.Name), false, this.AddNodeDelegateToNode, Tuple.Create(node, nodeDelegateType));
          }
          nodeDelegateMenu.ShowAsContext();
        }
      EditorGUILayout.EndVertical();

      EditorGUILayout.Space();

      // Node Transitions
      EditorGUILayout.BeginVertical((GUIStyle)"InspectorBigBox");
        EditorGUILayout.LabelField("Transitions:");
        IList<NodeTransition> nodeTransitions = this.TargetGraph.GetOutgoingTransitionsForNode(node);
        foreach (NodeTransition nodeTransition in nodeTransitions) {
          GUIStyle transitionStyle = this.IsNodeTransitionSelected(nodeTransition) ? (GUIStyle)"SelectedInspectorBox" : (GUIStyle)"InspectorBox";
          Rect transitionRect = EditorGUILayout.BeginVertical(transitionStyle, GUILayout.MinHeight(30.0f));

          Rect selectionRect = transitionRect;
          selectionRect.size = new Vector2(selectionRect.size.x - 25.0f, selectionRect.size.y);
          if (GUI.Button(selectionRect, "", (GUIStyle)"InvisibleButton")) {
            this.SelectNodeTransition(nodeTransition);
          }

            string targetText = "";
            targetText += (nodeTransition.targets.Length > 1) ? "Targets: " : "Target: ";
            targetText += StringUtil.Join(", ", nodeTransition.targets);

            EditorGUILayout.LabelField(targetText);

            Rect editButtonRect = new Rect(new Vector2(transitionRect.x + transitionRect.width - 25.0f,
                                                       transitionRect.y + 5.0f),
                                           new Vector2(20.0f, 20.0f));
            if (GUI.Button(editButtonRect, "", (GUIStyle)"EditButton")) {
              this.StartEditingNodeTransition(node, nodeTransition);
            }
          EditorGUILayout.EndVertical();
          EditorGUILayout.Space();
        }
      EditorGUILayout.EndVertical();
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