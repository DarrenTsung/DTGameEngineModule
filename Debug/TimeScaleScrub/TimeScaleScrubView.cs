using UnityEngine;

namespace DT.GameEngine {
  public class TimeScaleScrubView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
    // PRAGMA MARK - IRecycleSetupSubscriber Implementation
    public void OnRecycleSetup() {
      MonoBehaviourHelper.OnUpdate += this.HandleUpdate;
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      MonoBehaviourHelper.OnUpdate -= this.HandleUpdate;
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField] private TextOutlet _timeScaleText;
    [SerializeField] private CanvasGroup _canvasGroup;

    void OnDestroy() {
      MonoBehaviourHelper.OnUpdate -= this.HandleUpdate;
    }

    private void HandleUpdate() {
      float timeScale = Time.timeScale;

      if (!Mathf.Approximately(timeScale, 1.0f)) {
        this._canvasGroup.alpha = Mathf.Clamp(Mathf.Abs(timeScale - 1.0f), 0.0f, 1.0f);
        this._timeScaleText.Text = string.Format("x{0:0.#}", timeScale);
      } else {
        this._canvasGroup.alpha = 0.0f;
      }
    }
  }
}