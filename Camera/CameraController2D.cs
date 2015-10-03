using DT;
using System.Collections;
﻿using UnityEngine;

#if TK2D
namespace DT {
	public class CameraController2D : CameraController {
		// PRAGMA MARK - INTERFACE
		public float DistanceOutsideCameraScreenExtents(Vector2 point) {
			Rect cameraExtents = this.CameraWorldScreenExtents();
			float distanceOutside = 0.0f;
			
			if (point.x < cameraExtents.xMin) {
				distanceOutside = Mathf.Max(distanceOutside, Mathf.Abs(point.x - cameraExtents.xMin));
			} else if (point.x > cameraExtents.xMax) {
				distanceOutside = Mathf.Max(distanceOutside, Mathf.Abs(point.x - cameraExtents.xMax));
			} 
			
			if (point.y < cameraExtents.yMin) {
				distanceOutside = Mathf.Max(distanceOutside, Mathf.Abs(point.y - cameraExtents.yMin));
			} else if (point.y > cameraExtents.yMax) {
				distanceOutside = Mathf.Max(distanceOutside, Mathf.Abs(point.y - cameraExtents.yMax));
			}
			
			return distanceOutside;
		}
		
		// PRAGMA MARK - INTERNAL
		protected tk2dCamera _camera;
		protected Transform _targetTransform;
		
		protected void Awake() {
			_camera = this.GetComponent<tk2dCamera>();
			PlayerManager.Instance.OnPlayerChange.AddListener((object playerObject) => { this.SetupWithPlayer(playerObject as GameObject); });
		}

		protected virtual void SetupWithPlayer(GameObject player) {
			// do nothing
		}
		
		protected void Update() {
			if (!_targetTransform) {
				return;
			}
			
			Vector3 targetPosition = _targetTransform.position + (Vector3)_playerInfluenceVector;
			targetPosition.z = transform.position.z; // don't move in the z axis
			
			transform.position = Vector3.Lerp(transform.position, targetPosition, _cameraSpeed * Time.deltaTime);
			
			// this.RestrictCameraInBounds(_bounds, _boundsPosition);
		}

		protected bool RestrictCameraInBounds(Rect otherBounds, Vector3 otherPosition) {
			Rect cameraExtents = _camera.ScreenExtents;

			float yOver = (transform.position.y + cameraExtents.yMax) - (otherPosition.y + otherBounds.yMax);
			if (yOver > 0) {
				transform.position = transform.position - new Vector3(0.0f, yOver);
			}

			float yBelow = (transform.position.y + cameraExtents.yMin) - (otherPosition.y + otherBounds.yMin);
			if (yBelow < 0) {
				transform.position = transform.position - new Vector3(0.0f, yBelow);
			}

			float xOver = (transform.position.x + cameraExtents.xMax) - (otherPosition.x + otherBounds.xMax);
			if (xOver > 0) {
				transform.position = transform.position - new Vector3(xOver, 0.0f);
			}

			float xBelow = (transform.position.x + cameraExtents.xMin) - (otherPosition.x + otherBounds.xMin);
			if (xBelow < 0) {
				transform.position = transform.position - new Vector3(xBelow, 0.0f);
			}

			return true;
		}
		
		protected Rect CameraWorldScreenExtents() {
			Rect cameraWorldExtents = _camera.ScreenExtents;
			cameraWorldExtents.center = cameraWorldExtents.center + (Vector2)transform.position;
			return cameraWorldExtents;
		}
	}
}
#endif