using DT;
using UnityEngine;

namespace DT.GameEngine {
  public class PivotPositionPlacer : MonoBehaviour {
    // PRAGMA MARK - Public Interface
    public void SetPositionWithPivot(Vector3 position, Vector3 pivot) {
      Bounds relativeTotalBounds = this.ComputedRelativeTotalBounds;

      // convert pivot from (0 - 1) space to (-1 - 1) space (extents space)
      Vector3 convertedPivot = 2.0f * (pivot - new Vector3(0.5f, 0.5f, 0.5f));
      Vector3 pivotVector = Vector3.Scale(convertedPivot, relativeTotalBounds.extents);

      this.transform.position = position - pivotVector - relativeTotalBounds.center;
    }


    // PRAGMA MARK - Internal
    protected Bounds? _computedRelativeTotalBounds = null;
    protected Bounds ComputedRelativeTotalBounds {
      get {
        if (this._computedRelativeTotalBounds == null) {
          Bounds? computedBounds = null;
          foreach (Renderer r in this.GetComponentsInChildren<Renderer>()) {
            if (computedBounds == null) {
              computedBounds = r.bounds;
            } else {
              Bounds oldBounds = (Bounds)computedBounds;
              oldBounds.SetMinMax(
                Vector3.Min(oldBounds.min, r.bounds.min),
                Vector3.Max(oldBounds.max, r.bounds.max)
              );

              computedBounds = oldBounds;
            }
          }

          Bounds computedBoundsValue = (Bounds)computedBounds;
          computedBoundsValue.center = computedBoundsValue.center - this.transform.position;

          this._computedRelativeTotalBounds = computedBoundsValue;
        }
        return (Bounds)this._computedRelativeTotalBounds;
      }
    }
  }
}