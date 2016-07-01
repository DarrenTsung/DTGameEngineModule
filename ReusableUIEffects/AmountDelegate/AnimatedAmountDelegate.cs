using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT {
  public class AnimatedAmountDelegate : AmountDelegate, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
    public override void SetAmount(int amount) {
      if (this._uninitialized) {
        // skip animation if uninitialized
        this.SetCurrentAmount(amount);
        this._uninitialized = false;
        return;
      }

      if (this._animationCoroutine != null) {
        this.CleanupAnimation();
        this.StopCoroutine(this._animationCoroutine);
        this._animationCoroutine = null;
      }

      if (this._skipAnimationWhenDecrementing && amount < this._currentAmount) {
        this.SetCurrentAmount(amount);
        return;
      }

      if (amount == this._currentAmount) {
        return;
      }

      this._animationCoroutine = this.StartCoroutine(this.AnimateToAmount(amount));
    }


    // PRAGMA MARK - IRecycleSetupSubscriber Implementation
    public void OnRecycleSetup() {
      this._cachedBaseColor = this._textOutlet.Color;
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      this._textOutlet.Color = this._cachedBaseColor;
      this._uninitialized = true;
    }


    // PRAGMA MARK - Internal
    [Header("Properties")]
    [SerializeField] private bool _animateColor = false;
    [SerializeField] private Color _colorWhileAnimating = Color.yellow;

    [Space]
    [SerializeField] private bool _skipAnimationWhenDecrementing = false;

    [Space]
    [SerializeField] private AnimationCurve _curve = AnimationCurveUtil.NormalizedEaseFrom0To1();
    [SerializeField] private float _animationDuration = 0.8f;

    [Header("Read-Only")]
    [SerializeField, ReadOnly] private int _currentAmount;
    [SerializeField, ReadOnly] private bool _uninitialized = true;

    [SerializeField, ReadOnly] private Color _cachedBaseColor;

    private Coroutine _animationCoroutine;

    private IEnumerator AnimateToAmount(int targetAmount) {
      if (this._animateColor) {
        this._textOutlet.Color = this._colorWhileAnimating;
      }

      int startAmount = this._currentAmount;

      for (float t = 0.0f; t <= this._animationDuration; t += Time.deltaTime) {
        float normalizedT = t / this._animationDuration;

        float percent = this._curve.Evaluate(normalizedT);
        int computedAmount = (int)((float)startAmount + ((targetAmount - startAmount) * percent));
        this.SetCurrentAmount(computedAmount);
        yield return null;
      }

      this.SetCurrentAmount(targetAmount);
      this.CleanupAnimation();
    }

    private void CleanupAnimation() {
      if (this._animateColor) {
        this._textOutlet.Color = this._cachedBaseColor;
      }
    }

    private void SetCurrentAmount(int amount) {
      this._currentAmount = amount;
      this.SetTextOutletWithAmount(this._currentAmount);
    }
  }
}