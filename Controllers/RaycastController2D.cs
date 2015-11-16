using DT;
using System;
using System.Collections;
﻿using UnityEngine;

namespace DT.Controllers {
	public struct RaycastOrigins {
		public Vector2 TopLeft, TopRight;
		public Vector2 BottomLeft, BottomRight;
		public float Width, Height;
	}

	public class RaycastController2D : MonoBehaviour {
		// PRAGMA MARK - Public Interface 
		/// <summary>
		/// Helper function that passes Vector2.zero to the absolute ray offset
		/// </summary>
		public RaycastHit2D CastRelativeRay(Vector2 relativeRayOrigin, Vector2 direction, float rayLength, LayerMask layers, LayerMask blockingLayers = default(LayerMask)) {
			return this.CastRelativeRay(relativeRayOrigin, Vector2.zero, direction, rayLength, layers, blockingLayers);
		}
		
		/// <summary>
		/// Casts a ray against layers using coordinates relative to the controller's raycast origins
		/// Rays start from skin width!
		/// (0, 0) == BottomLeft, (0.5, 0.5) == Center, (1.0, 1.0) == TopRight
		/// An additional absolute (world coordinate) offset can also be added 
		/// </summary>
		public RaycastHit2D CastRelativeRay(Vector2 relativeRayOrigin, Vector2 absoluteRayOffset, Vector2 direction, float rayLength, LayerMask layers, LayerMask blockingLayers = default(LayerMask)) {
			Vector2 rayOrigin = _raycastOrigins.BottomLeft + new Vector2(relativeRayOrigin.x * _raycastOrigins.Width,
																															 		 relativeRayOrigin.y * _raycastOrigins.Height);
			rayOrigin += absoluteRayOffset;
			return this.CastRay(rayOrigin, direction, rayLength, layers, blockingLayers);
		}
		
		/// <summary>
		/// Casts a ray against layers, a blocking layer may be specified
		/// </summary>
		public virtual RaycastHit2D CastRay(Vector2 rayOrigin, Vector2 direction, float rayLength, LayerMask layers, LayerMask blockingLayers = default(LayerMask)) {
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, rayLength, layers);
			RaycastHit2D blockingHit = new RaycastHit2D();
			// don't send out blocking raycast if regular raycast doesn't hit
			// or if blocking layermask is 0 (no layers)
			if (hit.collider != null && blockingLayers != 0) {
				blockingHit = Physics2D.Raycast(rayOrigin, direction, rayLength, blockingLayers);
			}
			
			if (hit.collider != null && blockingHit.collider != null) {
				if (hit.distance > blockingHit.distance) {
					hit = new RaycastHit2D();
				}
			}
			
			Color rayColor = hit.collider != null ? Color.green : Color.red;
			Debug.DrawRay(rayOrigin, direction * rayLength, rayColor);
			return hit;
		}
		
		public float SkinWidth {
			get { return _skinWidth; }
		}
		
		public float Width {
			get { return _raycastOrigins.Width; }
		}
		
		public float Height {
			get { return _raycastOrigins.Height; }
		}
		
		// PRAGMA MARK - Internal
		[SerializeField, Range(2, 20)]
		protected int _horizontalRayCount = 4;
		[SerializeField, Range(2, 20)]
		protected int _verticalRayCount = 4;
		[SerializeField, Range(0.01f, 0.3f)]
		protected float _skinWidth = 0.015f;
		
		protected float _horizontalRaySpacing;
		protected float _verticalRaySpacing;
		
		protected RaycastOrigins _raycastOrigins;
		protected BoxCollider2D _collider;
		protected Rigidbody2D _rigidbody;
		protected Transform _transform;
		
		protected virtual void Awake() {
			_collider = this.GetComponentsInChildren<BoxCollider2D>()[0];
			_rigidbody = this.GetComponent<Rigidbody2D>();
			_transform = this.GetComponent<Transform>();
			if (!_rigidbody) {
				Debug.LogWarning("gameObject.FullName(): " + gameObject.FullName() + " missing rigidbody2D!");
			} else {
				_rigidbody.isKinematic = true;
				_rigidbody.interpolation = RigidbodyInterpolation2D.None;
			}
			this.CalculateRaySpacing();
		}
		
		protected void Move(Vector2 deltaVelocity) {
			this.StartMove(ref deltaVelocity);
			this.EndMove(ref deltaVelocity);
		}
		
		protected virtual void StartMove(ref Vector2 deltaVelocity) {
			this.UpdateRaycastOrigins();
		}
		
		protected virtual void EndMove(ref Vector2 deltaVelocity) {
			transform.Translate(deltaVelocity);
		}
			
		// recomputes raycast origins using skinwidth && collider
		protected void UpdateRaycastOrigins() {
			Bounds bounds = _collider.bounds;
			bounds.Expand(_skinWidth * -2.0f);
			
			_raycastOrigins.BottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			_raycastOrigins.BottomRight = new Vector2(bounds.max.x, bounds.min.y);
			_raycastOrigins.TopLeft = new Vector2(bounds.min.x, bounds.max.y);
			_raycastOrigins.TopRight = new Vector2(bounds.max.x, bounds.max.y);
			
			if (bounds.size.x != _raycastOrigins.Width || bounds.size.y != _raycastOrigins.Height) {
				this.CalculateRaySpacing();
			}
			_raycastOrigins.Width = bounds.size.x;
			_raycastOrigins.Height = bounds.size.y;
		}
		
		protected void CalculateRaySpacing() {
			Bounds bounds = _collider.bounds;
			bounds.Expand(_skinWidth * -2.0f);
			
			_horizontalRaySpacing = bounds.size.y / (_horizontalRayCount - 1);
			_verticalRaySpacing = bounds.size.x / (_verticalRayCount - 1);
		}
	}
}