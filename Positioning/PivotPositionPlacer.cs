using DT;
using UnityEngine;

namespace DT.GameEngine {
  public static class PivotPositionPlacer {
    // PRAGMA MARK - Public Interface
    public static void SetPositionWithPivot(GameObject gameObject, Vector3 position, Vector3 pivot) {
      Bounds relativeTotalBounds = PivotPositionPlacer.ComputeRelativeTotalBounds(gameObject);

      // convert pivot from (0 - 1) space to (-1 - 1) space (extents space)
      Vector3 convertedPivot = 2.0f * (pivot - new Vector3(0.5f, 0.5f, 0.5f));
      Vector3 pivotVector = Vector3.Scale(convertedPivot, relativeTotalBounds.extents);

      gameObject.transform.position = position - pivotVector - relativeTotalBounds.center;
    }


    // PRAGMA MARK - Internal
    private static Bounds ComputeRelativeTotalBounds(GameObject gameObject) {
      Bounds? computedBounds = null;
      foreach (Renderer r in gameObject.GetComponentsInChildren<Renderer>()) {
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
      computedBoundsValue.center = computedBoundsValue.center - gameObject.transform.position;

      return computedBoundsValue;
    }
  }
}