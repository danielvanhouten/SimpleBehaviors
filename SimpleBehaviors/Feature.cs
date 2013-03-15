using System.Collections.Generic;
using NUnit.Framework;
using System.Dynamic;

namespace SimpleBehaviors
{
    [TestFixture]
    public class Feature
    {
        /// <summary>
        /// Scenario Context facilitates the transfer of information between steps without 
        /// the need to create a variable each time.
        /// </summary>
        public static dynamic ScenarioContext;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            BeforeFeature();
        }

        [SetUp]
        public void TestSetUp()
        {   
            ScenarioContext = new ExpandoObject();
            BeforeScenario();
        }

        [TearDown]
        public void TestTearDown()
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
        public virtual void AfterFeature(){}
    }
}
