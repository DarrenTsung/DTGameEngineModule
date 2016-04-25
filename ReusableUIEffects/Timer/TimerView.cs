using DT;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public class TimerView : MonoBehaviour, IRecycleCleanupSubscriber, IRecycleSetupSubscriber {
    // PRAGMA MARK - Public Interface
    public void SetDataSource(ITimerDataSource dataSource) {
      this._dataSource = dataSource;
    }


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
    [SerializeField]
    private TextController _timerText;
    [SerializeField]
    private Slider _timerSlider;

    private ITimerDataSource _dataSource;

    private void HandleUpdate() {
      if (this._dataSource == null) {
        return;
      }

      float percentageComplete = this._dataSource.PercentageComplete();
      this._timerSlider.value = (1.0f - Mathf.Clamp(percentageComplete, 0.0f, 1.0f));
      this._timerText.Text = this._dataSource.TimerText();
    }
  }
}