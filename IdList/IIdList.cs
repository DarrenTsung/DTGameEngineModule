using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IIdList<TObject> : IEnumerable<TObject> where TObject : IIdObject {
    TObject LoadById(int id);

#if UNITY_EDITOR
    void Add(TObject newObj);
    void RemoveAt(int index);
    void SaveChanges();
#endif
  }
}