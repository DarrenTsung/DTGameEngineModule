using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kTransitionLineWidth = 4.0f;
    private const float kTransitionTangentMultiplier = 0.75f;


    // PRAGMA MARK - Internal
    private void DrawTransitions() {
      foreach (Node node in this.TargetGraph.GetAllNodes()) {
        this.DrawTransitionsForNode(node);
      }
    }

    private void DrawTransitionsForNode(Node node) {
      NodeViewData nodeViewData = this.GetViewDataForNode(node);
      IList<NodeTransition> nodeTransitions = this.TargetGraph.GetOutgoingTransitionsForNode(node);
      foreach (NodeTransition nodeTransition in nodeTransitions) {
        TransitionViewData transitionViewData = nodeViewData.GetViewDataForTransition(nodeTransition.transition);

        IList<Node> targetNodes = nodeTransition.targets.Select(targetId => this.TargetGraph.LoadNodeById(targetId)).ToList();
        foreach (Node targetNode in targetNodes) {
          this.DrawTransitionFromNodeToNode(transitionViewData, node, targetNode);
        }
      }
    }

    private void DrawTransitionFromNodeToNode(TransitionViewData transitionViewData, Node node, Node targetNode) {
      NodeViewData nodeViewData = this.GetViewDataForNode(node);
      NodeViewData targetNodeViewData = this.GetViewDataForNode(targetNode);

      this.DrawTransitionFromPointToPoint(transitionViewData,
                                          nodeViewData.position + this._panner.Position,
                                          targetNodeViewData.position + this._panner.Position);
    }

    private void DrawTransitionFromPointToPoint(TransitionViewData transitionViewData, Vector2 point, Vector2 targetPoint) {
      Vector2 offset = targetPoint - point;
      // ex. A ---> B    ==    Direction.RIGHT
      Direction offsetDirection = DirectionUtil.ConvertVector2(offset);

      // ex. (Direction.RIGHT).Vector2Value()   ==   Vector2(1.0f, 0.0f)
      Vector2 nodeTangent = Vector2.Scale(offsetDirection.Vector2Value(), Vector2Util.Abs(offset) * kTransitionTangentMultiplier);
      Vector2 targetNodeTangent = -nodeTangent;

      Handles.DrawBezier(point,
                         targetPoint,
                         point + nodeTangent,
                         targetPoint + targetNodeTangent,
                         Color.white,
                         null,
                         kTransitionLineWidth);
    }
  }
}