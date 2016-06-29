using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class FlyingEntityQuantity : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void Configure(EntityQuantity entityQuantity) {
      this._entityQuantity = entityQuantity;
    }


    // PRAGMA MARK - Internal
    private EntityQuantity _entityQuantity;
	}
}