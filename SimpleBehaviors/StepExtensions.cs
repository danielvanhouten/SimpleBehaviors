using System;

namespace SimpleBehaviors
{
    public static class StepExtensions
    {
        #region Given Step Extensions
        
        public static WhenStep When(this GivenStep givenStep, Action thisHappens)
        {
            var nextStep = new WhenStep(givenStep.ScenarioReference, thisHappens);
            return nextStep;
        }
        public static GivenStep And(this GivenStep givenStep, Action anotherContext)
        {
            var nextStep = new GivenStep(givenStep.ScenarioReference, StepPrefix.And, anotherContext);
            return nextStep;
        }

        #endregion

        #region When Step Extensions

        public static ThenStep Then(this WhenStep whenStep, Action thisShouldBeTrue)
        {
            var nextStep = new ThenStep(whenStep.ScenarioReference, thisShouldBeTrue);
            return nextStep;
        }

        public static WhenStep And(this WhenStep whenStep, Action thisHappens)
        {
            var nextStep = new WhenStep(whenStep.ScenarioReference, StepPrefix.And, thisHappens);
            return nextStep;
        }

        #endregion

        #region Then Step Extensions

        public static ThenStep And(this ThenStep whenStep, Action thisShouldAlsoBeTrue)
        {
            var nextStep = new ThenStep(whenStep.ScenarioReference, StepPrefix.And, thisShouldAlsoBeTrue);
            return nextStep;
        }

        #endregion
    }
}
