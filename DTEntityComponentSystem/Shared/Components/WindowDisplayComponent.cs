using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class WindowDisplayComponent : IDTComponent {
    // PRAGMA MARK - Public Interface
    public Texture2D iconTexture;
    public string title;
	}
}