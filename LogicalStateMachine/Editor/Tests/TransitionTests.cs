using DT;
using NUnit.Framework;
using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DT.GameEngine {
  public class TransitionTests {
    [Test]
    public void NoTransitionConditions_TakenImmediately() {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;
      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = new Transition() };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsTrue(entered);
    }

    [Test]
    public void PopulateStartingContextParameters_CalledWithStartingParameters() {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;
      GraphContextParameter startingParameter = new GraphContextParameter { type = GraphContextParameterType.Int, key = "Key" };
      graph.AddStartingContextParameter(startingParameter);

      IGraphContext mockContext = Substitute.For<IGraphContext>();

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(mockContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      graph.Start();

      mockContext.Received().PopulateStartingContextParameters(Arg.Is<IList<GraphContextParameter>>(list => list.Contains(startingParameter)));
    }

    [TestCase("Key", 5, true)]
    [TestCase("Key", 0, false)]
    [TestCase("NotKey", 5, false)]
    [TestCase("NotKey", 0, false)]
    [TestCase("NotKey", -1, false)]
    public void IntTransitionCondition_TakenWhenMatching_NotTakenWhenNotMatching(string key, int targetValue, bool expectedEntered) {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition();
      transition.AddTransitionCondition(new IntTransitionCondition(key, targetValue));
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasIntParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetInt(Arg.Is("Key")).Returns(5);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.AreEqual(expectedEntered, entered);
    }

    [TestCase("Key", 5)]
    [TestCase("Key", 0)]
    [TestCase("NotKey", 5)]
    [TestCase("NotKey", 0)]
    [TestCase("NotKey", -1)]
    public void IntTransitionCondition_NotTaken_WhenMissingStartingContextParameter(string key, int targetValue) {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition();
      transition.AddTransitionCondition(new IntTransitionCondition(key, targetValue));
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasBoolParameterKey(Arg.Any<string>()).Returns(false);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsFalse(entered);
    }

    [TestCase("Key", true, true)]
    [TestCase("Key", false, false)]
    [TestCase("NotKey", true, false)]
    [TestCase("NotKey", false, false)]
    public void BoolTransitionCondition_TakenWhenMatching_NotTakenWhenNotMatching(string key, bool targetValue, bool expectedEntered) {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition();
      transition.AddTransitionCondition(new BoolTransitionCondition(key, targetValue));
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasBoolParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.GetBool(Arg.Is("Key")).Returns(true);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.AreEqual(expectedEntered, entered);
    }

    [TestCase("Key", true)]
    [TestCase("Key", false)]
    [TestCase("NotKey", true)]
    [TestCase("NotKey", false)]
    public void BoolTransitionCondition_NotTaken_WhenMissingStartingContextParameter(string key, bool targetValue) {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition();
      transition.AddTransitionCondition(new BoolTransitionCondition(key, targetValue));
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasBoolParameterKey(Arg.Any<string>()).Returns(false);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsFalse(entered);
    }

    [TestCase("Key", true)]
    [TestCase("NotKey", false)]
    public void TriggerTransitionCondition_TakenWhenMatching_NotTakenWhenNotMatching(string key, bool expectedEntered) {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition();
      transition.AddTransitionCondition(new TriggerTransitionCondition(key));
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasTriggerParameterKey(Arg.Is("Key")).Returns(true);
      stubContext.HasTrigger(Arg.Is("Key")).Returns(true);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.AreEqual(expectedEntered, entered);
    }

    [TestCase("Key")]
    [TestCase("NotKey")]
    public void TriggerTransitionCondition_NotTaken_WhenMissingStartingContextParameter(string key) {
      Graph graph = ScriptableObject.CreateInstance(typeof(Graph)) as Graph;

      Node nodeA = graph.MakeNode();
      Node nodeB = graph.MakeNode();

      Transition transition = new Transition();
      transition.AddTransitionCondition(new TriggerTransitionCondition(key));
      NodeTransition nodeTransition = new NodeTransition { target = nodeB.Id, transition = transition };
      graph.AddOutgoingTransitionForNode(nodeA, nodeTransition);

      IGraphContext stubContext = Substitute.For<IGraphContext>();
      stubContext.HasTriggerParameterKey(Arg.Any<string>()).Returns(false);

      IGraphContextFactory stubFactory = Substitute.For<IGraphContextFactory>();
      stubFactory.MakeContext().Returns(stubContext);
      GraphContextFactoryLocator.Provide(stubFactory);

      bool entered = false;
      nodeB.OnEnter += () => { entered = true; };

      graph.Start();
      Assert.IsFalse(entered);
    }
  }
}