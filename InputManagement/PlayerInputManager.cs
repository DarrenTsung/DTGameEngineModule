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
		CONTROLLER
	}
	
	public class PlayerInputManager<TPlayerActions> : MonoBehaviour, IPlayerInputManager where TPlayerActions : PlayerActions, new() {
		protected PlayerInputManager() {}
		
		
		// PRAGMA MARK - Public Interface
		public bool InputDisabled {
			get { return _inputDisabled; }
			set {
				_inputDisabled = value;
			}
		}
		
		public void BindDeviceToUnusedPlayerIndex(InputDevice device) {
			if (this.IsDeviceBoundToAPlayerIndex(device)) {
				// nothing if device is already mapped
				return;
			}
			
			int playerIndex = this.FindUnusedPlayerIndex();
			this.BindDeviceWithPlayerIndex(device, playerIndex);
		}
		
		public bool IsDeviceBoundToAPlayerIndex(InputDevice device) {
			return _playerMapping.ContainsKey(device);
		}
		
		public int[] UsedPlayerIndexes() {
			return _playerMapping.Values.ToArray();
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
		public UnityEvents.V2 GetPrimaryDirectionEvent(int playerIndex) {
			return _playerPrimaryDirectionEvents.GetAndCreateIfNotFound(playerIndex);
		}
		
		public UnityEvents.V2 GetSecondaryDirectionEvent(int playerIndex) {
			return _playerSecondaryDirectionEvents.GetAndCreateIfNotFound(playerIndex);
		}
		
		protected Dictionary<int, UnityEvents.V2> _playerPrimaryDirectionEvents = new Dictionary<int, UnityEvents.V2>();
		protected Dictionary<int, UnityEvents.V2> _playerSecondaryDirectionEvents = new Dictionary<int, UnityEvents.V2>();
		
		
		// PRAGMA MARK - Internal
		[Header("---- PlayerInputManager Properties ----")]
		[SerializeField]
		protected bool _inputDisabled = false;
		[SerializeField]
		protected bool _logDebugInfo = false;
		
		[Header("---- Player One Properties (all other players default to controller) ----")]
		[SerializeField]
		protected PlayerInputType _playerOneInputType = PlayerInputType.CONTROLLER;
		[SerializeField]
		protected Vector3 _playerOneMouseInputPlaneNormal = new Vector3(0.0f, 1.0f, 0.0f);
		
		protected Dictionary<InputDevice, int> _playerMapping;
		protected Dictionary<int, TPlayerActions> _playerActions;
		
		protected GameObject _playerOne;
		
		protected virtual void Awake() {
			_playerMapping = new Dictionary<InputDevice, int>();
			_playerActions = new Dictionary<int, TPlayerActions>();
			
			DTGameEngineNotifications.PlayerChanged.AddListener(this.HandlePlayerChanged);
			InputManager.OnDeviceAttached += this.OnDeviceAttached;
			InputManager.OnDeviceDetached += this.OnDeviceDetached;
		}
		
		protected virtual void Update() {
			if (!this.InputDisabled) {
				this.UpdateInput();
			}
		}
		
		protected void HandlePlayerChanged(int playerIndex, GameObject player) {
			// always re-enable player input when player changes
			this.EnableInputForPlayer(playerIndex);
			
			if (_playerMapping.ContainsValue(playerIndex)) {
				// no need to do anything if this player has already been registered
				return;
			}
			
			InputDevice unusedDevice = this.FindUnusedDevice();
			if (unusedDevice) {
				this.BindDeviceWithPlayerIndex(unusedDevice, playerIndex);
			} else {
				Debug.LogWarning("Player changed (" + playerIndex + ") and had no mapping - failed to find an unused device!");
			}
		}
		
		protected void BindDeviceWithPlayerIndex(InputDevice device, int playerIndex) {
			TPlayerActions actions = new TPlayerActions();
			actions.Device = device;
			
			PlayerInputType type = (playerIndex == 0) ? _playerOneInputType : PlayerInputType.CONTROLLER;
			actions.BindWithInputType(type);
			
			_playerActions[playerIndex] = actions;
			_playerMapping[device] = playerIndex;
			
			this.LogIfDebugEnabled("Registered player " + playerIndex + " (type: " + type + ") with device: (" + device.Name + ")");
		}
		
		protected int FindUnusedPlayerIndex() {
			for (int i = 0;; i++) {
				if (!_playerActions.ContainsKey(i)) {
					return i;
				}
			}
		}
		
		protected void LogIfDebugEnabled(string logString) {
			if (_logDebugInfo) {
				Debug.Log(logString);
			}
		}
		
		
		// PRAGMA MARK - Device Handling
		protected void OnDeviceAttached(InputDevice device) {
			this.LogIfDebugEnabled("Device (" + device.Name + ") attached! Iterating through players to see if any players could use a controller");
			foreach (KeyValuePair<int, TPlayerActions> pair in _playerActions) {
				int playerIndex = pair.Key;
				TPlayerActions actions = pair.Value;
				if (!actions.Device.IsAttached) {
					actions.Device = device;
					_playerMapping[device] = playerIndex;
					this.LogIfDebugEnabled("Device attached to player " + pair.Key + "!");
					break;
				}
			}
		}
		
		protected void OnDeviceDetached(InputDevice device) {
			if (_playerMapping.ContainsKey(device)) {
				int playerIndex = -1;
				foreach (KeyValuePair<int, TPlayerActions> pair in _playerActions) {
					if (pair.Value.Device == device) {
						playerIndex = pair.Key;
					}
				}
				this.LogIfDebugEnabled("Device detached (" + device.Name + ") - player " + playerIndex + " is not connected anymore!");
				_playerMapping.Remove(device);
			} else {
				this.LogIfDebugEnabled("Device detached (" + device.Name + ") - no player was mapped.");
			}
		}
		
		protected InputDevice FindUnusedDevice() {
			foreach (InputDevice device in InputManager.Devices) {
				if (!_playerMapping.ContainsKey(device)) {
					return device;
					}
			}
			return null;
		}
		
		
		// PRAGMA MARK - Updating Input
		protected virtual void UpdateInput() {
			foreach (KeyValuePair<int, TPlayerActions> entry in _playerActions) {
				TPlayerActions actions = entry.Value;
				int playerIndex = entry.Key;
				if (actions.Device.IsAttached && !this.IsInputDisabledForPlayer(playerIndex)) {
					this.UpdateInputForPlayer(playerIndex, actions);
				}
			}
		}
		
		protected virtual void UpdateInputForPlayer(int playerIndex, TPlayerActions actions) {
			Vector2 primaryDirection = this.GetPrimaryDirection(playerIndex, actions);
			DTGameEngineNotifications.HandlePrimaryDirection.Invoke(playerIndex, primaryDirection);
			this.GetPrimaryDirectionEvent(playerIndex).Invoke(primaryDirection);
		
			Vector2 secondaryDirection = this.GetSecondaryDirection(playerIndex, actions);
			DTGameEngineNotifications.HandleSecondaryDirection.Invoke(playerIndex, secondaryDirection);
			this.GetSecondaryDirectionEvent(playerIndex).Invoke(secondaryDirection);
			
			// player one specific
			if (playerIndex == 0 && _playerOneInputType == PlayerInputType.MOUSE_AND_KEYBOARD) {
				Vector2 mouseScreenPosition = Input.mousePosition;
				DTGameEngineNotifications.HandleMouseScreenPosition.Invoke(mouseScreenPosition);
			}
		}
		
		protected virtual void UpdateInputForButton(ButtonEventGroup eventGroup, PlayerAction buttonAction) {
			if (buttonAction.WasPressed) {
				eventGroup.StartPressed.Invoke();
			} else if (buttonAction.IsPressed) {
				eventGroup.BeingPressed.Invoke();
			} else if (buttonAction.WasReleased) {
				eventGroup.WasReleased.Invoke();
			}
		}
		
		protected virtual Vector2 GetPrimaryDirection(int playerIndex, TPlayerActions actions) {
			Vector2 primaryDirection = actions.PrimaryDirection.Value;
			return primaryDirection;
		}
		
		protected virtual Vector2 GetSecondaryDirection(int playerIndex, TPlayerActions actions) {
			Vector2 secondaryDirection = actions.SecondaryDirection.Value;
			// player one specific
			if (playerIndex == 0 && _playerOneInputType == PlayerInputType.MOUSE_AND_KEYBOARD) {
				Plane playerMouseInputPlane = new Plane(_playerOneMouseInputPlaneNormal, _playerOne.transform.position);
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				float rayDistance;
				
				if (playerMouseInputPlane.Raycast(mouseRay, out rayDistance)) {
					Vector3 mouseVector = mouseRay.GetPoint(rayDistance) - _playerOne.transform.position;
					// use this quaternion to convert the line into the xy plane 
					Quaternion normalRotation = Quaternion.FromToRotation(_playerOneMouseInputPlaneNormal, Vector3.back);
					Vector3 xyPlaneVector = normalRotation * mouseVector;
					secondaryDirection = xyPlaneVector;
				}
			}
			return secondaryDirection;
		}
	}
}
#endif
