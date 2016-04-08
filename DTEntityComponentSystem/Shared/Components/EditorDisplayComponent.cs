#if UNITY_EDITOR
using DT;
using System;
﻿using UnityEngine;

namespace DT.GameEngine {
  [Serializable]
  public class EditorDisplayComponent : IDTComponent {
    // PRAGMA MARK - Public Interface
    [NonSerialized]
    public Texture2D iconTexture;
    public string title;
	}
}
#endif