using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class Item : IIdObject {
    // PRAGMA MARK - IIdObject Implementation
    public int Id {
      get { return this.itemId; }
    }

    public int itemId;
    public string notes;

    [Header("Display")]
    public string displayName;
    public Sprite displaySprite;
    public Sprite secondaryDisplaySprite;
    public int testHello;
	}
}