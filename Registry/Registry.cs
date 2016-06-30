using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class Registry<TKey, TStored> : Dictionary<TKey, TStored> {
    // PRAGMA MARK - Public Interface
    public void Register(TKey key, TStored obj) {
      if (this.ContainsKey(key)) {
        Debug.LogWarning("Overriding stored object for key: " + key);
      }
      this[key] = obj;
    }

    public TStored GetValue(TKey key) {
      if (!this.ContainsKey(key)) {
        Debug.LogError("Failed to find key: " + key + " in the registry!");
        return default(TStored);
      }

      return this[key];
    }
	}
}