using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleBehaviors
{
    public class Scenario : IRunnable
    {
        private readonly Queue<Step> _innerSteps;
        public bool IsRunning { get; protected set; }
        public Scenario()
        {
            PrintScenarioName(); 
            _innerSteps = new Queue<Step>();
        }

        private static void PrintScenarioName()
        {
            var parentMethodName = new StackTrace().GetFrame(2).GetMethod().Name;
            Console.WriteLine("Scenario: " + parentMethodName.Wordify(StringCase.Title));
            Console.WriteLine("----------------------------");
        }

        public IEnumerable<Step> Steps { get; set; }

        public void AddStep(Step step)
        {
            _innerSteps.Enqueue(step);
        }

        public GivenStep Given(Action aContext)
        {
            var step = new GivenStep(this, aContext);
            return step;
        }

        public void Run()
        {
            IsRunning = true;
            var step = _innerSteps.Dequeue();

            try
            {
                step.Run();
            }
            catch
            {
                Console.WriteLine("-- Scenario Failed --");
                throw;
            }

            if (_innerSteps.Any()){
                Run();
            }else{
                IsRunning = false;
            }
        }
    }
}
