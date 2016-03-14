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
  public abstract class IdListWindow<TIdList, TIdObject> : EditorWindow where TIdList : IIdList<TIdObject>, new()
                                                                        where TIdObject : IIdObject, new() {
    // PRAGMA MARK - Static
    private const float kLabelWidth = 150.0f;
    private const float kFieldWidth = 110.0f;

    private const int kIconEdgeSize = 160;

    private const float kButtonHeight = 40.0f;

    // PRAGMA MARK - Unity Methods
    public void OnGUI() {
      EditorGUILayout.BeginVertical();

      // Objects
      this._currentScrollPosition = EditorGUILayout.BeginScrollView(this._currentScrollPosition);
      EditorGUILayout.BeginHorizontal();
        int objIndex = 0;
        foreach (TIdObject obj in this._list) {
          GUIStyle columnStyle = new GUIStyle();
          columnStyle.normal.background = this._columnBackgrounds[objIndex % this._columnBackgrounds.Length];
          Rect objRect = EditorGUILayout.BeginVertical(columnStyle, GUILayout.Width(kLabelWidth + kFieldWidth));
            this.ObjectOnGUI(obj, objRect);
          EditorGUILayout.EndVertical();

          objIndex++;
        }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.EndScrollView();

      // Add Button
      if (GUILayout.Button("Add", GUILayout.Height(kButtonHeight))) {
        TIdObject newObj = new TIdObject();
        this._list.Add(newObj);
      }

      // Commit or revert
      EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Commit changes", GUILayout.Height(kButtonHeight))) {
          this._list.SaveChanges();
          foreach (TIdObject obj in this._list) {
            this.AddInitialValuesForObject(obj);
          }
        }

        if (GUILayout.Button("Revert", GUILayout.Height(kButtonHeight))) {
          this._list = new TIdList();
        }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.EndVertical();
    }

    private void OnEnable() {
      this._skin = Resources.Load("IdListWindow") as GUISkin;
      string listClassName = typeof(TIdList).Name;
      this.titleContent = new GUIContent(listClassName);
      this._list = new TIdList();

      this._objFields = typeof(TIdObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
      this._objPublicFields = (from field in this._objFields where !field.IsPrivate select field).ToArray();
      this._objPrivateFields = (from field in this._objFields where field.IsPrivate select field).ToArray();

      this._objInitialValues = new Dictionary<TIdObject, object[]>();
      foreach (TIdObject obj in this._list) {
        this.AddInitialValuesForObject(obj);
      }

      this._columnBackgrounds = new Texture2D[] {
        Texture2DUtil.CreateTextureWithColor(ColorExtensions.HexStringToColor("#ADADAD")),
        Texture2DUtil.CreateTextureWithColor(ColorExtensions.HexStringToColor("#C2C2C2"))
      };
    }

    // PRAGMA MARK - Internal
    private TIdList _list;

    private FieldInfo[] _objFields;

    private FieldInfo[] _objPublicFields;
    private FieldInfo[] _objPrivateFields;

    private Dictionary<TIdObject, object[]> _objInitialValues;
    private Dictionary<TIdObject, bool> _objShowPrivateFields = new Dictionary<TIdObject, bool>();
    private Texture2D[] _columnBackgrounds;

    private Vector2 _currentScrollPosition;
    private GUISkin _skin;

    private void AddInitialValuesForObject(TIdObject obj) {
      this._objInitialValues[obj] = (from field in this._objFields select field.GetValue(obj)).ToArray();
    }

    private bool IsFieldValueForObjectChanged(TIdObject obj, int fieldIndex, object fieldValue) {
      object[] initialFieldValues = this._objInitialValues.SafeGet(obj);

      if (initialFieldValues == null) {
        this.AddInitialValuesForObject(obj);
        initialFieldValues = this._objInitialValues.SafeGet(obj);
      }

      if (!initialFieldValues.ContainsIndex(fieldIndex)) {
        Debug.LogError("IsFieldValueForObjectChanged - invalid index!");
        return false;
      }

      object initialValue = initialFieldValues[fieldIndex];

      if (initialValue == null) {
        return fieldValue != null;
      }

      return !initialValue.Equals(fieldValue);
    }

    protected virtual void ObjectOnGUI(TIdObject obj, Rect objRect) {
      // Icon + Title
      IIdListDisplayObject windowObject = obj as IIdListDisplayObject;
      Texture2D iconTexture = null;
      string title = "";

      if (windowObject != null) {
        iconTexture = windowObject.IconTexture;
        title = windowObject.Title;
      }

      if (title.IsNullOrEmpty()) {
        title = "Default";
      } else {
        // remove spaces if User inputted title
        title = Regex.Replace(title, @"\s+", "");
      }
      iconTexture = iconTexture ?? Texture2DUtil.CreateTextureWithColor(Color.blue, kIconEdgeSize, kIconEdgeSize);

      GUIStyle titleStyle = new GUIStyle(this._skin.customStyles[0]);
      EditorGUILayout.LabelField(new GUIContent(title, iconTexture), titleStyle,
        GUILayout.Width(kIconEdgeSize),
        GUILayout.Height(kIconEdgeSize));



      // Fields
      float oldFieldWidth = EditorGUIUtility.fieldWidth;
      EditorGUIUtility.fieldWidth = kFieldWidth;
      EditorGUIUtility.labelWidth = kLabelWidth;

      int fieldIndex = 0;
      foreach (FieldInfo field in this._objPublicFields) {
        this.DrawField(obj, field, fieldIndex);
        fieldIndex++;
      }

      this._objShowPrivateFields[obj] = EditorGUILayout.Foldout(this._objShowPrivateFields.SafeGet(obj, defaultValue: false), "Private Fields");
      if (this._objShowPrivateFields[obj]) {
        EditorGUI.indentLevel++;
        foreach (FieldInfo field in this._objPrivateFields) {
          this.DrawField(obj, field, fieldIndex);
          fieldIndex++;
        }
        EditorGUI.indentLevel--;
      }

      EditorGUIUtility.fieldWidth = oldFieldWidth;
      EditorGUIUtility.labelWidth = 0;
    }

    private void DrawField(TIdObject obj, FieldInfo field, int fieldIndex) {
      EditorGUILayout.BeginHorizontal();

      float fieldWidth = EditorGUIUtility.fieldWidth;

      GUIStyle fieldStyle = new GUIStyle(GUI.skin.textField);
      GUIStyle labelStyle = new GUIStyle(GUI.skin.label);

      object fieldValue = field.GetValue(obj);
      bool changed = this.IsFieldValueForObjectChanged(obj, fieldIndex, fieldValue);
      if (changed) {
        fieldStyle.font = EditorStyles.boldFont;
        labelStyle.font = EditorStyles.boldFont;
      }

      string parsedFieldName = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(Regex.Replace(field.Name, "(\\B[A-Z])", " $1"));
      EditorGUILayout.PrefixLabel(parsedFieldName + ":", fieldStyle, labelStyle);
      if (field.FieldType == typeof(int)) {
        field.SetValue(obj, EditorGUILayout.IntField((int)fieldValue, fieldStyle, GUILayout.Width(fieldWidth)));
      } else if (field.FieldType == typeof(float)) {
        field.SetValue(obj, EditorGUILayout.FloatField((float)fieldValue, fieldStyle, GUILayout.Width(fieldWidth)));
      } else if (field.FieldType == typeof(string)) {
        field.SetValue(obj, EditorGUILayout.TextField((string)fieldValue, fieldStyle, GUILayout.Width(fieldWidth)));
      } else if (!field.FieldType.IsInstanceOfType(typeof(UnityEngine.Object))) {
        field.SetValue(obj, EditorGUILayout.ObjectField((UnityEngine.Object)fieldValue, field.FieldType, false, GUILayout.Width(fieldWidth), GUILayout.Height(40.0f)));
      }

      EditorGUILayout.EndHorizontal();
    }
  }
}
