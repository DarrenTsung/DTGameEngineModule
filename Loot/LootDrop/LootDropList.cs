using DT;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class LootDropList : IdList<LootDrop> {
    // PRAGMA MARK - Static
    public static LootDropList Instance {
      get {
        return IdListUtil<LootDropList>.Instance;
      }
    }
	}
}