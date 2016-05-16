using DT;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class TransitionManualExitTests {
    [Test]
    public void NoTransitionCondition_WithWaitForManualExit_TakenAfterManualExitTriggered() {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition(waitForManualExit: true);
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsFalse(entered);

      nodeA.TriggerManualExit();
      Assert.IsTrue(entered);
    }
  }
}