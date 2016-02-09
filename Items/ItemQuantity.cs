using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class ItemQuantity {
    public int itemId;
    public int quantity;

    public ItemQuantity(int itemId, int quantity) {
      this.itemId = itemId;
      this.quantity = quantity;
    }
	}
}