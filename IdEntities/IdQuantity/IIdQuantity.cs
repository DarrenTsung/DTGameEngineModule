using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IIdQuantity {
    IUserIdInventory UserInventory { get; }

    bool CanRemoveFromUserInventory();
    void RemoveFromUserInventory();

    void AddToUserInventory();

    DTEntity Entity { get; }
    Type EntityType { get; }

    int Id { get; }
    int Quantity { get; }
	}
}