using DT;
using System.Collections;
﻿using UnityEngine;

#if TK2D
namespace DT.GameEngine {
	public class SpawnWithThresholdInCameraBoundsController : MonoBehaviour {
		// PRAGMA MARK - Internal
		[SerializeField]
		protected Direction _direction;
		[SerializeField]
		protected Condition _condition;
		[SerializeField]
		protected float _threshold;
		[SerializeField]
		protected GameObject _objectToSpawn;
		
		protected GameObject _child;
		protected bool _previousConditionCheck;
		
		protected void Awake() {
			if (_objectToSpawn) {
				_objectToSpawn.SetActive(false);
			}
		}

		protected void Update() {
			Vector2 distanceOutside = CameraController.Main<CameraController2D>().DistanceFromCameraSide(_direction, transform.position);
			// if the direction is left / right than we will use the x value, otherwise y value
			float appropriateValue = _direction.ApplicableValueToVector2(distanceOutside);
			bool conditionCheck = _condition.Apply(appropriateValue, _threshold);
			
			// only spawn a child if
			// 1. no child exists
			// 2. condition went from false to true
			if (!_child && (!_previousConditionCheck && conditionCheck)) {
				_child = MonoBehaviour.Instantiate(_objectToSpawn, transform.position, Quaternion.identity) as GameObject;
				_child.SetActive(true);
			}
			
			_previousConditionCheck = conditionCheck;
		}
	}
}
#endif