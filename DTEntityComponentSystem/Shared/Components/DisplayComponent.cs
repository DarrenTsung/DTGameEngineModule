using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class DisplayComponent : IDTComponent {
    // PRAGMA MARK - Public Interface
    public Sprite displaySprite;
    public string displayName;
	}
}