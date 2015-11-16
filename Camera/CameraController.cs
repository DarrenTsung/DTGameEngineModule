using DT;
using System.Collections;
using System.Collections.Generic;
﻿using UnityEngine;

namespace DT {
	public class CameraController : MonoBehaviour {
		// PRAGMA MARK - Public Interface
		protected static Dictionary<System.Type, MonoBehaviour> _cachedComponentMap = new Dictionary<System.Type, MonoBehaviour>();
		public static T Main<T>() where T : class {
			return Camera.main.gameObject.GetCachedComponent<T>(_cachedComponentMap);
		}
		
		public virtual void Shake(float magnitude, float duration, EaseType easeType = EaseType.QuadOut) {
			if (_shakeCoroutine != null) {
				this.StopCoroutine(_shakeCoroutine);
			}
			_shakeCoroutine = this.ShakeHelper(magnitude, duration, easeType);
			this.StartCoroutine(_shakeCoroutine);
		}
		
		// PRAGMA MARK - Internal
		[SerializeField]
		protected float _cameraSpeed = 1.5f; 
		protected IEnumerator _shakeCoroutine;
		
		protected IEnumerator ShakeHelper(float magnitude, float duration, EaseType easeType) {
			Vector3 offset;
			for (float time = 0.0f; time < duration; time += Time.deltaTime) {
				float currentMagnitude = Easers.Ease(easeType, magnitude, 0.0f, time, duration);
				Vector3 transformedRight = transform.rotation * Vector3.right;
				Vector3 transformedUp = transform.rotation * Vector3.up;
				
				offset = (transformedRight * Random.Range(-currentMagnitude, currentMagnitude)) 
								 + (transformedUp * Random.Range(-currentMagnitude, currentMagnitude));
				
				transform.position += offset;
				yield return new WaitForEndOfFrame();
				transform.position -= offset;
			}
		}
	}
}