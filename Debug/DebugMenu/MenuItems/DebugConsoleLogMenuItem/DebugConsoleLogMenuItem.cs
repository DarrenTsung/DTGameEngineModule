using DT;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugConsoleLogMenuItem : DebugMenuItem {
    // PRAGMA MARK - Public Interface
    public override string Name { get { return "Console Log"; } }
    public override string PrefabName { get { return "DebugLogger"; } }

    public override int Priority {
      get { return DebugMenuItem.kDefaultPriority + 100; }
    }
  }
}