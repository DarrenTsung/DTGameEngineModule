using DT;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
	[CustomPropertyDrawer(typeof(ItemIdAttribute))]
	public class ItemIdDrawer : PropertyDrawer {
    private const int kIdFieldWidth = 100;
    private const int kPadding = 5;

		public override void OnGUI(Rect contentRect, SerializedProperty property, GUIContent label) {
      // Draw label and modify contentRect
      contentRect = EditorGUI.PrefixLabel(contentRect, GUIUtility.GetControlID(FocusType.Passive), label);

      EditorGUI.indentLevel--;
			if (property.propertyType == SerializedPropertyType.Integer) {
        ItemList itemList = Toolbox.GetInstance<ItemList>();
        if (itemList == null) {
          EditorGUI.LabelField(contentRect, "ItemList instance not found under Toolbox!");
        } else {
          List<string> displayedOptions = new List<string>();
          List<int> optionValues = new List<int>();
          foreach (Item item in itemList) {
            displayedOptions.Add(string.Format("{0} - {1}", item.itemId, item.displayName));
            optionValues.Add(item.itemId);
          }
  				property.intValue = EditorGUI.IntPopup(contentRect, property.intValue, displayedOptions.ToArray(), optionValues.ToArray());
        }
			} else {
        EditorGUI.LabelField(contentRect, "Use [ItemId] attribute with int types!");
      }
      EditorGUI.indentLevel++;
		}
	}
}