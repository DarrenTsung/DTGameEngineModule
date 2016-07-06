using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IIdList {
    IEnumerable<int> Ids();
    DTEntity LoadById(int id);
  }

  public interface IIdList<TEntity> : IIdList, IEnumerable<TEntity> where TEntity : DTEntity {
    new TEntity LoadById(int id);

#if UNITY_EDITOR
    void RemoveAt(int index);
#endif
  }
}