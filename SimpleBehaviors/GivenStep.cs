﻿using System;

namespace SimpleBehaviors
{
    public class GivenStep : Step
    {
        public GivenStep(Scenario scenario, Action methodToRun) 
            : base(scenario, StepPrefix.Given, methodToRun){}

        public GivenStep(Scenario scenario, StepPrefix stepPrefix, Action methodToRun)
            : base(scenario, stepPrefix, methodToRun) { }
    }


    public class GivenStep<TSteps> : GivenStep
    {
        public GivenStep(Scenario scenario, Action methodToRun) : base(scenario, methodToRun)
        {
        }

        public GivenStep(Scenario scenario, StepPrefix stepPrefix, Action methodToRun) : base(scenario, stepPrefix, methodToRun)
        {
        }
    }

}
