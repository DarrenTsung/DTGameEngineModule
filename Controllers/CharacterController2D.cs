using DT;
using System.Collections;
﻿using UnityEngine;

namespace DT.Controllers {
  public class CharacterController2D : BaseController2D {
  	public float Gravity {
  		get { return _gravity; }
  		set { _gravity = value; }
  	}
  	
  	public bool GravityDisabled {
  		get { return _gravityDisabled; }
  		set { 
  			_gravityDisabled = value; 
  		}
  	}
  	
  	public float MaxClimbAngle {
  		get { return _maxClimbAngle; }
  	}
  	
  	public float MaxDescendAngle {
  		get { return _maxDescendAngle; }
  	}
  	
  	// PRAGMA MARK - Internal
  	[SerializeField]
  	protected float _gravity = -24.0f;
  	[SerializeField]
  	protected bool _gravityDisabled = false;
  	
  	[SerializeField, Range(0, 90)]
  	protected float _maxClimbAngle = 60.0f;
  	[SerializeField, Range(0, 90)]
  	protected float _maxDescendAngle = 60.0f;

    [SerializeField]
    protected LayerMask _geometryCollisionMask;
    
  	protected Vector2 _previousDeltaVelocity;
  	
  	protected override void Update() {
  		if (!_gravityDisabled) {
  			_velocity.y += _gravity * Time.deltaTime;
  		}
  		base.Update();
  	}
  		
  	protected override void EndMove(ref Vector2 deltaVelocity) {
  		_previousDeltaVelocity = deltaVelocity;
  		
  		if (deltaVelocity.y < 0.0f) {
  			this.DescendSlope(ref deltaVelocity);
  		}
      
      this.HandleCollisions(ref deltaVelocity);
      
  		base.EndMove(ref deltaVelocity);
  	}
    
    protected void HandleCollisions(ref Vector2 deltaVelocity) {
  		// NOTE (darren): we have 4 possible cases here
  		// we want to avoid scenarios where both horizontal + vertical collisions happen
  		//
  		// this is to prevent the first collision detection inferring the other axis velocity and hitting something it 
  		// wouldn't hit if the order of collision detection was reversed
  		//
  		// we store the velocity and collision info (which get mutated by the function) and then
  		// restore them and use the other ordering if both functions change the velocity with normal collisions
  		bool horizontalCollisionsChangeVelocity = false;
  		bool verticalCollisionsChangeVelocity = false;
      
  		Vector2 deltaVelocityCopy = deltaVelocity;
  		CollisionInfo2D collisionCopy = _collisions;
      
  		if (deltaVelocity.y != 0.0f) {
  			this.HandleVerticalCollisions(ref deltaVelocity, out verticalCollisionsChangeVelocity);
  		}
  		if (deltaVelocity.x != 0.0f) {
  			this.HandleHorizontalCollisions(ref deltaVelocity, out horizontalCollisionsChangeVelocity);
  		}
  		
  		if (horizontalCollisionsChangeVelocity && verticalCollisionsChangeVelocity) {
  			deltaVelocity = deltaVelocityCopy;
  			_collisions = collisionCopy;
  			if (deltaVelocity.x != 0.0f) {
  				this.HandleHorizontalCollisions(ref deltaVelocity);
  			}
  			if (deltaVelocity.y != 0.0f) {
  				this.HandleVerticalCollisions(ref deltaVelocity);
  			}
  		}
    }
  		
  	protected void HandleHorizontalCollisions(ref Vector2 deltaVelocity) {
  		bool devNull;
  		this.HandleHorizontalCollisions(ref deltaVelocity, out devNull);
  	}
  	
  	protected void HandleHorizontalCollisions(ref Vector2 deltaVelocity, out bool changedWithNormalCollision) {
  		changedWithNormalCollision = false;
  		
  		float directionX = Mathf.Sign(deltaVelocity.x);
  		float rayLength = Mathf.Abs(deltaVelocity.x) + _skinWidth;
  		
  		if (_collisions.FallingDownSlope) {
  			changedWithNormalCollision = true;
  			return;
  		}
  		
  		for (int i=0; i<_horizontalRayCount; i++) {
  			Vector2 rayOrigin = (directionX == 1) ? _raycastOrigins.BottomRight : _raycastOrigins.BottomLeft;
  			rayOrigin += Vector2.up * (_horizontalRaySpacing * i + deltaVelocity.y);
  			RaycastHit2D hit = this.CastRay(rayOrigin, Vector2.right * directionX, rayLength, _geometryCollisionMask);
  			if (hit.collider) {
  				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
  				if (i == 0 && slopeAngle <= _maxClimbAngle) {
  					if (_collisions.DescendingSlope) {
  						_collisions.DescendingSlope = false;
  						deltaVelocity = _previousDeltaVelocity;
  					}
  					
  					float distanceToSlopeStart = 0.0f;
  					// hit a new slope
  					if (slopeAngle != _collisions.PreviousSlopeAngle) {
  						distanceToSlopeStart = hit.distance - _skinWidth;
  						deltaVelocity.x -= distanceToSlopeStart * directionX;
  					}
  					this.ClimbSlope(ref deltaVelocity, slopeAngle);
  					deltaVelocity.x += distanceToSlopeStart * directionX;
  				}
  				
  				if (!_collisions.ClimbingSlope || slopeAngle > _maxClimbAngle) {
  					deltaVelocity.x = (hit.distance - _skinWidth) * directionX;
  					changedWithNormalCollision = true;
  					rayLength = hit.distance;
  					
  					if (directionX == 1) {
  						_collisions.Right = true;
  					} else {
  						_collisions.Left = true;
  					}
  				}
  			}
  			
  			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
  		}
  	}
  	
  	protected void HandleVerticalCollisions(ref Vector2 deltaVelocity) {
  		bool devNull;
  		this.HandleVerticalCollisions(ref deltaVelocity, out devNull);
  	}
  	
  	protected void HandleVerticalCollisions(ref Vector2 deltaVelocity, out bool changedWithNormalCollision) {
  		changedWithNormalCollision = false;
  		
  		float directionY = Mathf.Sign(deltaVelocity.y);
  		float rayLength = Mathf.Abs(deltaVelocity.y) + _skinWidth;
  		
  		for (int i=0; i<_verticalRayCount; i++) {
  			Vector2 rayOrigin = (directionY == 1) ? _raycastOrigins.TopLeft : _raycastOrigins.BottomLeft;
  			rayOrigin += Vector2.right * (_verticalRaySpacing * i + deltaVelocity.x);
  			RaycastHit2D hit = this.CastRay(rayOrigin, Vector2.up * directionY, rayLength, _geometryCollisionMask);
  			if (hit.collider != null) {
  				if (hit.distance > 0.0f) {
  					float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
  					if (directionY == -1 && slopeAngle > _maxClimbAngle) {
  						float distanceToSlopeStart = hit.distance - _skinWidth;
  						deltaVelocity.y -= distanceToSlopeStart * directionY;
  						
  						float moveDistance = Mathf.Abs(deltaVelocity.y);
  						deltaVelocity.y = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance * directionY;
  						deltaVelocity.x = moveDistance / Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(hit.normal.x);
  						changedWithNormalCollision = true;
  						_collisions.FallingDownSlope = true;
  						
  						deltaVelocity.y += distanceToSlopeStart * directionY;
  						continue;
  					}
  				}
  				
  				deltaVelocity.y = (hit.distance - _skinWidth) * directionY;
  				changedWithNormalCollision = true;
  				rayLength = hit.distance;
  				
  				if (_collisions.ClimbingSlope) {
  					deltaVelocity.x = deltaVelocity.y / Mathf.Tan(_collisions.SlopeAngle * Mathf.Deg2Rad) * Mathf.Sign(deltaVelocity.x);
  				}
  				
  				if (directionY == 1) {
  					_collisions.Above = true;
  				} else {
  					_collisions.Below = true;
  				}
  			}
  		}
  		
  		if (_collisions.ClimbingSlope) {
  			float directionX = Mathf.Sign(deltaVelocity.x);
  			rayLength = Mathf.Abs(deltaVelocity.x) + _skinWidth;
  			
  			Vector2 rayOrigin = ((directionX == -1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight) + Vector2.up * deltaVelocity.y;
  			RaycastHit2D hit = this.CastRay(rayOrigin, Vector2.right * directionX, rayLength, _geometryCollisionMask);
  			
  			if (hit.collider) {
  				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
  				if (slopeAngle != _collisions.SlopeAngle) {
  					deltaVelocity.x = (hit.distance - _skinWidth) * directionX;
  					_collisions.SlopeAngle = slopeAngle;
  				}
  			}
  		}
  	}
  	
  	protected void ClimbSlope(ref Vector2 deltaVelocity, float slopeAngle) {
  		float moveDistance = Mathf.Abs(deltaVelocity.x);
  		float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
  		
  		if (deltaVelocity.y <= climbVelocityY) {
  			deltaVelocity.y = climbVelocityY;
  			deltaVelocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(deltaVelocity.x);
  			
  			_collisions.Below = true;
  			_collisions.ClimbingSlope = true;
  			_collisions.SlopeAngle = slopeAngle;
  		}
  	}
  	
  	protected void DescendSlope(ref Vector2 deltaVelocity) {
  		float directionX = Mathf.Sign(deltaVelocity.x);
  		Vector2 rayOrigin = (directionX == 1) ? _raycastOrigins.BottomLeft : _raycastOrigins.BottomRight;
  		RaycastHit2D hit = this.CastRay(rayOrigin, -Vector2.up, Mathf.Infinity, _geometryCollisionMask);
  		if (hit.collider) {
  			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
  			if (slopeAngle != 0 && slopeAngle <= _maxDescendAngle) {
  				if (Mathf.Sign(hit.normal.x) == directionX) {
  					if (hit.distance - _skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(deltaVelocity.x)) {
  						float moveDistance = Mathf.Abs(deltaVelocity.x);
  						float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
  						deltaVelocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(deltaVelocity.x);
  						deltaVelocity.y -= descendVelocityY;
  						
  						_collisions.Below = true;
  						_collisions.DescendingSlope = true;
  						_collisions.SlopeAngle = slopeAngle;
  					}
  				}
  			}
  		}
  	}
  }
}