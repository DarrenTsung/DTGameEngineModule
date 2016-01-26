using DT;
using System;
using System.Collections;
﻿using UnityEngine;
﻿using UnityEngine.UI;

namespace DT.GameEngine {
	public class FPSView : BasicView<FPSViewController> {
		// PRAGMA MARK - Public Interface
		public override void Show() {
      this._container.SetActive(true);
			base.Show();
		}

		public override void Dismiss() {
      this._container.SetActive(false);
			base.Dismiss();
		}

    public void HandleFpsUpdate(float newFps) {
      this._fpsText.text = "FPS: " + newFps.ToString("0.0");
    }


		// PRAGMA MARK - Internal
    [SerializeField] private GameObject _container;
		[SerializeField] private Text _fpsText;
	}
}