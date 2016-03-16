using DT;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DT.GameEngine {
  public class LootDropListWindow: IdListWindow<LootDropList, LootDrop> {
    // PRAGMA MARK - Static
    [MenuItem("DarrenTsung/DataSources/LootDropList")]
    public static void ShowWindow() {
      LootDropListWindow window = EditorWindow.GetWindow(typeof(LootDropListWindow)) as LootDropListWindow;
      window.Show();
    }
  }
}
