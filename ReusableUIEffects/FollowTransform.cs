using DT;
using System;
using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class FollowTransform : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void ConfigureWithTransform(Transform t, Func<Vector3, Vector3> positionTransformationFunction) {
      this._transformToFollow = t;
      this._positionTransformationFunction = positionTransformationFunction;
    }


    // PRAGMA MARK - Internal
    private Transform _transformToFollow;
    private Func<Vector3, Vector3> _positionTransformationFunction = null;

    private Vector3 _cachedPosition;

    void LateUpdate() {
      if (this._transformToFollow == null) {
        Debug.LogWarning("FollowTransform - no transform to follow! Why is this even being used?");
        return;
      }

      Vector3 position = this._transformToFollow.position;
      if (position == this._cachedPosition) {
        return;
      }
      // cache the position before transforming
      this._cachedPosition = position;

      if (this._positionTransformationFunction != null) {
        position = this._positionTransformationFunction.Invoke(position);
      }

      this.transform.position = position;
    }
  }
}