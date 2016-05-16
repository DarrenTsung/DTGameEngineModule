using DT;
using System;
using System.Collections;

namespace DT.GameEngine {
  public interface ITransition {
    void ConfigureWithContext(TransitionContext context);

    bool AreConditionsMet();
    void HandleTransitionUsed();

    void AddTransition(ITransitionCondition condition);
  }
}