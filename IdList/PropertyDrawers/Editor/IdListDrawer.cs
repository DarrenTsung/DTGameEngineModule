using DT;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	public abstract class IdListDrawer<TList, TIdObject> : PropertyDrawer where TList : IdList<TIdObject>
                                                                        where TIdObject : IIdObject {
    private const int kIdFieldWidth = 100;
    private const int kPadding = 5;

		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      // Draw label and modify contentRect
      contentRect = EditorGUI.PrefixLabel(contentRect, GUIUtility.GetControlID(FocusType.Passive), label);

      EditorGUI.indentLevel--;
			if (property.propertyType == SerializedPropertyType.Integer) {
        TList list = IdListUtil<TList>.Instance;
        if (list == null) {
          EditorGUI.LabelField(contentRect, typeof(TList).Name + " instance not found!");
        } else {
          List<string> displayedOptions = new List<string>();
          List<int> optionValues = new List<int>();
          foreach (TIdObject obj in list) {
            displayedOptions.Add(string.Format("{0} - {1}", obj.Id, this.GetTitleForObject(obj)));
            optionValues.Add(obj.Id);
          }
  				property.intValue = EditorGUI.IntPopup(contentRect, property.intValue, displayedOptions.ToArray(), optionValues.ToArray());
        }
			} else {
        EditorGUI.LabelField(contentRect, "Use IdListDrawer with int types!");
      }
      EditorGUI.indentLevel++;
		}

    private string GetTitleForObject(TIdObject obj) {
      IIdListDisplayObject displayObject = obj as IIdListDisplayObject;

      string title = "Not IIdListDisplayObject";
      if (displayObject != null) {
        title = Regex.Replace(displayObject.Title, @"\s+", "");
      }
      return title;
    }
	}
}