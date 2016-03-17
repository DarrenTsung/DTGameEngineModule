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
          SerializedProperty serializedObjReference = null;
          if (objIndex < this._serializedDataReference.arraySize) {
           serializedObjReference = this._serializedDataReference.GetArrayElementAtIndex(objIndex);
          }

          GUIStyle columnStyle = new GUIStyle();
          columnStyle.normal.background = this.ColumnBackgrounds[objIndex % this.ColumnBackgrounds.Length];
          Rect objRect = EditorGUILayout.BeginVertical(columnStyle, GUILayout.Width(kLabelWidth + kFieldWidth));
            this.ObjectOnGUI(obj, serializedObj, serializedObjReference, objRect);
          EditorGUILayout.EndVertical();

          objIndex++;
        }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.EndScrollView();

      // Add Button
      if (GUILayout.Button("Add", GUILayout.Height(kButtonHeight))) {
        TIdObject newObj = new TIdObject();
        this._list.Add(newObj);
        this.RebuildSerializedCopies(refreshReference: false);
      }

      // Commit or revert
      EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Commit changes", GUILayout.Height(kButtonHeight))) {
          this._serializedData.serializedObject.ApplyModifiedProperties();
    			EditorUtility.SetDirty(this._list);
          this.RebuildSerializedCopies(refreshReference: true);
        }

        if (GUILayout.Button("Revert", GUILayout.Height(kButtonHeight))) {
          IdListUtil<TIdList>.DirtyInstance();
          this._list = IdListUtil<TIdList>.Instance;
          this.RebuildSerializedCopies(refreshReference: true);
        }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.EndVertical();
    }

    private void OnEnable() {
      this._skin = Resources.Load("IdListWindow") as GUISkin;
      string listClassName = typeof(TIdList).Name;
      this.titleContent = new GUIContent(listClassName);

      this._list = IdListUtil<TIdList>.Instance;
      this.RebuildSerializedCopies(refreshReference: true);
    }

    // PRAGMA MARK - Internal
    private TIdList _list;
    private SerializedProperty _serializedData;
    private SerializedProperty _serializedDataReference;

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

    private void RebuildSerializedCopies(bool refreshReference) {
      this._serializedData = new SerializedObject(this._list).FindProperty("_data");
      if (refreshReference) {
        this._serializedDataReference = new SerializedObject(this._list).FindProperty("_data");
      }
    }

    private bool ArePropertyValuesChanged(SerializedProperty property, SerializedProperty propertyReference) {
      if (propertyReference == null) {
        return true;
      }

      object propertyValue;
      object propertyReferenceValue;
      try {
        propertyValue = property.GetValueAsObject();
        propertyReferenceValue = propertyReference.GetValueAsObject();
      } catch (NotImplementedException) {
        return false;
      }

      if (propertyReferenceValue == null) {
        return propertyValue != null;
      } else if (propertyValue == null) {
        return propertyReferenceValue != null;
      }

      return !propertyReferenceValue.Equals(propertyValue);
    }

    protected virtual void ObjectOnGUI(TIdObject obj, SerializedProperty serializedObj, SerializedProperty serializedObjReference, Rect objRect) {
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
      SerializedProperty propertyReference = serializedObjReference;
      SerializedProperty endProperty = property.GetEndProperty();
      SerializedProperty endReferenceProperty = (propertyReference != null) ? propertyReference.GetEndProperty() : null;
      int fieldIndex = 0;

      // go into the children of the property
      property.NextVisible(enterChildren: true);
      if (propertyReference != null) {
        propertyReference.NextVisible(enterChildren: true);
      }

      do {
        bool changed = this.ArePropertyValuesChanged(property, propertyReference);
        EditorGUIUtil.SetBoldDefaultFont(changed);

        EditorGUILayout.PropertyField(property);
        fieldIndex++;

        if (property.hasVisibleChildren && property.isExpanded) {
          this.DrawPropertyChildrenRecursive(property.Copy(), (propertyReference != null) ? propertyReference.Copy() : null);
        }
      } while (this.EnterNextVisibleForProperties(property, ref propertyReference, endReferenceProperty, enterChildren: false) && !SerializedProperty.EqualContents(property, endProperty));

      EditorGUIUtil.SetBoldDefaultFont(false);
      property.Reset();

      EditorGUIUtility.fieldWidth = oldFieldWidth;
      EditorGUIUtility.labelWidth = 0;
    }

    private void DrawPropertyChildrenRecursive(SerializedProperty property, SerializedProperty propertyReference) {
      SerializedProperty endProperty = property.GetEndProperty();
      SerializedProperty endReferenceProperty = (propertyReference != null) ? propertyReference.GetEndProperty() : null;

      // go into the children of the property
      property.NextVisible(enterChildren: true);
      if (propertyReference != null) {
        propertyReference.NextVisible(enterChildren: true);
      }

      EditorGUI.indentLevel++;
      do {
        bool changed = this.ArePropertyValuesChanged(property, propertyReference);
        EditorGUIUtil.SetBoldDefaultFont(changed);

        EditorGUILayout.PropertyField(property);

        if (property.hasVisibleChildren && property.isExpanded) {
          this.DrawPropertyChildrenRecursive(property.Copy(), (propertyReference != null) ? propertyReference.Copy() : null);
        }
      } while (this.EnterNextVisibleForProperties(property, ref propertyReference, endReferenceProperty, enterChildren: false) && !SerializedProperty.EqualContents(property, endProperty));

      EditorGUI.indentLevel--;
    }

    private bool EnterNextVisibleForProperties(SerializedProperty property, ref SerializedProperty propertyReference, SerializedProperty endReferenceProperty, bool enterChildren) {
      property.NextVisible(enterChildren);
      if (propertyReference != null) {
        bool succeeded = propertyReference.NextVisible(enterChildren);
        // if we can't enter the next visible for propertyReference, then it is new
        // and we should null out propertyReference so that the comparison shows that the value is changed
        if (!succeeded || SerializedProperty.EqualContents(propertyReference, endReferenceProperty)) {
          propertyReference = null;
        }
      }

      return true;
    }
  }
}
