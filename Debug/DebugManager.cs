using DT;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class DebugManager : Singleton<DebugManager> {
    // PRAGMA MARK - Static
    private const float kTouchOffsetMax = 100.0f;
    private const float kTimescaleScrubMax = 20.0f;
    private const float kTimescaleScrubMin = 0.1f;

    public static bool IsDebug {
      get {
        return Debug.isDebugBuild;
      }
    }

    private static float? _timeOffset;
    public static float TimeOffset {
      get {
        if (DebugManager._timeOffset == null) {
          DebugManager._timeOffset = PlayerPrefs.GetFloat("DebugManager::TimeOffset", defaultValue: 0.0f);
        }
        return DebugManager._timeOffset.Value;
      }
      set {
        DebugManager._timeOffset = value;
        PlayerPrefs.SetFloat("DebugManager::TimeOffset", DebugManager._timeOffset.Value);
      }
    }


    // PRAGMA MARK - Internal
    private GameObject _debugView;
    private GameObject _fpsView;

    void Awake() {
      if (!DebugManager.IsDebug) {
        this.enabled = false;
        return;
      }

      DebugLogger.Initialize();

      this._fpsView = ObjectPoolManager.Instantiate("FPSView");
      ViewManagerLocator.Main.AttachView(this._fpsView);

      this._debugView = ObjectPoolManager.Instantiate("DebugView");
      ViewManagerLocator.Main.AttachView(this._debugView);
      this._debugView.SetActive(false);

      CoroutineWrapper.StartCoroutine(this.UpdateToggleDebugView());
      CoroutineWrapper.StartCoroutine(this.UpdateTimescaleScrubbing());
    }

    private IEnumerator UpdateToggleDebugView() {
      int previousNumberOfTouches = 0;

      while (true) {
        int numberOfTouches = Input.touches.Length;
        if (numberOfTouches != previousNumberOfTouches) {
          if (Input.touches.Length == 3) {
            this._debugView.ToggleActive();
          }
        }

        if (Input.GetKeyDown(KeyCode.D)) {
          this._debugView.ToggleActive();
        }

        previousNumberOfTouches = numberOfTouches;
        yield return null;
      }
    }

    private IEnumerator UpdateTimescaleScrubbing() {
      int previousNumberOfTouches = 0;
      Vector2 scrubTouchStartingCenter = Vector2.zero;

      while (true) {
        float timeScaleScrub = 0.0f;

        if (Input.GetKey(KeyCode.E)) {
          timeScaleScrub = 1.0f;
        } else if (Input.GetKey(KeyCode.Q)) {
          timeScaleScrub = -1.0f;
        }

        int numberOfTouches = Input.touches.Length;
        if (numberOfTouches == 2) {
          Vector2 touchCenter = Input.touches.Average(t => t.position);
          if (numberOfTouches != previousNumberOfTouches) {
            // start scrubbing
            scrubTouchStartingCenter = touchCenter;
          }

          float yOffset = touchCenter.y - scrubTouchStartingCenter.y;
          timeScaleScrub = Mathf.Clamp(yOffset, -kTouchOffsetMax, kTouchOffsetMax) / kTouchOffsetMax;
        }

        float timeScale = 1.0f;
        if (timeScaleScrub > 0) {
          timeScale = Mathf.Lerp(1.0f, kTimescaleScrubMax, timeScaleScrub);
        } else if (timeScaleScrub < 0) {
          timeScale = Mathf.Lerp(1.0f, kTimescaleScrubMin, -timeScaleScrub);
        }

        Time.timeScale = timeScale;

        float timeScaleDifference = timeScale - 1.0f;
        DebugManager.TimeOffset += Time.unscaledDeltaTime * timeScaleDifference;

        previousNumberOfTouches = numberOfTouches;
        yield return null;
      }
    }
  }
}