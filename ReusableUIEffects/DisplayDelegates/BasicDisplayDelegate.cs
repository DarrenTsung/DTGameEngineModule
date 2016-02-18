using UnityEngine;
using System.Collections;

namespace DT.GameEngine {
  public class BasicDisplayDelegate : MonoBehaviour, IDisplayDelegate {
    // PRAGMA MARK - Public Interface
    public void Display() {
      this._animator.SetTrigger("Display");
    }


    // PRAGMA MARK - Internal
    private Animator _animator;

    private void Awake() {
      this._animator = this.GetRequiredComponent<Animator>();
    }
  }
}