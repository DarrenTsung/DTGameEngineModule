using DT;
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;

namespace DT.GameEngine {
  public abstract class IdListWindow<TIdList, TIdObject> : EditorWindow where TIdList : IIdList<TIdObject>, new()
                                                                        where TIdObject : IIdObject, new() {
    // PRAGMA MARK - Unity Methods
    public void OnGUI() {
      EditorGUILayout.BeginVertical();

      // Objects
      this._currentScrollPosition = EditorGUILayout.BeginScrollView(this._currentScrollPosition);
        foreach (TIdObject obj in this._list) {
          Rect objRect = EditorGUILayout.BeginHorizontal();
          this.ObjectOnGUI(obj, objRect);
          EditorGUILayout.EndHorizontal();
        }
      EditorGUILayout.EndScrollView();

      // Add Button
      if (GUILayout.Button("Add")) {
        TIdObject newObj = new TIdObject();
        this._list.Add(newObj);
      }

      // Commit or revert
      EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Commit changes")) {
          this._list.SaveChanges();
        }

        if (GUILayout.Button("Revert")) {
          this._list = new TIdList();
        }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.EndVertical();
    }

    private void OnEnable() {
      string listClassName = typeof(TIdList).Name;
      this.titleContent = new GUIContent(listClassName);
      this._list = new TIdList();

      this._objFields = typeof(TIdObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      this._columnBackgrounds = new Texture2D[] {
        Texture2DUtil.CreateTextureWithColor(ColorExtensions.HexStringToColor("#ADADAD")),
        Texture2DUtil.CreateTextureWithColor(ColorExtensions.HexStringToColor("#C2C2C2"))
      };
    }

    // PRAGMA MARK - Internal
    private TIdList _list;

    private FieldInfo[] _objFields;
    private Texture2D[] _columnBackgrounds;

    private Vector2 _currentScrollPosition;

    protected virtual void ObjectOnGUI(TIdObject obj, Rect objRect) {
      int fieldIndex = 0;
      foreach (FieldInfo field in this._objFields) {
        GUIStyle columnStyle = new GUIStyle();
        columnStyle.normal.background = this._columnBackgrounds[fieldIndex % this._columnBackgrounds.Length];
        EditorGUILayout.BeginVertical(columnStyle, GUILayout.Height(40));

        EditorGUIUtility.fieldWidth = 100.0f;
        EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(new GUIContent(field.Name)).x + 5.0f;

        float fieldWidth = EditorGUIUtility.fieldWidth + EditorGUIUtility.labelWidth;

        if (field.FieldType == typeof(int)) {
          field.SetValue(obj, EditorGUILayout.IntField(field.Name, (int)field.GetValue(obj), GUILayout.Width(fieldWidth)));
        } else if (field.FieldType == typeof(float)) {
          field.SetValue(obj, EditorGUILayout.FloatField(field.Name, (float)field.GetValue(obj), GUILayout.Width(fieldWidth)));
        } else if (field.FieldType == typeof(string)) {
          field.SetValue(obj, EditorGUILayout.TextField(field.Name, (string)field.GetValue(obj), GUILayout.Width(fieldWidth)));
        } else if (!field.FieldType.IsInstanceOfType(typeof(UnityEngine.Object))) {
          field.SetValue(obj, EditorGUILayout.ObjectField(field.Name, (UnityEngine.Object)field.GetValue(obj), field.FieldType, false, GUILayout.Width(fieldWidth), GUILayout.Height(40.0f)));
        }

        EditorGUILayout.EndVertical();
        fieldIndex++;
      }
    }
  }
}
