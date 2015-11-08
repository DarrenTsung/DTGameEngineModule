using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
  public interface ILevel {
  	// PRAGMA MARK - Interface
  	GameObject SpawnPlayerFromTemplate(int playerIndex, GameObject template);
  }
}
