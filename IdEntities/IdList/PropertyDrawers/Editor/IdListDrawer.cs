using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	[CustomPropertyDrawer(typeof(IdAttribute))]
	public class IdListDrawer : PropertyDrawer {
    // PRAGMA MARK - Static
    private const int kIdFieldWidth = 100;
    private const int kPadding = 5;

    private MethodInfo _drawListFieldMethod = null;
    private MethodInfo GetDrawListFieldMethod() {
      if (this._drawListFieldMethod == null) {
        IdAttribute idAttribute = attribute as IdAttribute;

        Type entityType = idAttribute.type;
        if (entityType == null) {
          Type classType = this.fieldInfo.ReflectedType;
          Type[] classGenericTypes = classType.GetGenericArguments();
          if (classGenericTypes.Length > 0) {
            for (int i = 0; i < classGenericTypes.Length; ++i) {
              Type genericType = classGenericTypes[i];
              if (genericType.IsSubclassOf(typeof(DTEntity))) {
                entityType = genericType;
                break;
              }
            }

            if (entityType == null) {
              Debug.LogError("Attempting to reflect entity parameter type, but failed to find subclass of DTEntity in generic type arguments!");
              return null;
            }
          } else {
            Debug.LogError("Attempting to reflect entity parameter type, but no generic type arguments!");
            return null;
          }
        }

        Type utilType = typeof(IdListDrawer.IdListDrawerUtil<>).MakeGenericType(entityType);
        this._drawListFieldMethod = utilType.GetMethod("DrawListField", BindingFlags.NonPublic | BindingFlags.Static);
      }
      return this._drawListFieldMethod;
    }


    // PRAGMA MARK - Public Internal
		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      // Draw label and modify contentRect
      contentRect = EditorGUI.PrefixLabel(contentRect, GUIUtility.GetControlID(FocusType.Passive), label);

      EditorGUI.indentLevel--;
			if (property.propertyType == SerializedPropertyType.Integer) {
        MethodInfo drawListMethod = this.GetDrawListFieldMethod();
        if (drawListMethod != null) {
          drawListMethod.Invoke(null, new object[] { contentRect, property });
        }
			} else {
        EditorGUI.LabelField(contentRect, "Use IdListDrawer with int types!");
      }
      EditorGUI.indentLevel++;
		}

    private class IdListDrawerUtil<TEntity> where TEntity : DTEntity, new() {
      private static void DrawListField(Rect contentRect, SerializedProperty property) {
        IdList<TEntity> list = IdList<TEntity>.Instance;
        if (list == null) {
          EditorGUI.LabelField(contentRect, IdList<TEntity>.ListName() + " instance not found!");
        } else {
          List<string> displayedOptions = new List<string>();
          List<int> optionValues = new List<int>();
          foreach (TEntity obj in list) {
            IdComponent idComponent = obj.GetComponent<IdComponent>();

            displayedOptions.Add(string.Format("{0} - {1}", idComponent.id, IdListDrawerUtil<TEntity>.GetTitleForObject(obj)));
            optionValues.Add(idComponent.id);
          }
  				property.intValue = EditorGUI.IntPopup(contentRect, property.intValue, displayedOptions.ToArray(), optionValues.ToArray());
        }
      }

      private static string GetTitleForObject(TEntity obj) {
        EditorDisplayComponent editorDisplayComponent = obj.GetComponent<EditorDisplayComponent>();

        string title = "No EditorDisplayComponent";
        if (editorDisplayComponent != null) {
          title = Regex.Replace(editorDisplayComponent.title, @"\s+", "");
        }
        return title;
      }
    }
	}
}