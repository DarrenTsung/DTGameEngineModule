using DT;
using UnityEngine;

namespace DT.GameEngine {
  public class GameConstants : MonoBehaviour {
    // PRAGMA MARK - Static
    public static int kDooberAutoTapDelay {
      get { return Toolbox.GetInstance<GameConstants>()._dooberAutoTapDelay; }
    }


    // PRAGMA MARK - Internal
    [Header("IdQuantityDoober Properties")]
    [SerializeField]
    private int _dooberAutoTapDelay;
  }
}