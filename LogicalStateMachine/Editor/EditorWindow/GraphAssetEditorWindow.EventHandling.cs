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
      Vector2 mousePosition = currentEvent.mousePosition;

      bool leftMouseButton = currentEvent.button == 0;
      bool rightMouseButton = currentEvent.button == 1;

      bool rightMouseButtonMouseDown = rightMouseButton && currentEvent.type == EventType.MouseDown;

			if (currentEvent.type == EventType.ContextClick || rightMouseButtonMouseDown) {
        if (this.CanvasRect.Contains(mousePosition)) {
          Node node = this.FindNodeContainingPoint(mousePosition);
          if (node == null) {
            this.OpenNodeCreationMenu(currentEvent);
          } else {
            this.OpenNodeContextMenu(currentEvent, node);
          }
          currentEvent.Use();
        }
        return;
      }

      if (leftMouseButton) {
        bool mouseInCanvas = this.CanvasRect.Contains(mousePosition);
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
          this.StartDraggingNode(node, mousePosition);
        } else {
          if (currentEvent.modifiers == EventModifiers.Alt) {
            this.StartDraggingPanner(mousePosition);
          }
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