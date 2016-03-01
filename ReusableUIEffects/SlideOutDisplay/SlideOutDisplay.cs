using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DT.GameEngine {
  public class SlideOutDisplay : MonoBehaviour, IRecycleCleanupSubscriber {
    // PRAGMA MARK - Public Interface
    public void DisplayObjects(IEnumerable<GameObject> gameObjects) {
      List<IDisplayDelegate> displayDelegates = new List<IDisplayDelegate>();

      foreach (GameObject gameObject in gameObjects) {
        displayDelegates.Add(gameObject.GetRequiredComponent<IDisplayDelegate>());
        gameObject.transform.SetParent(this._objectContainer.transform, worldPositionStays: false);
      }

      this._displayDelegates = displayDelegates.ToArray();

      this.SlideOut();
    }


    // PRAGMA MARK - IRecycleCleanupSubscriber Implementation
    public void OnRecycleCleanup() {
      this._animator.SetTrigger("Reset");
      this._objectContainer.transform.DestroyAllChildren();
    }


    // PRAGMA MARK - Internal
    [Header("Outlets")]
    [SerializeField]
    private GameObject _objectContainer;
    private Animator _animator;

    [Header("Properties")]
    [SerializeField]
    private float _delayAfterSlidingOut = 0.8f;
    [SerializeField]
    private float _delayBetweenObjects = 0.5f;

    private IDisplayDelegate[] _displayDelegates;

    private void Awake() {
      this._animator = this.GetRequiredComponent<Animator>();
    }

    private void SlideOut() {
      this._animator.SetTrigger("SlideOut");
    }

    private void HandleSlideOutFinished() {
      this.DoAfterDelay(this._delayAfterSlidingOut, () => {
        this.ShowObjects();
      });
    }

    private void ShowObjects() {
      this.StartCoroutine(this.ShowObjectCoroutine());
    }

    private IEnumerator ShowObjectCoroutine() {
      foreach (IDisplayDelegate displayDelegate in this._displayDelegates) {
        displayDelegate.Display();
        yield return new WaitForSeconds(this._delayBetweenObjects);
      }
    }
  }
}