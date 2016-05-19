using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class GraphAssetEditorWindow : EditorWindow {
    // PRAGMA MARK - Public Interface
    public void OnGUI() {
			if (EditorApplication.isCompiling) {
				this.ShowNotice("Compiling, please wait..");
				return;
			}

      if (this.TargetGraph == null) {
        this.ShowNotice("Select a graph asset to edit!");
        return;
      }

			GUI.skin = this._skin;
      this.RecomputeCanvasRect();

			GUI.BeginGroup(this.CanvasRect);
        this.DrawGrid(this.CanvasRect);
        this.DrawNodes(this.CanvasRect);

        this.DrawInspectorWindow();
			GUI.EndGroup();

      this.CheckEvents(Event.current);
      this.SaveChanges();
    }


    // PRAGMA MARK - Internal
    private Rect _canvasRect;
    private Rect CanvasRect {
      get { return this._canvasRect; }
    }

    private void RecomputeCanvasRect() {
      this._canvasRect = new Rect(0.0f, 0.0f, this.position.width, this.position.height);
    }

    private void ShowNotice(string noticeText) {
      this.ShowNotification(new GUIContent(noticeText));
    }
  }
}