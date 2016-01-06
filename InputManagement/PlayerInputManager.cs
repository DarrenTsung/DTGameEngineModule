using DT;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;
﻿using UnityEngine.Events;

#if IN_CONTROL
using InControl;

namespace DT.GameEngine {
	public enum PlayerInputType {
		MOUSE_AND_KEYBOARD,
		CONTROLLER,
		TOUCH
	}
	
	public class PlayerInputManager<TPlayerActionSet> : MonoBehaviour, IPlayerInputManager where TPlayerActionSet : PlayerActionSet, new() {
		protected PlayerInputManager() {}
		
		
		// PRAGMA MARK - Public Interface
		public bool InputDisabled {
			get { return _inputDisabled; }
			set {
				_inputDisabled = value;
			}
		}
		
		public void DisableInputForPlayer(int playerIndex) {
			this.SetInputDisabledForPlayer(playerIndex, true);
		}
		
		public void EnableInputForPlayer(int playerIndex) {
			this.SetInputDisabledForPlayer(playerIndex, false);
		}
		
		public void SetInputDisabledForPlayer(int playerIndex, bool inputDisabled) {
			_playerInputDisabledMapping[playerIndex] = inputDisabled;
		}
		
		public bool IsInputDisabledForPlayer(int playerIndex) {
			return _playerInputDisabledMapping.SafeGet(playerIndex, false);
		}
		
		protected Dictionary<int, bool> _playerInputDisabledMapping = new Dictionary<int, bool>();
		
		
		// PRAGMA MARK - Input Unity Events
		//
		// Example:
		// public UnityEvents.V2 GetPrimaryDirectionEvent(int playerIndex) {
		// 	return _playerPrimaryDirectionEvents.GetAndCreateIfNotFound(playerIndex);
		// }
		// 
		// protected Dictionary<int, UnityEvents.V2> _playerPrimaryDirectionEvents = new Dictionary<int, UnityEvents.V2>();
		
		
		// PRAGMA MARK - Internal
		[Header("---- PlayerInputManager Properties ----")]
		[SerializeField]
		protected bool _inputDisabled = false;
		[SerializeField]
		protected bool _logDebugInfo = false;
		
		protected Dictionary<int, TPlayerActionSet> _playerActionsMapping = new Dictionary<int, TPlayerActionSet>();
		
		protected virtual void Awake() {
			DTGameEngineNotifications.PlayerChanged.AddListener(this.HandlePlayerChanged);
		}
		
		protected virtual void Update() {
			if (!this.InputDisabled) {
				this.UpdateInput();
			}
		}
		
		protected TPlayerActionSet ActionsForPlayerIndex(int playerIndex) {
			return _playerActionsMapping[playerIndex];
		}
		
		protected virtual void HandlePlayerChanged(int playerIndex, GameObject player) {
			// always re-enable player input when player changes
			this.EnableInputForPlayer(playerIndex);
		}
		
		protected void LogIfDebugEnabled(string logString) {
			if (_logDebugInfo) {
				Debug.Log(logString);
			}
		}
		
		
		// PRAGMA MARK - Updating Input
		protected virtual void UpdateInput() {
			foreach (KeyValuePair<int, TPlayerActionSet> entry in _playerActionsMapping) {
				TPlayerActionSet actions = entry.Value;
				int playerIndex = entry.Key;
				if (this.CanUpdateInputForPlayer(playerIndex)) {
					this.UpdateInputForPlayer(playerIndex, actions);
				}
			}
		}
		
		protected virtual bool CanUpdateInputForPlayer(int playerIndex) {
			if (this.IsInputDisabledForPlayer(playerIndex)) {
				return false;
			}
			
			return true;
		}
		
		protected virtual void UpdateInputForPlayer(int playerIndex, TPlayerActionSet actions) { }
	}
}
#endif
