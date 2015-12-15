using DT;
using DT.GameEngine;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT.Game {
	public class CameraController3D : CameraController {
		// PRAGMA MARK - Public Interface
		
		
		// PRAGMA MARK - Internal
		[SerializeField]
		protected Vector3 _offsetVector = new Vector3(0.0f, 9.3f, -5.6f);
		[SerializeField]
		protected Transform _targetTransform;
		
		protected void Awake() {
			DTGameEngineNotifications.PlayerChanged.AddListener(SetupWithPlayer);
		}

		protected virtual void SetupWithPlayer(int playerIndex, GameObject player) {
			_targetTransform = player.transform;
		}
		
		protected void LateUpdate() {
			if (!_targetTransform) {
				return;
			}
			
			Vector3 targetPosition = _targetTransform.position + _offsetVector;
			transform.position = Vector3.Lerp(transform.position, targetPosition, _cameraSpeed * Time.deltaTime);
		}
	}
}