using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Internal
    private void CheckEvents(Event currentEvent) {
      if (currentEvent.isMouse) {
        this.CheckMouseEvents(currentEvent);
      }
    }

    private void CheckMouseEvents(Event currentEvent) {
      bool leftMouseButton = currentEvent.button == 0;
      bool rightMouseButton = currentEvent.button == 1;

      bool rightMouseButtonMouseDown = rightMouseButton && currentEvent.type == EventType.MouseDown;

			if (currentEvent.type == EventType.ContextClick || rightMouseButtonMouseDown) {
        if (this.CanvasRect.Contains(currentEvent.mousePosition)) {
          this.OpenNodeCreationMenu(currentEvent);
          currentEvent.Use();
        }
        return;
      }

      if (leftMouseButton) {
        bool mouseInCanvas = this.CanvasRect.Contains(currentEvent.mousePosition);
        if (mouseInCanvas) {
          this.HandleLeftMouseButtonEventInCanvas(currentEvent);
          currentEvent.Use();
        }
        return;
      }
    }

    private void HandleLeftMouseButtonEventInCanvas(Event currentEvent) {
      Vector2 mousePosition = currentEvent.mousePosition;

      Node node = this.FindNodeContainingPoint(mousePosition);
      if (currentEvent.type == EventType.MouseDown) {
        if (node != null) {
          this.SelectNode(node);
          this.StartDragging(node, mousePosition);
        } else {
          this.DeselectCurrentNode();
        }
        return;
      } else if (currentEvent.type == EventType.MouseDrag) {
        this.UpdateDragging(mousePosition);
      } else if (currentEvent.type == EventType.MouseUp) {
        this.StopDragging();
      }
    }
  }
}