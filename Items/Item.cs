using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class Item : IIdObject, IIdListWindowObject {
    // PRAGMA MARK - IIdObject Implementation
    public int Id {
      get { return this.itemId; }
    }

    public int itemId;
    public string notes;

    [Header("Display")]
    public string displayName;
    public Sprite displaySprite;


    // PRAGMA MARK - IIdListWindowObject Implementation
    [SerializeField]
    private Texture2D _iconTexture;

    public Texture2D IconTexture {
      get { return this._iconTexture; }
    }

    public string Title {
      get { return this.displayName; }
    }
	}
}