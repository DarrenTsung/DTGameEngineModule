using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class Item : IIdObject, IIdListWindowObject {
    // PRAGMA MARK - IIdObject Implementation
    public int Id {
      get { return this.itemId; }
    }


    // PRAGMA MARK - IIdListWindowObject Implementation
    public Sprite DisplaySprite {
      get { return this.displaySprite; }
    }

    public string DisplayName {
      get { return this.displayName; }
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