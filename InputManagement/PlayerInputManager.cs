using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

#if IN_CONTROL
using InControl;

namespace DT.GameEngine {
	public enum PlayerInputType {
		MOUSE_AND_KEYBOARD,
		CONTROLLER
	}
	
	public class PlayerInputManager<TPlayerActions> : Singleton<PlayerInputManager<TPlayerActions>> where TPlayerActions : PlayerActions, new() {
		protected PlayerInputManager() {}
		
		// PRAGMA MARK - INTERFACE
		public bool InputDisabled {
			get { return _inputDisabled; }
			set {
				_inputDisabled = value;
			}
		}
		
		// PRAGMA MARK - INTERNAL
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
			
			DTNotifications.PlayerChanged.AddListener(this.HandlePlayerChanged);
			InputManager.OnDeviceAttached += this.OnDeviceAttached;
			InputManager.OnDeviceDetached += this.OnDeviceDetached;
		}
		
		protected void HandlePlayerChanged(int playerIndex, GameObject player) {
			if (_playerMapping.ContainsValue(playerIndex)) {
				// no need to do anything if this player has already been registered
				return;
			}
			
			InputDevice unusedDevice = this.FindUnusedDevice();
			if (unusedDevice) {
				TPlayerActions actions = new TPlayerActions();
				actions.Device = unusedDevice;
				
				PlayerInputType type = (playerIndex == 0) ? _playerOneInputType : PlayerInputType.CONTROLLER;
				actions.BindWithInputType(type);
				
				this.LogIfDebugEnabled("Registered player " + playerIndex + " (type: " + type + ") with device: (" + unusedDevice.Name + ")");
				
				_playerActions[playerIndex] = actions;
				_playerMapping[unusedDevice] = playerIndex;
			} else {
				Debug.LogWarning("Attempted to register player " + playerIndex + ", but failed to find an unused device!");
			}
		}
		
		protected void OnDeviceAttached(InputDevice device) {
			this.LogIfDebugEnabled("Device (" + device.Name + ") attached! Iterating through players to see if any players could use a controller");
			foreach (KeyValuePair<int, TPlayerActions> pair in _playerActions) {
				TPlayerActions actions = pair.Value;
				if (!actions.Device.IsAttached) {
					actions.Device = device;
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
		
		protected void LogIfDebugEnabled(string logString) {
			if (_logDebugInfo) {
				Debug.Log(logString);
			}
		}
		
		#region mark - Updating Input

		protected virtual void Update() {
			if (!this.InputDisabled) {
				this.UpdateInput();
			}
		}
		
		protected virtual void UpdateInput() {
			foreach (KeyValuePair<int, TPlayerActions> entry in _playerActions) {
				TPlayerActions actions = entry.Value;
				if (actions.Device.IsAttached) {
					this.UpdateInputForPlayer(entry.Key, actions);
				}
			}
		}
		
		protected virtual void UpdateInputForPlayer(int playerIndex, TPlayerActions actions) {
			Vector2 primaryDirection = this.GetPrimaryDirection(playerIndex, actions);
			DTNotifications.HandlePrimaryDirection.Invoke(playerIndex, primaryDirection);
		
			Vector2 secondaryDirection = this.GetSecondaryDirection(playerIndex, actions);
			DTNotifications.HandleSecondaryDirection.Invoke(playerIndex, secondaryDirection);
			
			// player one specific
			if (playerIndex == 0 && _playerOneInputType == PlayerInputType.MOUSE_AND_KEYBOARD) {
				Vector2 mouseScreenPosition = Input.mousePosition;
				DTNotifications.HandleMouseScreenPosition.Invoke(mouseScreenPosition);
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
		
		#endregion
	}
}
#endif
