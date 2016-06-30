using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public class FlyingEntityQuantity : MonoBehaviour {
    // PRAGMA MARK - Static Public Interface
    public static float kAnimationDuration = 2.0f;

    public static void Make(EntityQuantity entityQuantity, Vector2 screenPosition, Action finishedCallback) {
      FlyingEntityQuantity f = ObjectPoolManager.Instantiate<FlyingEntityQuantity>("FlyingEntityQuantity");
      CanvasUtil.ParentUIElementToCanvas(f.gameObject, CanvasUtil.ScreenSpaceMainCanvas);

      f.transform.position = screenPosition;
      f.Configure(entityQuantity, finishedCallback);
    }


    // PRAGMA MARK - Public Interface
    public void Configure(EntityQuantity entityQuantity, Action finishedCallback) {
      this._entityQuantity = entityQuantity;

      string key = FlyingEntityTarget.KeyForTypeId(entityQuantity.entityType, entityQuantity.entity.Id());
      RectTransform targetTransform = FlyingEntityTarget.targets.GetValue(key);
      if (targetTransform == null) {
        Debug.LogError("Failed to find target for key: " + key + "!");
        ObjectPoolManager.Recycle(this.gameObject);
        return;
      }

      this._displayImage.sprite = this._entityQuantity.entity.DisplaySprite();
      this._quantityText.Text = this._entityQuantity.quantity.ToString();

      this.AnimateToTarget(targetTransform.position, finishedCallback);
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField] private Image _displayImage;
    [SerializeField] private TextOutlet _quantityText;

    private EntityQuantity _entityQuantity;

    private void AnimateToTarget(Vector2 targetPosition, Action callback) {
      Vector2 startPosition = this.transform.position;

      CubicBezierV2 bezier = new CubicBezierV2(startPosition, targetPosition);
      this.DoEaseEveryFrameForDuration(FlyingEntityQuantity.kAnimationDuration, EaseType.SineIn, (float percentage) => {
        Vector2 position = bezier.Evaluate(percentage);
        this.transform.position = position;
      }, finishedCallback: () => {
        ObjectPoolManager.Recycle(this.gameObject);
        callback.Invoke();
      });
    }
	}
}