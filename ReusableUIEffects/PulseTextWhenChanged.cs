using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace DT {
	[CustomExtensionInspector]
	public class PulseTextWhenChanged : MonoBehaviour {
		// PRAGMA MARK - Internal
		[SerializeField]
		private Text _textReference;
		[SerializeField]
		private Transform _textTransform; 
		
		[SerializeField]
		private float _fontSizePulseAdditiveAmount = 0.2f;
		[SerializeField]
		private float _pulseWavelengthTime = 0.5f;
		[SerializeField]
		private int _pulseTimes = 1;
		
		[SerializeField, ReadOnly]
		private Vector3 _baseScale;
		private string _previousText;
		
		private void Awake() {
			if (this._textReference == null) {
				this._textReference = this.GetComponent<Text>();
			}
			
			if (this._textReference == null) {
				Debug.LogError("PulseTextWhenChanged: no reference to Text found!");
			}
			
			this._textTransform = this._textReference.transform;
			
			this._baseScale = this._textTransform.localScale;
			this._previousText = this._textReference.text;
		}
		
		private void Update() {
			if (this._previousText != this._textReference.text) {
				this.PulseText();
				this._previousText = this._textReference.text;
			}
		}
		
		[MakeButton]
		private void PulseText() {
			this.StopAllCoroutines();
			this.StartCoroutine(this.PulseTextCoroutine());
		}
		
		private IEnumerator PulseTextCoroutine() {
			float pulseTimeLength = this._pulseWavelengthTime * this._pulseTimes;
			for (float time = 0.0f;; time += Time.deltaTime) {
				if (time >= pulseTimeLength) {
					time = pulseTimeLength;
				}
				
				this.ComputeAndSetScaleAtTime(time);
				
				if (time >= pulseTimeLength) {
					break;
				}
				yield return new WaitForEndOfFrame();
			}
		}
		
		private void ComputeAndSetScaleAtTime(float time) {
			Vector3 computedScale = this._baseScale * (1.0f + this._fontSizePulseAdditiveAmount * Mathf.Sin(time * 2.0f * Mathf.PI / this._pulseWavelengthTime));
			this._textTransform.localScale = computedScale;
		}
	}
}