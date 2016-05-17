using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private static readonly Vector2 kNodeSize = new Vector2(10 * kGridLineSpacing, 5 * kGridLineSpacing);
    private static readonly Color kNodeColor = ColorUtil.HexStringToColor("#909090");

    private static GUIStyle _kNodeStyle;
    private static GUIStyle kNodeStyle {
      get {
        if (_kNodeStyle == null) {
          _kNodeStyle = GUI.skin.box;
          _kNodeStyle.normal.background = Texture2DUtil.CreateTextureWithColor(kNodeColor);
        }
        return _kNodeStyle;
      }
    }


    // PRAGMA MARK - Internal
    private void DrawNodes(Rect canvasRect, Vector2 currentPan) {
      foreach (Node node in this.TargetGraph.GetAllNodes()) {
        NodeViewData viewData = this.TargetGraphViewData.LoadViewDataForNode(node);
        Rect nodeRect = new Rect(viewData.position, kNodeSize);

        GUI.Box(nodeRect, "Node " + node.Id, kNodeStyle);
      }
    }
  }
}