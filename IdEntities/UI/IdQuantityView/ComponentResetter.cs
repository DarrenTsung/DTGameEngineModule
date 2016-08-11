using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  public class ComponentResetter : MonoBehaviour, IRecycleSetupSubscriber {
    // PRAGMA MARK - IRecycleSetupSubscriber Implementation
    public void OnRecycleSetup() {
      JsonUtility.FromJsonOverwrite(this._serialized, this._component);
    }


    // PRAGMA MARK - Internal
    [SerializeField] private Component _component;

    [SerializeField, TextArea] private string _serialized;

    private void Awake() {
      if (this._component == null) {
        Debug.LogError("Component is null!");
      }

      this._serialized = JsonUtility.ToJson(this._component);
    }
  }
}
