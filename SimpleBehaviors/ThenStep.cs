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
    }
}
