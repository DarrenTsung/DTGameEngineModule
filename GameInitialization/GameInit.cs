using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public class GameInit : MonoBehaviour {
		// PRAGMA MARK - Internal
		protected void Start() {
			this.InitializeGame();
		}
		
		protected virtual void InitializeGame() {
			// do nothing for now
		}
	}
}