using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DT.GameEngine {
  public class FloatingText : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Static
    public static void Create(string text, GameObject parent) {
      RecyclablePrefab parentRecyclable = parent.GetRequiredComponentInParent<RecyclablePrefab>();
      if (parentRecyclable == null) {
        // don't need to log since using required component
        return;
      }

      FloatingText floatingText = ObjectPoolManager.Instantiate<FloatingText>("FloatingText", parent);
      floatingText.SetupWithText(text);

      parentRecyclable.AttachChildRecyclableObject(floatingText.gameObject);
    }


    // PRAGMA MARK - Public Interface
    public void SetupWithText(string text) {
      this._text.Text = text;
      this.Animate();
    }


    // PRAGMA MARK - IRecycleSetupSubscriber Implementation
    public void OnRecycleSetup() {
      this._startLocalPosition = this.transform.localPosition;
      this._startColor = this._text.Color;
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      this.transform.localPosition = this._startLocalPosition;
      this._text.Color = this._startColor;
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private TextOutlet _text = new TextOutlet();

    [Header("Properties")]
    [SerializeField]
    private AnimationCurve _alphaCurve;
    [SerializeField]
    private AnimationCurve _yCurve;
    [SerializeField]
    private float _animationDuration = 0.7f;

    private Vector3 _startLocalPosition;
    private Color _startColor;

    private void Animate() {
      Vector3 startLocalPosition = this.transform.localPosition;
      Color startColor = this._text.Color;

      this.DoEveryFrameForDuration(this._animationDuration, (float time, float duration) => {
        float percentage = time / duration;
        float y = this._yCurve.Evaluate(percentage);
        float alpha = this._alphaCurve.Evaluate(percentage);

        this.transform.localPosition = startLocalPosition + new Vector3(0.0f, y, 0.0f);

        Color newColor = startColor;
        newColor.a = alpha;
        this._text.Color = newColor;
      }, finishedCallback: () => {
        ObjectPoolManager.Recycle(this.gameObject);

        RecyclablePrefab parentRecyclable = this.GetRequiredComponentInParent<RecyclablePrefab>();
        parentRecyclable.DettachChildRecyclableObject(this.gameObject);
      });
    }

    [MakeButton]
    private void SetDebugText(string text) {
      this.SetupWithText(text);
    }
  }
}