using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	public class IdQuantityDrawer<TEntity> : PropertyDrawer where TEntity : DTEntity {
    // PRAGMA MARK - Public Internal
		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      this._property = property;

      EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtilityUtil.IndentedLabelWidth));
        EditorGUILayout.PropertyField(this.IdProperty, GUILayout.Width(30.0f));
        this.QuantityProperty.intValue = EditorGUILayout.IntField(this.QuantityProperty.intValue);
      EditorGUILayout.EndHorizontal();
		}


    // PRAGMA MARK - Internal
    private SerializedProperty _property;

    private SerializedProperty _quantityProperty;
    private SerializedProperty QuantityProperty {
      get {
        if (this._quantityProperty == null) {
          this._quantityProperty = this._property.FindPropertyRelative("quantity");
          if (this._quantityProperty == null) {
            Debug.LogError("Failed to find quantity property relative from EntityTypeId property!");
          }
        }
        return this._quantityProperty;
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
	}
}