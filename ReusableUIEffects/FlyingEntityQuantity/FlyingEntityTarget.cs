using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class FlyingEntityTarget : MonoBehaviour {
    // PRAGMA MARK - Static Public Interface
    public static StringRegistry<RectTransform> targets = new StringRegistry<RectTransform>();

    public static string KeyForTypeId(Type entityType, int id) {
      return string.Format("FlyingEntityTarget{0}{1}", entityType.Name, id);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private EntityTypeId _entityTypeId;

    void Awake() {
      string key = FlyingEntityTarget.KeyForTypeId(this._entityTypeId.EntityType(), this._entityTypeId.id);
      FlyingEntityTarget.targets.Register(key, this.GetRequiredComponent<RectTransform>());
    }
	}
}