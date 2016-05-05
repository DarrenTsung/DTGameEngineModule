using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IViewIdQuantity {
    event Action OnUserInventoryUpdated;

    int Id {
      get;
    }

    int Quantity {
      get;
    }

    int UserQuantity {
      get;
    }

    DTEntity Entity {
      get;
    }
	}
}