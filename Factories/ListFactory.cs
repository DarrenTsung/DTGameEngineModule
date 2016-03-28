using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class ListFactory<TEntity> : IListFactory<TEntity> where TEntity : DTEntity {
    // PRAGMA MARK - Static
    private static IListFactory<TEntity> _instance = new ListFactory<TEntity>();

    public static IListFactory<TEntity> Instance {
      get {
        return ListFactory<TEntity>._instance;
      }
      set {
        ListFactory<TEntity>._instance = value;
      }
    }


    // PRAGMA MARK - IListFactory<TEntity> Implementation
    public IIdList<TEntity> GetList() {
      return null;
    }
	}
}
