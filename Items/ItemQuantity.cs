using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class ItemQuantity {
    [Id(typeof(Item))]
    public int itemId;
    public int quantity;

    public ItemQuantity(int itemId, int quantity = 1) {
      this.itemId = itemId;
      this.quantity = quantity;
    }
	}
}