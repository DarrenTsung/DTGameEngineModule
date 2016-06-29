using DT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public static class IdListDrawerUtil {
    public static MethodInfo GetDrawListFieldMethod(Type type) {
      Type utilType = typeof(IdListDrawerUtil.ListDrawer<>).MakeGenericType(type);
      return utilType.GetMethod("DrawListField", BindingFlags.NonPublic | BindingFlags.Static);
    }

    private static class ListDrawer<TEntity> where TEntity : DTEntity {
      private static void DrawListField(SerializedProperty property) {
        IIdList<TEntity> list = ListFactory<TEntity>.Instance.GetList();
        if (list == null) {
          list = IdList<TEntity>.Instance;
        }

        if (list == null) {
          EditorGUILayout.LabelField(IdList<TEntity>.ListName() + " instance not found!");
        } else {
          List<string> displayedOptions = new List<string>();
          List<int> optionValues = new List<int>();
          foreach (TEntity obj in list) {
            IdComponent idComponent = obj.GetComponent<IdComponent>();

            displayedOptions.Add(string.Format("{0} - {1}", idComponent.id, ListDrawer<TEntity>.GetTitleForObject(obj)));
            optionValues.Add(idComponent.id);
          }
          property.intValue = EditorGUILayout.IntPopup(property.intValue, displayedOptions.ToArray(), optionValues.ToArray());
        }
      }

      private static string GetTitleForObject(TEntity obj) {
        EditorDisplayComponent editorDisplayComponent = obj.GetComponent<EditorDisplayComponent>();

        string title = "No EditorDisplayComponent";
        if (editorDisplayComponent != null) {
          if (!editorDisplayComponent.title.IsNullOrEmpty()) {
            title = Regex.Replace(editorDisplayComponent.title, @"\s+", "");
          } else {
            title = "Empty Title";
          }
        }
        return title;
      }
    }
  }
}