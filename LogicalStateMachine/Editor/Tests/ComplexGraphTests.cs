using DT;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class ComplexGraphTests {
    [Test]
    public void MultipleTransitions_HappenCorrectly() {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      // A -> B -> C
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();

      Transition bTransition = new Transition();
      bTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition bNodeTransition = new NodeTransition { target = nodeB.Id, transition = bTransition };
      graph.AddOutgoingTransitionForNode(nodeA, bNodeTransition);

      Transition cTransition = new Transition();
      cTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition cNodeTransition = new NodeTransition { target = nodeC.Id, transition = cTransition };
      graph.AddOutgoingTransitionForNode(nodeB, cNodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasIntParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetInt(Arg.Is("Key")).Returns(5);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };

      graph.Start();
      Assert.IsTrue(bEntered);
      Assert.IsTrue(cEntered);
    }

    [Test]
    public void MoreComplexGraph_RunsCorrectly() {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      // A --- B --
      //  \--- C --\- D
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();
      Node nodeC = graph.MakeNode();
      Node nodeD = graph.MakeNode();

      Transition bTransition = new Transition();
      bTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition bNodeTransition = new NodeTransition { target = nodeB.Id, transition = bTransition };
      graph.AddOutgoingTransitionForNode(nodeA, bNodeTransition);

      Transition cTransition = new Transition();
      cTransition.AddTransitionCondition(new IntTransitionCondition("Key", 3));
      NodeTransition cNodeTransition = new NodeTransition { target = nodeC.Id, transition = cTransition };
      graph.AddOutgoingTransitionForNode(nodeA, cNodeTransition);

      Transition cdTransition = new Transition();
      cdTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition cdNodeTransition = new NodeTransition { target = nodeD.Id, transition = cdTransition };
      graph.AddOutgoingTransitionForNode(nodeC, cdNodeTransition);

      Transition bdTransition = new Transition(waitForManualExit: true);
      bdTransition.AddTransitionCondition(new IntTransitionCondition("Key", 5));
      NodeTransition bdNodeTransition = new NodeTransition { target = nodeD.Id, transition = bdTransition };
      graph.AddOutgoingTransitionForNode(nodeB, bdNodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasIntParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetInt(Arg.Is("Key")).Returns(5);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool bEntered = false;
      nodeB.OnEnter += () => { bEntered = true; };
      bool cEntered = false;
      nodeC.OnEnter += () => { cEntered = true; };
      bool dEntered = false;
      nodeD.OnEnter += () => { dEntered = true; };

      graph.Start();
      Assert.IsTrue(bEntered);
      Assert.IsFalse(cEntered);
      Assert.IsFalse(dEntered);

      bEntered = false;
      cEntered = false;
      dEntered = false;

      nodeB.TriggerManualExit();
      Assert.IsFalse(bEntered);
      Assert.IsFalse(cEntered);
      Assert.IsTrue(dEntered);
    }
  }
}