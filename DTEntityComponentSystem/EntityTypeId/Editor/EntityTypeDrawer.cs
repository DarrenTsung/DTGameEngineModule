using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	[CustomPropertyDrawer(typeof(EntityTypeId))]
	public class EntityTypeIdDrawer : PropertyDrawer {
    // PRAGMA MARK - Public Internal
		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      this._property = property;

      EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtilityUtil.IndentedLabelWidth));
        if (this.DTEntityTypeNames.Length == 0) {
          EditorGUILayout.LabelField("No DTEntity types found!");
  			} else {
    				this.CurrentSelectedIndex = EditorGUILayout.Popup(this.CurrentSelectedIndex, this.DTEntityTypeNames);
            if (this._idListDrawerMethod != null) {
              this._idListDrawerMethod.Invoke(null, new object[] { this.IdProperty });
            }
        }
      EditorGUILayout.EndHorizontal();
		}


    // PRAGMA MARK - Internal
    private string[] _entityTypeNames = null;
    private string[] DTEntityTypeNames {
      get {
        if (this._entityTypeNames == null) {
          this._entityTypeNames = DTEntityUtil.EntitySubclasses.Select(t => t.Name).ToArray();
        }
        return this._entityTypeNames;
      }
    }

    private int? _currentSelectedIndex = 0;
    private int CurrentSelectedIndex {
      get { return this._currentSelectedIndex.Value; }
      set {
        if (this._currentSelectedIndex == null) {
          this._currentSelectedIndex = this.GetIndexForPropertyStringValue();
        }

        if (this._currentSelectedIndex.Value == value) {
          return;
        }

        this._currentSelectedIndex = value;
        this.HandleCurrentSelectedIndexChanged();
      }
    }

    private SerializedProperty _property;

    private SerializedProperty _typeNameProperty;
    private SerializedProperty TypeNameProperty {
      get {
        if (this._typeNameProperty == null) {
          this._typeNameProperty = this._property.FindPropertyRelative("typeName");
          if (this._typeNameProperty == null) {
            Debug.LogError("Failed to find typeName property relative from EntityTypeId property!");
          }
        }
        return this._typeNameProperty;
      }
    }

    private SerializedProperty _idProperty;
    private SerializedProperty IdProperty {
      get {
        if (this._idProperty == null) {
          this._idProperty = this._property.FindPropertyRelative("id");
          if (this._idProperty == null) {
            Debug.LogError("Failed to find id property relative from EntityTypeId property!");
          }
        }
        return this._idProperty;
      }
    }

    private void HandleCurrentSelectedIndexChanged() {
      if (!this.DTEntityTypeNames.ContainsIndex(this.CurrentSelectedIndex)) {
        Debug.LogError("EntityTypeDrawer - CurrentSelectedIndex changed to an invalid index, how did that even happen??");
        return;
      }

      if (this.TypeNameProperty == null) {
        Debug.LogError("EntityTypeDrawer - CurrentSelectedIndex changed before property was assigned?");
        return;
      }

      string typeName = this.DTEntityTypeNames[this.CurrentSelectedIndex];
      this.SetTypeName(typeName);
    }

    private int GetIndexForPropertyStringValue() {
      if (this.TypeNameProperty == null) {
        Debug.LogError("EntityTypeDrawer - GetIndexForPropertyStringValue does not have property not set!");
        return 0;
      }

      Type entityType = DTEntityUtil.EntitySubclassesByName.SafeGet(this.TypeNameProperty.stringValue);
      // if string has not been set before, set to first entityType
      if (entityType == null) {
        this.SetTypeName(this.DTEntityTypeNames[0]);
        return 0;
      }

      int index = Array.IndexOf(this.DTEntityTypeNames, entityType.Name);
      if (index == -1) {
        Debug.LogError("EntityTypeDrawer - Couldn't find type name in list!");
        this.SetTypeName(this.DTEntityTypeNames[0]);
        return 0;
      }

      return index;
    }

    private MethodInfo _idListDrawerMethod;
    private void SetTypeName(string typeName) {
      Type entityType = DTEntityUtil.EntitySubclassesByName.SafeGet(typeName);
      if (entityType == null) {
        Debug.LogError("Failed to get entity type from type name: " + typeName);
        return;
      }

      this.TypeNameProperty.stringValue = typeName;
      this._idListDrawerMethod = IdListDrawerUtil.GetDrawListFieldMethod(entityType);
    }
	}
}