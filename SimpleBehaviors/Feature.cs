using System.Collections.Generic;
using NUnit.Framework;

namespace SimpleBehaviors
{
    [TestFixture]
    public class Feature
    {
        public static Dictionary<string, object> ScenarioContext;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            BeforeFeature();
        }

        [SetUp]
        public void TestSetUp()
        {   
            ScenarioContext = new Dictionary<string, object>();
            BeforeScenario();
        }

        [TearDown]
        public void TearDown()
        {
            AfterScenario();
        }

        [TestFixtureSetUp]
        public void FixtureTearDown()
        {
            AfterFeature();
        }

        public virtual void BeforeScenario(){}
        public virtual void AfterScenario(){}
        public virtual void BeforeFeature(){}
        public virtual void AfterFeature(){ }
    }
}
