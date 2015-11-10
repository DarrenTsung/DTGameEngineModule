using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface ILevel {
    int LevelId {
      get;
    }
    
  	GameObject SpawnPlayerFromTemplate(int playerIndex, GameObject template);
  }
}
