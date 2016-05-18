using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private Vector2 _startDragCanvasPosition;
    private Vector2 _startDragNodePosition;
    private bool _dragging = false;

    private Node _draggedNode;
    private NodeViewData _draggedNodeViewData;

    private void StartDragging(Node node, Vector2 startCanvasPosition) {
      if (this._dragging) {
        Debug.LogWarning("StartDragging - already dragging!");
        return;
      }

      if (node == null) {
        Debug.LogWarning("StartDragging - node is null!");
      }

      this._draggedNode = node;
      this._draggedNodeViewData = this.TargetGraphViewData.LoadViewDataForNode(this._draggedNode);

      this._dragging = true;
      this._startDragCanvasPosition = startCanvasPosition;
      this._startDragNodePosition = this._draggedNodeViewData.position;
    }

    private void UpdateDragging(Vector2 updatedCanvasPosition) {
      if (!this._dragging) {
        return;
      }

      Vector2 dragOffset = updatedCanvasPosition - this._startDragCanvasPosition;
      this._draggedNodeViewData.position = this._startDragNodePosition + dragOffset;
    }

    private void StopDragging() {
      if (!this._dragging) {
        return;
      }

      this._draggedNodeViewData.position = this.SnapToGrid(this._draggedNodeViewData.position);
      this.SetTargetDirty();

      this._dragging = false;

      this._draggedNode = null;
      this._draggedNodeViewData = null;
    }
  }
}