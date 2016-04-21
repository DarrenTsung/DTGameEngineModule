using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public static class IdQuantityExtensions {
    public static TEntity GetEntity<TEntity>(this IdQuantity<TEntity> idQuantity) where TEntity : DTEntity {
      return IdList<TEntity>.Instance.LoadById(idQuantity.id);
    }
	}
}