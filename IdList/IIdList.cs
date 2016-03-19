using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface IIdList {
#if UNITY_EDITOR
    void AddNew();
    void RemoveAt(int index);
#endif
  }
}