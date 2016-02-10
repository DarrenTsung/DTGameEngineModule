using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  public class Item : IIdObject {
    // PRAGMA MARK - IIdObject Implementation
    public int Id {
      get { return this._itemId; }
    }


    // PRAGMA MARK - Internal
    [SerializeField]
    private int _itemId;
    [SerializeField]
    private Sprite _displaySprite;
	}
}