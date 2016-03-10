using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	public abstract class IdListDrawer<TList, TIdObject> : PropertyDrawer where TList : IIdList<TIdObject>, new()
                                                                        where TIdObject : IIdObject {
    private const int kIdFieldWidth = 100;
    private const int kPadding = 5;

		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      // Draw label and modify contentRect
      contentRect = EditorGUI.PrefixLabel(contentRect, GUIUtility.GetControlID(FocusType.Passive), label);

      EditorGUI.indentLevel--;
			if (property.propertyType == SerializedPropertyType.Integer) {
        TList list = new TList();
        if (list == null) {
          EditorGUI.LabelField(contentRect, typeof(TList).Name + " instance not found!");
        } else {
          List<string> displayedOptions = new List<string>();
          List<int> optionValues = new List<int>();
          foreach (TIdObject obj in list) {
            displayedOptions.Add(string.Format("{0} - {1}", obj.Id, this.GetDisplayStringForObject(obj)));
            optionValues.Add(obj.Id);
          }
  				property.intValue = EditorGUI.IntPopup(contentRect, property.intValue, displayedOptions.ToArray(), optionValues.ToArray());
        }
			} else {
        EditorGUI.LabelField(contentRect, "Use IdListDrawer with int types!");
      }
      EditorGUI.indentLevel++;
		}

    protected abstract string GetDisplayStringForObject(TIdObject obj);
	}
}