using DT;
using System.Collections;
﻿using UnityEngine;

#if TK2D
namespace DT.GameEngine {
	public class CameraController2D : CameraController {
		// PRAGMA MARK - Public Interface
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

		public Vector2 DistanceFromCameraSide(Direction cameraSide, Vector2 point) {
			Rect cameraExtents = this.CameraWorldScreenExtents();

			float x = 0.0f;
			float y = 0.0f;
			switch (cameraSide) {
				case Direction.LEFT:
					x = point.x - cameraExtents.xMin;
					break;
				case Direction.RIGHT:
					x = point.x - cameraExtents.xMax;
					break;
				case Direction.UP:
					y = point.y - cameraExtents.yMax;
					break;
				case Direction.DOWN:
					y = point.y - cameraExtents.yMin;
					break;
			}

			if (cameraSide == Direction.LEFT || cameraSide == Direction.RIGHT) {
				if (point.y > cameraExtents.yMax) {
					y = point.y - cameraExtents.yMax;
				} else if (point.y < cameraExtents.yMin) {
					y = point.y - cameraExtents.yMin;
				}
			} else if (cameraSide == Direction.UP || cameraSide == Direction.DOWN) {
				if (point.x > cameraExtents.xMax) {
					x = point.x - cameraExtents.xMax;
				} else if (point.x < cameraExtents.xMin) {
					x = point.x - cameraExtents.xMin;
				}
			}

			return new Vector2(x, y);
		}

		public Vector2 OffsetVector {
			set { _offsetVector = value; }
		}

		public void TeleportToTarget() {
			Vector3 targetPosition = _targetTransform.position;
			targetPosition.z = transform.position.z; // don't move in the z axis
			transform.position = targetPosition;
		}

		public Rect CameraWorldScreenExtents() {
			Rect cameraWorldExtents = this.Camera.ScreenExtents;
			cameraWorldExtents.center = cameraWorldExtents.center + (Vector2)transform.position;
			return cameraWorldExtents;
		}

    public Vector2 GetCameraRelativePosition(Vector2 relativePosition) {
      Rect cameraWorldScreenExtents = this.CameraWorldScreenExtents();
      return cameraWorldScreenExtents.min + Vector2.Scale(new Vector2(cameraWorldScreenExtents.width, cameraWorldScreenExtents.height), relativePosition);
    }

		// PRAGMA MARK - Internal
    protected tk2dCamera Camera {
      get {
        if (this._camera == null) {
          this._camera = this.GetComponent<tk2dCamera>();
        }
        return this._camera;
      }
    }

    [SerializeField]
    protected bool _followPlayer;

		protected tk2dCamera _camera;
		protected Transform _targetTransform;
		protected Vector2 _offsetVector;

		protected void Awake() {
			_camera = this.GetComponent<tk2dCamera>();
			DTGameEngineNotifications.PlayerChanged.AddListener(SetupWithPlayer);
		}

		protected virtual void SetupWithPlayer(int playerIndex, GameObject player) {
			_targetTransform = player.transform;
		}

		protected void LateUpdate() {
			if (!_targetTransform || !this._followPlayer) {
				return;
			}

			Vector3 targetPosition = _targetTransform.position + (Vector3)_offsetVector;
			targetPosition.z = transform.position.z; // don't move in the z axis

			transform.position = Vector3.Lerp(transform.position, targetPosition, _cameraSpeed * Time.deltaTime);

			// this.RestrictCameraInBounds(_bounds, _boundsPosition);
		}

		protected bool RestrictCameraInBounds(Rect otherBounds, Vector3 otherPosition) {
			Rect cameraExtents = this.Camera.ScreenExtents;

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
	}
}
#endif