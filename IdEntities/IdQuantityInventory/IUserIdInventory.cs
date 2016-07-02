using DT;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace DT.GameEngine {
  public interface IUserIdInventory {
    // PRAGMA MARK - Public Interface
    event Action OnInventoryUpdated;

    int GetCountOfId(int id);
  }
}