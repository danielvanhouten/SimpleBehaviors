using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimpleBehaviors
{
    public class Scenario
    {
        private readonly Queue<Step> _innerSteps;
        internal bool IsRunning { get; set; }
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

        internal void AddStep(Step step)
        {
            _innerSteps.Enqueue(step);
        }

        public GivenStep Given(Action aContext)
        {
            var step = new GivenStep(this, aContext);
            return step;
        }
        
        public WhenStep When(Action thisHappens)
        {
            var step = new WhenStep(this, thisHappens);
            return step;
        }
        
        internal void Run()
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
