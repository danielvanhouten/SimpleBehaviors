using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

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

        public IEnumerable<Step> Steps { get; set; }

        public virtual GivenStep Given(Action context)
        {
            var step = new GivenStep(this, context);
            return step;
        }
        
        public virtual WhenStep When(Action thisHappens)
        {
            var step = new WhenStep(this, thisHappens);
            return step;
        }

        internal void AddStep( Step step )
        {
            _innerSteps.Enqueue( step );
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

        protected virtual void PrintScenarioName()
        {
            var parentMethodName = new StackTrace().GetFrame( 2 ).GetMethod().Name;
            Console.WriteLine( "Scenario: " + parentMethodName.Wordify( StringCase.Title ) );
            Console.WriteLine( "----------------------------" );
        }
    }

    public class Scenario<TSteps> : Scenario
    {
        public readonly TSteps StepsContainer;

        public Scenario()
        {
            StepsContainer = Activator.CreateInstance<TSteps>();
        }

        public GivenStep<TSteps> Given( Expression<Action<TSteps>> context )
        {
            var method = context.ConvertMethodExpressionToAction(StepsContainer);
            return new GivenStep<TSteps>( this, method );
        }

        public WhenStep When( Expression<Action<TSteps>> thisHappens )
        {
            var method = thisHappens.ConvertMethodExpressionToAction( StepsContainer );
            return new WhenStep( this, method );
        }

        override protected void PrintScenarioName()
        {
            var parentMethodName = new StackTrace().GetFrame( 3 ).GetMethod().Name;
            Console.WriteLine( "Scenario: " + parentMethodName.Wordify( StringCase.Title ) );
            Console.WriteLine( "----------------------------" );
        }
    }
}
