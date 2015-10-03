using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public class CameraController : MonoBehaviour {
		// PRAGMA MARK - INTERFACE
		public static CameraController _cachedMainCameraController;
		
		public static CameraController MainCameraController() {
			if (_cachedMainCameraController == null) {
				_cachedMainCameraController = Camera.main.GetComponent<CameraController>();
			}
			return _cachedMainCameraController;
		}
		
		// PRAGMA MARK - INTERNAL
		[SerializeField]
		protected float _cameraSpeed = 1.5f; 
	}
}