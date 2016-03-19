using DT;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	public abstract class IdListDrawer<TEntity> : PropertyDrawer where TEntity : DTEntity, new() {
    private const int kIdFieldWidth = 100;
    private const int kPadding = 5;

		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      // Draw label and modify contentRect
      contentRect = EditorGUI.PrefixLabel(contentRect, GUIUtility.GetControlID(FocusType.Passive), label);

      EditorGUI.indentLevel--;
			if (property.propertyType == SerializedPropertyType.Integer) {
        IdList<TEntity> list = IdList<TEntity>.Instance;
        if (list == null) {
          EditorGUI.LabelField(contentRect, IdList<TEntity>.ListName() + " instance not found!");
        } else {
          List<string> displayedOptions = new List<string>();
          List<int> optionValues = new List<int>();
          foreach (TEntity obj in list) {
            IdComponent idComponent = obj.GetComponent<IdComponent>();

            displayedOptions.Add(string.Format("{0} - {1}", idComponent.id, this.GetTitleForObject(obj)));
            optionValues.Add(idComponent.id);
          }
  				property.intValue = EditorGUI.IntPopup(contentRect, property.intValue, displayedOptions.ToArray(), optionValues.ToArray());
        }
			} else {
        EditorGUI.LabelField(contentRect, "Use IdListDrawer with int types!");
      }
      EditorGUI.indentLevel++;
		}

    private string GetTitleForObject(TEntity obj) {
      WindowDisplayComponent windowDisplayObject = obj.GetComponent<WindowDisplayComponent>();

      string title = "No WindowDisplayComponent";
      if (windowDisplayObject != null) {
        title = Regex.Replace(windowDisplayObject.title, @"\s+", "");
      }
      return title;
    }
	}
}