using DT;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DT.GameEngine {
  public class ItemListWindow: IdListWindow<Item> {
    // PRAGMA MARK - Static
    [MenuItem("DarrenTsung/DataSources/ItemList")]
    public static void ShowWindow() {
      ItemListWindow window = EditorWindow.GetWindow(typeof(ItemListWindow)) as ItemListWindow;
      window.Show();
    }
  }
}
