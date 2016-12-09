using DT;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugUIRebuildMenuItem : DebugMenuItem {
    // PRAGMA MARK - Public Interface
    public override string Name { get { return "UI Rebuild"; } }
    public override string PrefabName { get { return "DebugUIRebuild"; } }
  }
}