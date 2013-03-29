using System;

namespace SimpleBehaviors
{
    public class ThenStep : Step
    {
        public ThenStep(Scenario scenario, Action methodToRun)
            : base(scenario, StepPrefix.Then, methodToRun)
        {
        }

        public ThenStep(Scenario scenario, StepPrefix stepPrefix, Action methodToRun)
            : base(scenario, stepPrefix, methodToRun) { }

        public new void Run()
        {
            base.Run();
        }
    }

    public class ThenStep<TSteps> : ThenStep
    {
        public ThenStep( Scenario scenario, Action methodToRun )
            : base( scenario, StepPrefix.Then, methodToRun )
        {
        }

        public ThenStep( Scenario scenario, StepPrefix stepPrefix, Action methodToRun )
            : base( scenario, stepPrefix, methodToRun ) { }
    }
}
