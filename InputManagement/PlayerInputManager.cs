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

		public bool IsDeviceBoundToAPlayerIndex(InputDevice device) {
      foreach (KeyValuePair<int, TPlayerActionSet> pair in this._playerActionsMapping) {
        if (pair.Value.Device == device) {
          return true;
        }
      }
      return false;
		}

		protected Dictionary<int, bool> _playerInputDisabledMapping = new Dictionary<int, bool>();
		protected Dictionary<InputDevice, int> _playerMapping = new Dictionary<InputDevice, int>();


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
			return _playerActionsMapping.GetAndCreateIfNotFound(playerIndex);
		}

		protected void HandlePlayerChanged(int playerIndex, GameObject player) {
			// always re-enable player input when player changes
			this.EnableInputForPlayer(playerIndex);

      TPlayerActionSet playerActions = this.ActionsForPlayerIndex(playerIndex);
      if (playerActions.Device == null) {
  			InputDevice unusedDevice = this.FindUnusedDevice();
  			if (unusedDevice) {
          this.BindDeviceToPlayerIndex(unusedDevice, playerIndex);
  			} else {
  				this.LogIfDebugEnabled("Player changed (" + playerIndex + ") and had no device - failed to find an unused device!");
  			}
      }
		}

    protected virtual void BindDeviceToPlayerIndex(InputDevice device, int playerIndex) {
      TPlayerActionSet playerActions = this.ActionsForPlayerIndex(playerIndex);
      playerActions.Device = device;
			this.LogIfDebugEnabled("Registered player " + playerIndex + " with device: (" + device.Name + ")");
    }

		protected InputDevice FindUnusedDevice() {
			foreach (InputDevice device in InputManager.Devices) {
        if (!this.IsDeviceBoundToAPlayerIndex(device)) {
          return device;
        }
			}
			return null;
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

		protected virtual void UpdateInputForButton(ButtonEventGroup eventGroup, PlayerAction buttonAction) {
			if (buttonAction.WasPressed) {
				eventGroup.StartPressed.Invoke();
			} else if (buttonAction.IsPressed) {
				eventGroup.BeingPressed.Invoke();
			} else if (buttonAction.WasReleased) {
				eventGroup.WasReleased.Invoke();
			}
		}
	}
}
#endif
