using DT;
using System;
using System.Collections;
using System.Linq;

namespace DT.GameEngine {
  public interface ITransitionCondition {
    bool IsConditionMet(TransitionContext context);
    void HandleTransitionUsed(TransitionContext context);
  }
}
