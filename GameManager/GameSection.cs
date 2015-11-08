using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public abstract class GameSection {
		// PRAGMA MARK - Interface
		public bool IsActive {
			get { return _isActive; }
		}
		
		public void Setup() {
			_isActive = true;
			this.InternalSetup();
		}
		
		public void Teardown() {
			_isActive = false;
			this.InternalTeardown();
		}
		
		// optional
		public virtual void Update() {}
		
		// PRAGMA MARK - Internal
		private bool _isActive;
		
		protected abstract void InternalSetup();
		protected abstract void InternalTeardown();
	}
}