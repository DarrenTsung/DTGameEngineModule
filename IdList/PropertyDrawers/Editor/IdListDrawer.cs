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
    private MethodInfo DrawListFieldMethod {
      get {
        if (this._drawListFieldMethod == null) {
          IdAttribute idAttribute = attribute as IdAttribute;

          Type utilType = typeof(IdListDrawer.IdListDrawerUtil<>).MakeGenericType(idAttribute.type);
          this._drawListFieldMethod = utilType.GetMethod("DrawListField", BindingFlags.NonPublic | BindingFlags.Static);
        }
        return this._drawListFieldMethod;
      }
    }


    // PRAGMA MARK - Public Internal
		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      // Draw label and modify contentRect
      contentRect = EditorGUI.PrefixLabel(contentRect, GUIUtility.GetControlID(FocusType.Passive), label);

      EditorGUI.indentLevel--;
			if (property.propertyType == SerializedPropertyType.Integer) {
        this.DrawListFieldMethod.Invoke(null, new object[] { contentRect, property });
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
        WindowDisplayComponent windowDisplayObject = obj.GetComponent<WindowDisplayComponent>();

        string title = "No WindowDisplayComponent";
        if (windowDisplayObject != null) {
          title = Regex.Replace(windowDisplayObject.title, @"\s+", "");
        }
        return title;
      }
    }
	}
}