using DT;
﻿using UnityEngine;

namespace DT.GameEngine {
  [System.Serializable]
  public class Item : DTEntity, IIdListDisplayObject {
    public IdComponent idComponent = new IdComponent();
#if UNITY_EDITOR
    public EditorDisplayComponent editorDisplayComponent = new EditorDisplayComponent();
#endif

    public string notes;

    [Header("Display")]
    public string displayName;
    public Sprite displaySprite;


    // PRAGMA MARK - IIdListDisplayObject Implementation
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