using System;

namespace SimpleBehaviors
{
    public class WhenStep : Step
    {
        public WhenStep(Scenario scenario, Action methodToRun)
            : base(scenario, StepPrefix.When, methodToRun)
        {
        }

        public WhenStep(Scenario scenario, StepPrefix stepPrefix, Action methodToRun)
            : base(scenario, stepPrefix, methodToRun)
        {
        }
    }


    public class WhenStep<TStep> : WhenStep
    {
        public WhenStep( Scenario scenario, Action methodToRun )
            : base( scenario, StepPrefix.When, methodToRun )
        {
        }

        public WhenStep( Scenario scenario, StepPrefix stepPrefix, Action methodToRun )
            : base( scenario, stepPrefix, methodToRun )
        {
        }
    }
}
