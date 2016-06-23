using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public partial class IdListWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kSelectionButtonWidth = 150.0f;
    private const float kSelectionButtonHeight = 40.0f;

    [MenuItem("DarrenTsung/IdListWindow")]
    public static void ShowWindow() {
      IdListWindow window = EditorWindow.GetWindow(typeof(IdListWindow)) as IdListWindow;
      window.Show();
    }

    // PRAGMA MARK - Unity Methods
    public void OnGUI() {
      EditorGUILayout.BeginHorizontal();

      this._currentListSelectionScrollPosition = EditorGUILayout.BeginScrollView(this._currentListSelectionScrollPosition);
      Rect selectionRect = EditorGUILayout.BeginVertical(GUILayout.MinWidth(kSelectionButtonWidth), GUILayout.MaxWidth(kSelectionButtonWidth));
        int typeIndex = 0;
        foreach (Type entitySubclassType in DTEntityUtil.EntitySubclasses) {
          bool selected = this._selectedEntityType == entitySubclassType;

          Rect buttonPos = new Rect(selectionRect.x, selectionRect.y + kSelectionButtonHeight * typeIndex, kSelectionButtonWidth, kSelectionButtonHeight);
          if (GUI.Toggle(buttonPos, selected, entitySubclassType.Name, GUI.skin.button)) {
            // only select type if wasn't selected before
            if (selected == false) {
              this.SelectEntityType(entitySubclassType);
            }
          }
          typeIndex++;
        }

        EditorGUILayout.LabelField("", GUILayout.Width(kSelectionButtonWidth), GUILayout.Height(typeIndex * kSelectionButtonHeight));
      EditorGUILayout.EndVertical();
      EditorGUILayout.EndScrollView();

      if (this._currentListDrawer != null) {
        this._currentListDrawer.OnGUI(width : this.position.width - kSelectionButtonWidth - 10.0f);
      }

      EditorGUILayout.EndHorizontal();
    }

    private void OnEnable() {
      this.titleContent = new GUIContent("IdListWindow");

      this.SelectEntityType(DTEntityUtil.EntitySubclasses[0]);
    }

    // PRAGMA MARK - Internal
    private Type _selectedEntityType;

    private Vector2 _currentListSelectionScrollPosition;
    private Dictionary<Type, IListDrawer> _listDrawerMap = new Dictionary<Type, IListDrawer>();
    private IListDrawer _currentListDrawer;

    private void SelectEntityType(Type entityType) {
      this._selectedEntityType = entityType;
      this.RefreshListDrawer();
    }

    private void RefreshListDrawer() {
      if (this._listDrawerMap.ContainsKey(this._selectedEntityType)) {
        this._currentListDrawer = this._listDrawerMap[this._selectedEntityType];
        return;
      }

      try {
        this._currentListDrawer = (IListDrawer)Activator.CreateInstance(typeof(ListDrawer<>).MakeGenericType(this._selectedEntityType));
      } catch (InvalidCastException) {}

      this._listDrawerMap[this._selectedEntityType] = this._currentListDrawer;
    }
  }
}
