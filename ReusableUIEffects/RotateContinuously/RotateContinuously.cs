using UnityEngine;
using System.Collections;
using DT;

namespace DT.GameEngine {
  public class RotateContinuously : MonoBehaviour {
    // PRAGMA MARK - Internal
    [SerializeField]
    private float _rotationSpeedInDegrees;
    [SerializeField, EnumFlag]
    private VectorAxis _axis;
    [SerializeField]
    private RotationDirection _rotationDirection;

    private void Update() {
      Vector3 rotationAxis = this._axis.VectorValue() * this._rotationDirection.FloatValue();
      this.transform.Rotate(rotationAxis * this._rotationSpeedInDegrees * Time.deltaTime);
    }
  }
}