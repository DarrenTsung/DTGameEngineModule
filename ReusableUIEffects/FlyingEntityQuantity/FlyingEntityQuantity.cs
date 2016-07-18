using DT;
using System;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  // TODO (darren): rename class in Unity so GUIDs don't get lost
  public class FlyingEntityQuantity : MonoBehaviour, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Static Public Interface
    public static float kAnimationDuration = 1.2f;

    public static void Make(IIdQuantity idQuantity, Vector2 screenPosition, GameObject recycleParent, Action finishedCallback) {
      RecyclablePrefab parentRecyclable = recycleParent.GetRequiredComponentInParent<RecyclablePrefab>();
      if (parentRecyclable == null) {
        // don't need to log since using required component
        return;
      }

      FlyingEntityQuantity f = ObjectPoolManager.Instantiate<FlyingEntityQuantity>("FlyingEntityQuantity");
      ViewManagerLocator.Main.AttachView(f.gameObject);

      parentRecyclable.AttachChildRecyclableObject(f.gameObject);

      f.transform.position = screenPosition;
      f.Configure(idQuantity, finishedCallback);
    }


    // PRAGMA MARK - Public Interface
    public void Configure(IIdQuantity idQuantity, Action finishedCallback) {
      this._finishedCallback = finishedCallback;
      this._finishedCallbackInvoked = false;

      string key = FlyingEntityTarget.KeyForTypeId(idQuantity.EntityType, idQuantity.Id);
      RectTransform targetTransform = FlyingEntityTarget.targets.GetValue(key);
      if (targetTransform == null) {
        Debug.LogError("Failed to find target for key: " + key + "!");
        ObjectPoolManager.Recycle(this.gameObject);
        return;
      }

      this._displayImage.sprite = idQuantity.Entity.DisplaySprite();
      this._quantityText.Text = idQuantity.Quantity.ToString();

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

    [Space]
    [SerializeField] private TweenController _tweenController;

    private Action _finishedCallback;
    private bool _finishedCallbackInvoked = false;

    private void AnimateToTarget(Vector2 targetPosition) {
      Vector2 startPosition = this.transform.position;

      QuadBezierV2 bezier = new QuadBezierV2(startPosition, targetPosition, axis: Axis.HORIZONTAL);
      this.DoEaseEveryFrameForDuration(FlyingEntityQuantity.kAnimationDuration, EaseType.QuadIn, (float percentage) => {
        Vector2 position = bezier.Evaluate(percentage);
        this.transform.position = position;

        if (this._tweenController != null) {
          this._tweenController.Value = percentage;
        }
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