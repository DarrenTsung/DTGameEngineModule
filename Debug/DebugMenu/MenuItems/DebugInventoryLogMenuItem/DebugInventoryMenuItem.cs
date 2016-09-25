using DT;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugInventoryMenuItem : DebugMenuItem {
    // PRAGMA MARK - Public Interface
    public override string Name { get { return "Inventory"; } }
    public override string PrefabName { get { return "DebugInventory"; } }
  }
}