using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public class GameInit : MonoBehaviour {
		// PRAGMA MARK - Internal
		protected void Start() {
			this.InitializeGame();
      this.DoAfterDelay(0.0f, () => {
        this.DelayedInitializeGame();
      });
		}

		protected virtual void InitializeGame() {
			// do nothing for now
		}

    protected virtual void DelayedInitializeGame() {
      // do nothing for now
    }
	}
}