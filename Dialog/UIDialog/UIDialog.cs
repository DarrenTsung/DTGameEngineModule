using DT;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DT.GameEngine {
  [System.Serializable]
  public class UIDialogModel : BasicDialogModel<UIDialogKeyframe> {
  }

  [System.Serializable]
  public class UIDialogKeyframe : BasicDialogKeyframe {
    public string characterPrefabName;
  }

  public class UIDialog : BasicDialog<UIDialogModel, UIDialogKeyframe> {
    // PRAGMA MARK - Internal
    [SerializeField]
    private GameObject _characterPrefab;

    [Header("Outlets (FILL THESE OUT)")]
    [SerializeField]
    private GameObject _container;
    [SerializeField]
    private Text _dialogText;

    [Header("Animation Properties")]
    [SerializeField]
    private float _newKeyframeDelay = 0.5f;
    [SerializeField]
    private float _textCharacterAnimationSpeed = 0.03f;

    protected override void HandleDialogIndexChange() {
      this.Cleanup();
      this.DoAfterDelay(this._newKeyframeDelay, () => {
        this.CreateNewKeyframe();
      });
    }

    protected override void HandleDialogActiveStateChange(bool dialogActive) {
      this.Cleanup();
    }

    private void Cleanup() {
      if (this._characterPrefab != null) {
        Toolbox.GetInstance<ObjectPoolManager>().Recycle(this._characterPrefab);
      }
      this._dialogText.text = "";

      this._container.SetActive(false);
    }

    private void CreateNewKeyframe() {
      this._container.SetActive(true);

      UIDialogKeyframe currentKeyframe = this.GetCurrentKeyframe();
      this._characterPrefab = Toolbox.GetInstance<ObjectPoolManager>().Instantiate(currentKeyframe.characterPrefabName, parent : this._container, worldPositionStays : false);

      this.StopAllCoroutines();
      this.DoEveryFrameForDuration(this._textCharacterAnimationSpeed * currentKeyframe.text.Length, (float time, float duration) => {
				int numberCharactersToDisplay = Mathf.Min((int)(time / this._textCharacterAnimationSpeed), currentKeyframe.text.Length);
				this._dialogText.text = currentKeyframe.text.Substring(0, numberCharactersToDisplay);
      }, () => {
        this._dialogText.text = currentKeyframe.text;
      });
    }
  }
}
