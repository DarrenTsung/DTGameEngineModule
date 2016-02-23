using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public abstract class GameSection {
		// PRAGMA MARK - Public Interface
		public bool IsActive {
			get { return this._isActive; }
		}

		public void Setup() {
			this._isActive = true;
			this.InternalSetup();
		}

		public void Teardown() {
			this._isActive = false;
			this.InternalTeardown();
		}

		public void SetContext(GameManager context) {
			_context = context;
		}

		// optional
		public virtual void Update() {}


		// PRAGMA MARK - Internal
		protected GameManager _context;

		private bool _isActive;

		protected abstract void InternalSetup();
		protected abstract void InternalTeardown();
	}
}