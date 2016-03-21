using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class IdQuantityDoober : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetupWithViewIdQuantity(IViewIdQuantity viewIdQuantity) {
      this._viewIdQuantity = viewIdQuantity;

      DTEntity entity = this._viewIdQuantity.Entity;
      DisplayComponent displayComponent = entity.GetComponent<DisplayComponent>();
      if (displayComponent != null) {
        this._renderer.sprite = displayComponent.displaySprite;
      }

      this._autoTapCoroutine = CoroutineWrapper.StartCoroutine(this.AutoTapCoroutine());
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private SpriteRenderer _renderer;

    private IViewIdQuantity _viewIdQuantity;
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