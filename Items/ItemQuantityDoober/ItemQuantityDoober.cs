using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class ItemQuantityDoober : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetupWithItemQuantity(ItemQuantity itemQuantity) {
      this._itemQuantity = itemQuantity;

      Item item = Toolbox.GetInstance<ItemList>().LoadById(this._itemQuantity.itemId);
      this._renderer.sprite = item.displaySprite;

      this._autoTapCoroutine = CoroutineWrapper.StartCoroutine(this.AutoTapCoroutine());
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private SpriteRenderer _renderer;

    private ItemQuantity _itemQuantity;
    private CoroutineWrapper _autoTapCoroutine;

    private IEnumerator AutoTapCoroutine() {
      yield return new WaitForSeconds(GameConstants.kDooberAutoTapDelay);
      this._autoTapCoroutine = null;
      this.HandleTap();
    }

    private void HandleTap() {
      if (this._autoTapCoroutine != null) {
        this._autoTapCoroutine.Stop();
      }

      // TODO (darren): finish this
    }
  }
}