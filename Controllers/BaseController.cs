using DT;
using System;
using System.Collections;
﻿using UnityEngine;

namespace DT.Controllers {
  [Serializable]
  public struct CollisionInfo2D {
  	public bool Above, Below;
  	public bool Left, Right;
  	
  	public bool ClimbingSlope;
  	public bool DescendingSlope;
  	public bool FallingDownSlope;
  	public float SlopeAngle;
  	public float PreviousSlopeAngle;
  	
  	public void Reset() {
  		Above = Below = false;
  		Left = Right = false;
  		ClimbingSlope = false;
  		DescendingSlope = false;
  		FallingDownSlope = false;
  		PreviousSlopeAngle = SlopeAngle;
  		SlopeAngle = 0.0f;
  	}
  }

  [Serializable]
  public struct Constraints2D {
  	public bool FreezePositionX, FreezePositionY;
  	public bool FallThroughOneWayPlatforms;
  }

  public class BaseController2D : RaycastController2D {
  	const float MaxFallVelocity = 10.0f;
  	
		public override RaycastHit2D CastRay(Vector2 relativeRayOrigin, Vector2 direction, float rayLength, LayerMask layers, LayerMask blockingLayers = default(LayerMask)) {
  		RaycastHit2D hit = base.CastRay(relativeRayOrigin, direction, rayLength, layers, blockingLayers);
  		if (hit.collider && hit.GameObject().IsInLayerMask(_oneWayGeometryLayer)) {
  			if (this.Constraints.FallThroughOneWayPlatforms || Mathf.Sign(direction.y) != -1) {
          // return empty hit instead of actual hit if we're allowed to go through the one way platform
  				return new RaycastHit2D();
  			}
  		}
  		return hit;
  	}
  	
  	public void FallThroughOneWayPlatformsForTime(float time) {
  		if (_fallThroughCoroutine != null) {
  			StopCoroutine(_fallThroughCoroutine);
  			_fallThroughCoroutine = null;
  		}
  		_fallThroughCoroutine = this.FallThroughOneWayPlatformsHelper(time);
  		StartCoroutine(_fallThroughCoroutine);
  	}
  	
  	public Vector2 Velocity {
  		get { return _velocity; }
  		set { 
  			_velocity = value; 
  			if (_velocity.y < -BaseController2D.MaxFallVelocity) {
  				_velocity = _velocity.SetY(-BaseController2D.MaxFallVelocity);
  			}
  		}
  	}
  	
  	public CollisionInfo2D Collisions {
  		get { return _collisions; }
  	}
  	
  	public bool Grounded() {
  		return _collisions.Below;
  	}
  	
  	public Constraints2D Constraints;
  	
  	public Vector2 VelocityRelativeToSize {
  		get { return new Vector2(_velocity.x / this.Width, _velocity.y / this.Height); }
  	}
  	
  	// PRAGMA MARK - Internal
  	[SerializeField]
  	protected CollisionInfo2D _collisions;
    
    [SerializeField]
    protected LayerMask _oneWayGeometryLayer;
  	
  	[SerializeField]
  	protected Vector2 _velocity;
  	protected IEnumerator _fallThroughCoroutine = null;
  	
  	protected virtual void Update() {
			this.Move(_velocity * Time.deltaTime);
  		this.ApplyCollisionsAndConstraintsToVelocity();
  	}
  	
  	protected virtual void ApplyCollisionsAndConstraintsToVelocity() {
  		if (_collisions.Below || _collisions.Above || this.Constraints.FreezePositionY) {
  			_velocity.y = 0.0f;
  		}
  		
  		if (_collisions.Left || _collisions.Right || this.Constraints.FreezePositionX) {
  			_velocity.x = 0.0f;
  		}
  	}
  	
  	protected override void StartMove(ref Vector2 deltaVelocity) {
      base.StartMove(ref deltaVelocity);
  		_collisions.Reset();
  	}
  	
  	protected override void EndMove(ref Vector2 deltaVelocity) {
  		this.UpdateDeltaVelocityWithConstraints(ref deltaVelocity);
      base.EndMove(ref deltaVelocity);
  	}
  	
  	protected void UpdateDeltaVelocityWithConstraints(ref Vector2 deltaVelocity) {
  		if (this.Constraints.FreezePositionX) {
  			deltaVelocity.x = 0.0f;
  		}
  		
  		if (this.Constraints.FreezePositionY) {
  			deltaVelocity.y = 0.0f;
  		}
  	}
  	
  	protected IEnumerator FallThroughOneWayPlatformsHelper(float time) {
  		this.Constraints.FallThroughOneWayPlatforms = true;
  		yield return new WaitForSeconds(time);
  		this.Constraints.FallThroughOneWayPlatforms = false;
  		_fallThroughCoroutine = null;
  	}
  }
}