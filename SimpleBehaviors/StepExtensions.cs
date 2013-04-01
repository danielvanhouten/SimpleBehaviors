using System;
using System.Linq.Expressions;

namespace SimpleBehaviors
{
    public static class StepExtensions
    {
        #region Given Step Extensions
        public static WhenStep When(this GivenStep givenStep, Action thisHappens)
        {
            var nextStep = new WhenStep(givenStep.Scenario, thisHappens);
            return nextStep;
        }
        public static WhenStep<TSteps> When<TSteps>( this GivenStep<TSteps> givenStep, Expression<Action<TSteps>> thisHappens )
        {
            var action = TryBuildStepAction( givenStep, thisHappens );
            return new WhenStep<TSteps>(givenStep.Scenario, action);
        }

        public static GivenStep And(this GivenStep givenStep, Action anotherContext)
        {
            var nextStep = new GivenStep(givenStep.Scenario, StepPrefix.And, anotherContext);
            return nextStep;
        }
        public static GivenStep<TSteps> And<TSteps>( this GivenStep<TSteps> givenStep, Expression<Action<TSteps>> anotherContext )
        {
            var action = TryBuildStepAction( givenStep, anotherContext );
            return new GivenStep<TSteps>( givenStep.Scenario, StepPrefix.And, action );
        }
        #endregion


        #region When Step Extensions
        public static ThenStep Then(this WhenStep whenStep, Action thisShouldBeTrue)
        {
            var nextStep = new ThenStep(whenStep.Scenario, thisShouldBeTrue);
            return nextStep;
        }
        public static ThenStep<TSteps> Then<TSteps>( this WhenStep<TSteps> whenStep, Expression<Action<TSteps>> thisShouldBeTrue )
        {
            var action = TryBuildStepAction( whenStep, thisShouldBeTrue );
            return new ThenStep<TSteps>( whenStep.Scenario, action);
        }

        public static WhenStep And(this WhenStep whenStep, Action thisHappens)
        {
            var nextStep = new WhenStep(whenStep.Scenario, StepPrefix.And, thisHappens);
            return nextStep;
        }
        public static WhenStep<TSteps> And<TSteps>( this WhenStep<TSteps> whenStep, Expression<Action<TSteps>> thisHappens )
        {
            var action = TryBuildStepAction( whenStep, thisHappens );
            return new WhenStep<TSteps>( whenStep.Scenario, action );
        }
        #endregion


        #region Then Step Extensions
        public static ThenStep And(this ThenStep thenStep, Action thisShouldAlsoBeTrue)
        {
            var nextStep = new ThenStep(thenStep.Scenario, StepPrefix.And, thisShouldAlsoBeTrue);
            return nextStep;
        }
        public static ThenStep<TSteps> And<TSteps>( this ThenStep<TSteps> thenStep, Expression<Action<TSteps>> thisShouldAlsoBeTrue )
        {
            var action = TryBuildStepAction( thenStep, thisShouldAlsoBeTrue );
            return new ThenStep<TSteps>( thenStep.Scenario, StepPrefix.And, action );
        }
        #endregion

        private static Action TryBuildStepAction<TSteps>( Step step, Expression<Action<TSteps>> anotherContext )
        {
            try
            {
                var stepsContainer = ((Scenario<TSteps>) step.Scenario).StepsContainer;
                return anotherContext.ConvertMethodExpressionToAction(stepsContainer);
            }
            catch(Exception e)
            {
                throw new Exception("Error Building an Action delegate from a step and expression", e);
            }
        }
    }
}
