using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public class FlyingEntityQuantity : MonoBehaviour, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Static Public Interface
    public static float kAnimationDuration = 1.5f;

    public static void Make(EntityQuantity entityQuantity, Vector2 screenPosition, GameObject recycleParent, Action finishedCallback) {
      RecyclablePrefab parentRecyclable = recycleParent.GetRequiredComponentInParent<RecyclablePrefab>();
      if (parentRecyclable == null) {
        // don't need to log since using required component
        return;
      }

      FlyingEntityQuantity f = ObjectPoolManager.Instantiate<FlyingEntityQuantity>("FlyingEntityQuantity");
      ViewManagerLocator.Main.AttachView(f.gameObject);

      parentRecyclable.AttachChildRecyclableObject(f.gameObject);

      f.transform.position = screenPosition;
      f.Configure(entityQuantity, finishedCallback);
    }


    // PRAGMA MARK - Public Interface
    public void Configure(EntityQuantity entityQuantity, Action finishedCallback) {
      this._entityQuantity = entityQuantity;
      this._finishedCallback = finishedCallback;
      this._finishedCallbackInvoked = false;

      string key = FlyingEntityTarget.KeyForTypeId(entityQuantity.entityType, entityQuantity.entity.Id());
      RectTransform targetTransform = FlyingEntityTarget.targets.GetValue(key);
      if (targetTransform == null) {
        Debug.LogError("Failed to find target for key: " + key + "!");
        ObjectPoolManager.Recycle(this.gameObject);
        return;
      }

      this._displayImage.sprite = this._entityQuantity.entity.DisplaySprite();
      this._quantityText.Text = this._entityQuantity.quantity.ToString();

      this.AnimateToTarget(targetTransform.position);
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      this.InvokeFinishedCallbackIfNecessary();
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField] private Image _displayImage;
    [SerializeField] private TextOutlet _quantityText;

    private Action _finishedCallback;
    private bool _finishedCallbackInvoked = false;

    private EntityQuantity _entityQuantity;

    private void AnimateToTarget(Vector2 targetPosition) {
      Vector2 startPosition = this.transform.position;

      QuadBezierV2 bezier = new QuadBezierV2(startPosition, targetPosition, axis: Axis.HORIZONTAL);
      this.DoEaseEveryFrameForDuration(FlyingEntityQuantity.kAnimationDuration, EaseType.SineIn, (float percentage) => {
        Vector2 position = bezier.Evaluate(percentage);
        this.transform.position = position;
      }, finishedCallback: () => {
        this.InvokeFinishedCallbackIfNecessary();
        ObjectPoolManager.Recycle(this.gameObject);
      });
    }

    private void InvokeFinishedCallbackIfNecessary() {
      if (!this._finishedCallbackInvoked) {
        this._finishedCallback.Invoke();
        this._finishedCallbackInvoked = true;
      }
    }
	}
}