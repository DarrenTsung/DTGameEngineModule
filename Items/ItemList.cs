using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class ItemList : IdList<Item> {
    // PRAGMA MARK - Static
    public static ItemList Instance {
      get {
        return IdListUtil<ItemList>.Instance;
      }
    }
	}
}