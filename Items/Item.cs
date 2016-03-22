using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class Item : DTEntity {
    public IdComponent idComponent = new IdComponent();

#if UNITY_EDITOR
    public EditorDisplayComponent editorDisplayComponent = new EditorDisplayComponent();
#endif
	}
}