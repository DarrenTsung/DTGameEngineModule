using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IViewIdQuantity {
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