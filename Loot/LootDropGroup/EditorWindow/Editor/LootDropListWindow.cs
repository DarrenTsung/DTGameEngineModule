using DT;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace DT.GameEngine {
  public class LootDropGroupListWindow: IdListWindow<LootDropGroupList, LootDropGroup> {
    // PRAGMA MARK - Static
    [MenuItem("DarrenTsung/DataSources/LootDropGroupList")]
    public static void ShowWindow() {
      LootDropGroupListWindow window = EditorWindow.GetWindow(typeof(LootDropGroupListWindow)) as LootDropGroupListWindow;
      window.Show();
    }
  }
}
