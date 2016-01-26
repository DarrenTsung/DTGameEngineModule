using DT;


namespace DT.GameEngine {
  public class UnityBehaviourManager : Singleton<UnityBehaviourManager> {
    protected UnityBehaviourManager() {}

    public delegate void UpdateAction();
    public static event UpdateAction OnUpdate = delegate {};

    private void Update() {
      UnityBehaviourManager.OnUpdate.Invoke();
    }
  }
}