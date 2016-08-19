using DT;
using System;
using System.Collections;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public class FPSView : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
    private const float kUpdateRate = 4.0f;

    // PRAGMA MARK - IRecycleSetupSubscriber Implementation
    public void OnRecycleSetup() {
      this._updateFpsCoroutine = CoroutineWrapper.StartCoroutine(this.UpdateFPS());
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      if (this._updateFpsCoroutine != null) {
        this._updateFpsCoroutine.Stop();
        this._updateFpsCoroutine = null;
      }
    }


		// PRAGMA MARK - Internal
    [SerializeField] private GameObject _container;
		[SerializeField] private Text _fpsText;

    private CoroutineWrapper _updateFpsCoroutine;

    private IEnumerator UpdateFPS() {
      int frameCount = 0;
      float dt = 0.0f;
      float fps = 0.0f;

      while (true) {
        frameCount++;
        dt += Time.unscaledDeltaTime;
        if (dt > 1.0f / kUpdateRate) {
          fps = frameCount / dt ;
          frameCount = 0;
          dt -= 1.0f / kUpdateRate;
        }

        this.UpdateFpsText(fps);
        yield return null;
      }
    }

    private void UpdateFpsText(float newFps) {
      this._fpsText.text = "FPS: " + newFps.ToString("0.0");
    }
	}
}