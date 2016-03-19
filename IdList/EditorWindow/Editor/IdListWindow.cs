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
  public class IdListWindow : EditorWindow {
    // PRAGMA MARK - Static
    private const float kLabelWidth = 150.0f;
    private const float kFieldWidth = 110.0f;

    private const int kIconEdgeSize = 160;

    private const float kSelectionButtonWidth = 150.0f;
    private const float kButtonHeight = 40.0f;

    [MenuItem("DarrenTsung/IdListWindow")]
    public static void ShowWindow() {
      IdListWindow window = EditorWindow.GetWindow(typeof(IdListWindow)) as IdListWindow;
      window.Show();
    }

    // PRAGMA MARK - Unity Methods
    public void OnGUI() {
      EditorGUILayout.BeginVertical();
      this._currentScrollPosition = EditorGUILayout.BeginScrollView(this._currentScrollPosition);
      Rect selectionRect = EditorGUILayout.BeginHorizontal(GUILayout.Width(1000), GUILayout.Height(kButtonHeight));
        int typeIndex = 0;
        foreach (Type entitySubclassType in DTEntityUtil.EntitySubclasses) {
          bool selected = this._selectedEntityType == entitySubclassType;

          Rect buttonPos = new Rect(selectionRect.x + kSelectionButtonWidth * typeIndex, selectionRect.y, kSelectionButtonWidth, kButtonHeight);
          if (GUI.Toggle(buttonPos, selected, entitySubclassType.Name, GUI.skin.button)) {
            // only select type if wasn't selected before
            if (selected == false) {
              this.SelectEntityType(entitySubclassType);
            }
          }
          typeIndex++;
        }

        EditorGUILayout.LabelField("", GUILayout.Width(typeIndex * kSelectionButtonWidth), GUILayout.Height(kButtonHeight));
      EditorGUILayout.EndHorizontal();


      if (this._selectedEntityListType != null) {
        // Objects
        EditorGUILayout.BeginHorizontal();
          for (int objIndex = 0; objIndex < this._serializedData.arraySize; ++objIndex) {
            SerializedProperty serializedObj = this._serializedData.GetArrayElementAtIndex(objIndex);
            SerializedProperty serializedObjReference = null;
            if (objIndex < this._serializedDataReference.arraySize) {
             serializedObjReference = this._serializedDataReference.GetArrayElementAtIndex(objIndex);
            }

            GUIStyle columnStyle = new GUIStyle();
            columnStyle.normal.background = this.ColumnBackgrounds[objIndex % this.ColumnBackgrounds.Length];
            Rect objRect = EditorGUILayout.BeginVertical(columnStyle, GUILayout.Width(kLabelWidth + kFieldWidth));
              this.ObjectOnGUI(serializedObj, serializedObjReference, objRect);
            EditorGUILayout.EndVertical();
          }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();

        // Add Button
        if (GUILayout.Button("Add", GUILayout.Height(kButtonHeight))) {
          this._list.AddNew();
          this.RebuildSerializedCopies(refreshReference: false);
        }

        // Commit or revert
        EditorGUILayout.BeginHorizontal();
          if (GUILayout.Button("Commit changes", GUILayout.Height(kButtonHeight))) {
            this._serializedData.serializedObject.ApplyModifiedProperties();
      			EditorUtility.SetDirty(this._scriptableList);
            this.RebuildSerializedCopies(refreshReference: true);
          }

          if (GUILayout.Button("Revert", GUILayout.Height(kButtonHeight))) {
            this.DirtyListInstance();
            this.RefreshList();
            this.RebuildSerializedCopies(refreshReference: true);
          }
        EditorGUILayout.EndHorizontal();
      }

      EditorGUILayout.EndVertical();
    }

    private void OnEnable() {
      this._skin = Resources.Load("IdListWindow") as GUISkin;
      this.titleContent = new GUIContent("IdListWindow");

      this.SelectEntityType(DTEntityUtil.EntitySubclasses[0]);
    }

    // PRAGMA MARK - Internal
    private IIdList _list;
    private ScriptableObject _scriptableList;
    private SerializedProperty _serializedData;
    private SerializedProperty _serializedDataReference;

    private Type _selectedEntityType;
    private Type _selectedEntityListType;

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

    private void SelectEntityType(Type entityType) {
      this._selectedEntityType = entityType;
      this._selectedEntityListType = typeof(IdList<>).MakeGenericType(this._selectedEntityType);

      this.RefreshList();
      this.RebuildSerializedCopies(refreshReference: true);
    }

    private void DirtyListInstance() {
      this._selectedEntityListType.GetMethod("DirtyInstance", BindingFlags.Public | BindingFlags.Static).Invoke(null, null);
    }

    private void RefreshList() {
      try {
        object instance = this._selectedEntityListType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static).GetValue(null, null);
        this._list = (IIdList)instance;
        this._scriptableList = (ScriptableObject)instance;
      } catch (InvalidCastException) {
        this._list = null;
        this._scriptableList = null;
      }
    }

    private void RebuildSerializedCopies(bool refreshReference) {
      if (this._scriptableList != null) {
        this._serializedData = new SerializedObject(this._scriptableList).FindProperty("_data");
        if (refreshReference) {
          this._serializedDataReference = new SerializedObject(this._scriptableList).FindProperty("_data");
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

    protected virtual void ObjectOnGUI(SerializedProperty serializedObj, SerializedProperty serializedObjReference, Rect objRect) {
      // Icon + Title
      Texture2D iconTexture = null;
      string title = "Default";

      SerializedProperty windowProperty = serializedObj.FindPropertyRelative("windowDisplayComponent");
      if (windowProperty != null) {
        SerializedProperty iconTextureProperty = windowProperty.FindPropertyRelative("iconTexture");
        if (iconTextureProperty != null) {
          iconTexture = iconTextureProperty.objectReferenceValue as Texture2D;
        }

        SerializedProperty titleProperty = windowProperty.FindPropertyRelative("title");
        if (titleProperty != null) {
          title = Regex.Replace(titleProperty.stringValue, @"\s+", "");
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
