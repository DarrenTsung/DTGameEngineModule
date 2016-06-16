using System.Collections;
using UnityEngine;

namespace DT.GameEngine {
  public class BasicDisplayDelegate : MonoBehaviour, IDisplayDelegate, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Public Interface
    public void Display() {
      this._animator.SetTrigger("Display");
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      this._animator.SetTrigger("Reset");
    }


    // PRAGMA MARK - Internal
    private Animator _animator;

    private void Awake() {
      this._animator = this.GetRequiredComponent<Animator>();
    }
  }
}