using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class IdQuantity<TEntity> where TEntity : DTEntity {
    [Id]
    public int id;
    public int quantity;

    public IdQuantity() {
      this.id = 0;
      this.quantity = 1;
    }

    public IdQuantity(int id, int quantity = 1) {
      this.id = id;
      this.quantity = quantity;
    }
	}
}