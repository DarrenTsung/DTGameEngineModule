using DT;
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;

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
      EditorGUILayout.BeginVertical();
      this._currentScrollPosition = EditorGUILayout.BeginScrollView(this._currentScrollPosition);
      Rect selectionRect = EditorGUILayout.BeginHorizontal(GUILayout.Width(1000), GUILayout.Height(kSelectionButtonHeight));
        int typeIndex = 0;
        foreach (Type entitySubclassType in DTEntityUtil.EntitySubclasses) {
          bool selected = this._selectedEntityType == entitySubclassType;

          Rect buttonPos = new Rect(selectionRect.x + kSelectionButtonWidth * typeIndex, selectionRect.y, kSelectionButtonWidth, kSelectionButtonHeight);
          if (GUI.Toggle(buttonPos, selected, entitySubclassType.Name, GUI.skin.button)) {
            // only select type if wasn't selected before
            if (selected == false) {
              this.SelectEntityType(entitySubclassType);
            }
          }
          typeIndex++;
        }

        EditorGUILayout.LabelField("", GUILayout.Width(typeIndex * kSelectionButtonWidth), GUILayout.Height(kSelectionButtonHeight));
      EditorGUILayout.EndHorizontal();


      if (this._currentListDrawer != null) {
        this._currentListDrawer.OnGUI();
      }

      EditorGUILayout.EndVertical();
    }

    private void OnEnable() {
      this.titleContent = new GUIContent("IdListWindow");

      this.SelectEntityType(DTEntityUtil.EntitySubclasses[0]);
    }

    // PRAGMA MARK - Internal
    private Type _selectedEntityType;

    private Vector2 _currentScrollPosition;
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
