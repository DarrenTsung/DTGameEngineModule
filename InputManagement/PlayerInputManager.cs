using DT;
using System.Collections;
﻿using UnityEngine;

#if IN_CONTROL
using InControl;

namespace DT {
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
		[Header("Properties")]
		[SerializeField]
		protected PlayerInputType _inputType;
		
		[Header("Primary Direction - Properties")]
		[SerializeField]
		protected bool _primaryDirectionEnabled = false;
		
		[Header("Secondary Direction - Properties")]
		[SerializeField]
		protected bool _secondaryDirectionEnabled = false;
		[SerializeField]
		protected Vector3 _playerMouseInputPlaneNormal;
		
		[Header("Mouse Position - Properties")]
		[SerializeField]
		protected bool _mousePositionEnabled = false;
		
		[Header("Read-Only")]
		[SerializeField, ReadOnly]
		protected bool _inputDisabled;
		
		protected TPlayerActions _playerActions;
		
		protected virtual void Awake() {
			// do nothing at the moment
		}
		
		protected virtual void Start() {
			this.SetupPlayerActions();
		}
		
		protected virtual void SetupPlayerActions() {
			_playerActions = new TPlayerActions();
			_playerActions.BindWithActions(_inputType);
		}

		protected virtual void Update() {
			if (_player != null && !this.InputDisabled) {
				this.UpdateInput();
			}
		}
		
		protected virtual void UpdateInput() {
			if (_primaryDirectionEnabled) {
				Vector2 primaryDirection = this.GetPrimaryDirection();
				NotificationsBase.HandlePrimaryDirection.Invoke(primaryDirection);
			}
			
			if (_secondaryDirectionEnabled) {
				Vector2 secondaryDirection = this.GetSecondaryDirection();
				NotificationsBase.HandleSecondaryDirection.Invoke(primaryDirection);
			}
			
			if (_inputType == PlayerInputType.MOUSE_AND_KEYBOARD && _mousePositionEnabled) {
				Vector2 mouseScreenPosition = Input.mousePosition;
				NotificationsBase.HandleMouseScreenPosition.Invoke(primaryDirection);
			}
		}
		
		protected virtual Vector2 GetPrimaryDirection() {
			Vector2 primaryDirection = _playerActions.PrimaryDirection.Value;
			return primaryDirection;
		}
		
		protected virtual Vector2 GetSecondaryDirection() {
			Vector2 secondaryDirection = _playerActions.SecondaryDirection.Value;
			if (_inputType == PlayerInputType.MOUSE_AND_KEYBOARD) {
				Plane playerMouseInputPlane = new Plane(_playerMouseInputPlaneNormal, _player.transform.position);
				Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
				float rayDistance;
				
				if (playerMouseInputPlane.Raycast(mouseRay, out rayDistance)) {
					Vector3 mouseVector = mouseRay.GetPoint(rayDistance) - _player.transform.position;
					// use this quaternion to convert the line into the xy plane 
					Quaternion normalRotation = Quaternion.FromToRotation(_playerMouseInputPlaneNormal, Vector3.back);
					Vector3 xyPlaneVector = normalRotation * mouseVector;
					secondaryDirection = xyPlaneVector;
				}
			}
			return secondaryDirection;
		}
	}
}
#endif
