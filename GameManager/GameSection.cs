using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.GameEngine {
	public abstract class GameSection {
		// PRAGMA MARK - Public Interface
		public bool IsActive {
			get { return _isActive; }
		}
		
		public void Setup() {
			_isActive = true;
			Debug.Log("Setup - " + this.GetType().Name);
			this.InternalSetup();
		}
		
		public void Teardown() {
			_isActive = false;
			Debug.Log("Teardown - " + this.GetType().Name);
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