using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const int kGridLineSpacing = 10;        // number of pixels between grid line
    private const int kSecondaryGridSpacing = 5;    // number of grid lines between secondary grid line

    private static readonly Color kLightLineColor = new Color(0.0f, 0.0f, 0.0f, 0.15f);
    private static readonly Color kDarkLineColor = new Color(0.0f, 0.0f, 0.0f, 0.25f);


    // PRAGMA MARK - Internal
		private void DrawGrid(Rect rect, Vector2 offset) {
      Color oldColor = Handles.color;

      int numberOfVerticalLines = Mathf.CeilToInt(rect.width / kGridLineSpacing);
      int numberOfHorizontalLines = Mathf.CeilToInt(rect.height / kGridLineSpacing);

      for (int i = 0; i < numberOfVerticalLines; i++) {
        float x = i * kGridLineSpacing;

        Handles.color = (i % kSecondaryGridSpacing == 0) ? kDarkLineColor : kLightLineColor;
        Handles.DrawLine(new Vector3(x, 0.0f, 0.0f), new Vector3(x, rect.height, 0.0f));
      }

      for (int i = 0; i < numberOfHorizontalLines; i++) {
        float y = i * kGridLineSpacing;

        Handles.color = (i % kSecondaryGridSpacing == 0) ? kDarkLineColor : kLightLineColor;
        Handles.DrawLine(new Vector3(0.0f, y, 0.0f), new Vector3(rect.width, y, 0.0f));
      }

			Handles.color = oldColor;
		}

    private Vector2 SnapToGrid(Vector2 point) {
      float newX = Mathf.Round(point.x / kGridLineSpacing) * kGridLineSpacing;
      float newY = Mathf.Round(point.y / kGridLineSpacing) * kGridLineSpacing;
      return new Vector2(newX, newY);
    }
  }
}