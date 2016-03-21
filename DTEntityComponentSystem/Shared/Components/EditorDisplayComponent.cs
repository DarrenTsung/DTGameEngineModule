#if UNITY_EDITOR
using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class EditorDisplayComponent : IDTComponent {
    // PRAGMA MARK - Public Interface
    public Texture2D iconTexture;
    public string title;
	}
}
#endif