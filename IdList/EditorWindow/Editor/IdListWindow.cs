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
  public abstract class IdListWindow<TIdList, TIdObject> : EditorWindow where TIdList : IdList<TIdObject>
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
          SerializedProperty serializedObj = this._serializedData.GetArrayElementAtIndex(objIndex);

          GUIStyle columnStyle = new GUIStyle();
          columnStyle.normal.background = this.ColumnBackgrounds[objIndex % this.ColumnBackgrounds.Length];
          Rect objRect = EditorGUILayout.BeginVertical(columnStyle, GUILayout.Width(kLabelWidth + kFieldWidth));
            this.ObjectOnGUI(obj, serializedObj, objRect);
          EditorGUILayout.EndVertical();

          objIndex++;
        }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.EndScrollView();

      // Add Button
      if (GUILayout.Button("Add", GUILayout.Height(kButtonHeight))) {
        TIdObject newObj = new TIdObject();
        this._list.Add(newObj);
        this.RebuildSerializedCopies();
      }

      // Commit or revert
      EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Commit changes", GUILayout.Height(kButtonHeight))) {
          this._serializedList.ApplyModifiedProperties();
    			EditorUtility.SetDirty(this._list);
          this.RebuildInitialValues();
        }

        if (GUILayout.Button("Revert", GUILayout.Height(kButtonHeight))) {
          IdListUtil<TIdList>.DirtyInstance();
          this._list = IdListUtil<TIdList>.Instance;
          this.RebuildSerializedCopies();
          this.RebuildInitialValues();
        }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.EndVertical();
    }

    private void OnEnable() {
      this._skin = Resources.Load("IdListWindow") as GUISkin;
      string listClassName = typeof(TIdList).Name;
      this.titleContent = new GUIContent(listClassName);

      this._objFields = typeof(TIdObject).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

      this._list = IdListUtil<TIdList>.Instance;
      this.RebuildSerializedCopies();
      this.RebuildInitialValues();
    }

    // PRAGMA MARK - Internal
    private TIdList _list;
    private SerializedObject _serializedList;
    private SerializedProperty _serializedData;

    private FieldInfo[] _objFields;

    private Dictionary<TIdObject, object[]> _objInitialValues = new Dictionary<TIdObject, object[]>();
    private Texture2D[] _columnBackgrounds;
    private Texture2D[] ColumnBackgrounds {
      get {
        if (this._columnBackgrounds == null || this._columnBackgrounds.Length == 0) {
          this._columnBackgrounds = new Texture2D[] {
            Texture2DUtil.CreateTextureWithColor(ColorExtensions.HexStringToColor("#ADADAD")),
            Texture2DUtil.CreateTextureWithColor(ColorExtensions.HexStringToColor("#C2C2C2"))
          };
        }
        return this._columnBackgrounds;
      }
    }

    private Vector2 _currentScrollPosition;
    private GUISkin _skin;

    private void RebuildSerializedCopies() {
      this._serializedList = new SerializedObject(this._list);
      this._serializedData = this._serializedList.FindProperty("_data");
    }

    private void RebuildInitialValues() {
      this._objInitialValues.Clear();
      foreach (TIdObject obj in this._list) {
        this._objInitialValues[obj] = (from field in this._objFields select field.GetValue(obj)).ToArray();
      }
    }

    private bool IsFieldValueForObjectChanged(TIdObject obj, int fieldIndex, object fieldValue) {
      object[] initialFieldValues = this._objInitialValues.SafeGet(obj);

      if (initialFieldValues == null) {
        Debug.LogError("IsFieldValueForObjectChanged - no initial field value for index: " + fieldIndex);
        return false;
      }

      if (!initialFieldValues.ContainsIndex(fieldIndex)) {
        Debug.LogError("IsFieldValueForObjectChanged - invalid index: " + fieldIndex + "!");
        return false;
      }

      object initialValue = initialFieldValues[fieldIndex];

      if (initialValue == null) {
        return fieldValue != null;
      }

      return !initialValue.Equals(fieldValue);
    }

    protected virtual void ObjectOnGUI(TIdObject obj, SerializedProperty serializedObj, Rect objRect) {
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

      SerializedProperty property = serializedObj;
      SerializedProperty endProperty = property.GetEndProperty();
      int fieldIndex = 0;

      while (property.NextVisible(enterChildren: true) && !SerializedProperty.EqualContents(property, endProperty)) {
        if (this._objFields.ContainsIndex(fieldIndex)) {
          EditorGUIUtil.SetBoldDefaultFont(this.IsFieldValueForObjectChanged(obj, fieldIndex, property.GetValueAsObject()));
        } else {
          Debug.LogWarning("Going out of field index bounds with index: " + fieldIndex);
        }

        EditorGUILayout.PropertyField(property);
        fieldIndex++;
      }

      EditorGUIUtil.SetBoldDefaultFont(false);
      property.Reset();

      EditorGUIUtility.fieldWidth = oldFieldWidth;
      EditorGUIUtility.labelWidth = 0;
    }
  }
}
