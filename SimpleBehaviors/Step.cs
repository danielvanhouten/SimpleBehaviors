﻿using System;

namespace SimpleBehaviors
{
    public class Step 
    {
        protected StepPrefix Prefix { get; set; }
        protected Action MethodToRun { get; set; }
        public Scenario Scenario { get; set; }

        public Step(Scenario scenario, StepPrefix stepPrefix, Action methodToRun)
        {
            Prefix = stepPrefix;
            MethodToRun = methodToRun;
            Scenario = scenario;
            Scenario.AddStep(this);
        }

        internal void Run()
        {
            // Steps will likely fail if run alone. If the scenario
            // the step belongs to isn't running, then run the scenario instead.
            if (!Scenario.IsRunning){
                Scenario.Run();
                return;
            }

            if (Prefix == StepPrefix.And){
                Console.Write("  ");
            }

            Console.Write(Prefix + " ");
            Console.Write(MethodToRun.Method.Name.Wordify());

            try
            {
                MethodToRun();
                Console.WriteLine(" (passed)");
            }
            catch
            {
                Console.WriteLine("  <<< FAIL");
                throw;
            }
        }
    }
}
