using DT;
using System;
﻿using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class DisplayComponent : IDTComponent {
    // PRAGMA MARK - Public Interface
    public Sprite displaySprite;
    public string displayName;
	}
}