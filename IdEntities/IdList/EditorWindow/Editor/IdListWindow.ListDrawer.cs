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
    private interface IListDrawer {
      void OnGUI(float width);
    }

    private class ListDrawer<TEntity> : IListDrawer where TEntity : DTEntity, new() {
      // PRAGMA MARK - Static
      private const float kLabelWidth = 150.0f;
      private const float kFieldWidth = 110.0f;

      private const int kIconEdgeSize = 160;

      private const float kButtonHeight = 40.0f;

      public ListDrawer() {
        this._skin = Resources.Load("IdListWindow") as GUISkin;
        this._list = IdList<TEntity>.Instance;
        this.RebuildSerializedCopies(refreshReference: true);
      }

      public void OnGUI(float width) {
        EditorGUILayout.BeginVertical(GUILayout.Width(width));

        // Objects
        this._currentScrollPosition = EditorGUILayout.BeginScrollView(this._currentScrollPosition);
        EditorGUILayout.BeginHorizontal();
          int objIndex = 0;
          foreach (TEntity entity in this._list) {
            SerializedProperty serializedObj = this._serializedData.GetArrayElementAtIndex(objIndex);
            SerializedProperty serializedObjReference = null;
            if (objIndex < this._serializedDataReference.arraySize) {
             serializedObjReference = this._serializedDataReference.GetArrayElementAtIndex(objIndex);
            }

            GUIStyle columnStyle = new GUIStyle();
            columnStyle.normal.background = this.ColumnBackgrounds[objIndex % this.ColumnBackgrounds.Length];
            Rect objRect = EditorGUILayout.BeginVertical(columnStyle, GUILayout.Width(kLabelWidth + kFieldWidth));
              this.ObjectOnGUI(entity, serializedObj, serializedObjReference, objRect);
              if (GUILayout.Button("X", GUILayout.Width(70))) {
                if (EditorUtility.DisplayDialog("Remove Element?",
                  "Are you sure you want to remove this element?",
                  "Remove",
                  "Cancel")) {
                  this._arrayIndexesToRemove.Add(objIndex);
                  break;
                }
              }
            EditorGUILayout.EndVertical();

            objIndex++;
          }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        // Remove any objects in array at this state
        if (this._arrayIndexesToRemove.Count > 0) {
          foreach (int index in this._arrayIndexesToRemove) {
            this._list.RemoveAt(index);
            this._serializedData.DeleteArrayElementAtIndex(index);
            if (index < this._serializedDataReference.arraySize) {
              this._serializedDataReference.DeleteArrayElementAtIndex(index);
            }
          }
          this.RebuildSerializedCopies(refreshReference: false);
          this._arrayIndexesToRemove.Clear();
        }


        // Add Button
        if (GUILayout.Button("Add", GUILayout.Height(kButtonHeight))) {
          this._list.AddNew();
          this.RebuildSerializedCopies(refreshReference: false);
        }

        // Commit or revert
        EditorGUILayout.BeginHorizontal();
          if (GUILayout.Button("Commit changes", GUILayout.Height(kButtonHeight))) {
            this._serializedData.serializedObject.ApplyModifiedProperties();
      			EditorUtility.SetDirty(this._list);
            AssetDatabase.SaveAssets();
            this.RebuildSerializedCopies(refreshReference: true);
          }

          if (GUILayout.Button("Revert", GUILayout.Height(kButtonHeight))) {
            IdList<TEntity>.DirtyInstance();
            this._list = IdList<TEntity>.Instance;
            this.RebuildSerializedCopies(refreshReference: true);
          }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();
      }

      // PRAGMA MARK - Internal
      private IdList<TEntity> _list;
      private SerializedProperty _serializedData;
      private SerializedProperty _serializedDataReference;
      private List<int> _arrayIndexesToRemove = new List<int>();

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
        if (this._list != null) {
          this._serializedData = new SerializedObject(this._list).FindProperty("_data");
          if (refreshReference) {
            this._serializedDataReference = new SerializedObject(this._list).FindProperty("_data");
          }
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

      protected virtual void ObjectOnGUI(TEntity entity, SerializedProperty serializedObj, SerializedProperty serializedObjReference, Rect objRect) {
        // Icon + Title
        Texture2D iconTexture = null;
        string title = "Default";

        EditorDisplayComponent displayComponent = entity.GetComponent<EditorDisplayComponent>();
        if (displayComponent != null) {
          iconTexture = displayComponent.iconTexture;
          if (!displayComponent.title.IsNullOrEmpty()) {
            title = Regex.Replace(displayComponent.title, @"\s+", "");
          }
        }

        iconTexture = iconTexture ?? Texture2DUtil.CreateTextureWithColor(Color.blue, kIconEdgeSize, kIconEdgeSize);

        GUIStyle titleStyle = new GUIStyle(this._skin.customStyles[0]);
        EditorGUILayout.LabelField(new GUIContent(title, iconTexture), titleStyle,
          GUILayout.Width(kIconEdgeSize),
          GUILayout.Height(kIconEdgeSize + 10.0f));



        // Fields
        float oldFieldWidth = EditorGUIUtility.fieldWidth;
        EditorGUIUtility.fieldWidth = kFieldWidth;
        EditorGUIUtility.labelWidth = kLabelWidth;

        SerializedProperty property = serializedObj;
        SerializedProperty propertyReference = serializedObjReference;
        this.DrawPropertyChildrenRecursive(property, propertyReference);

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
}
