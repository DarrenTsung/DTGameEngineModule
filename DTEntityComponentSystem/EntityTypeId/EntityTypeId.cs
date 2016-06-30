using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public struct EntityTypeId {
    public string typeName;
    public int id;
	}

  public static class EntityTypeIdExtensions {
    public static Type EntityType(this EntityTypeId e) {
      if (!DTEntityUtil.EntitySubclassesByName.ContainsKey(e.typeName)) {
        Debug.LogError("Failed to find type for " + e.typeName + " please fill out missing type!");
        return null;
      }

      return DTEntityUtil.EntitySubclassesByName[e.typeName];
    }
  }
}