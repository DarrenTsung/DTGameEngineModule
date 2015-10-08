using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT {
	public class CameraController<T> : MonoBehaviour {
		// PRAGMA MARK - INTERFACE
		public static T _cachedMainCameraController;
		
		public static T MainCameraController() {
			if (_cachedMainCameraController == null) {
				_cachedMainCameraController = Camera.main.GetComponent<T>();
			}
			return _cachedMainCameraController;
		}
		
		// PRAGMA MARK - INTERNAL
		[SerializeField]
		protected float _cameraSpeed = 1.5f; 
	}
}