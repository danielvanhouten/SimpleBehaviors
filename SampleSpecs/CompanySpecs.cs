using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using NUnit.Framework;
using Should;
using SimpleBehaviors;

namespace SampleSpecs
{
    public class Messages
    {
        public const string CannotHireCriminals = "We cannot hire criminals";
    }
    public class PoliceRecord
    {
        private readonly IList<string> _crimes;

        public PoliceRecord()
        {
            _crimes = new List<string>();
        }

        public bool HasCrimes
        {
            get { return Crimes.Any(); }
        }

        public IEnumerable<string> Crimes 
        {
            get { return _crimes; }
        }

        public void AddCrime(string description)
        {
            _crimes.Add(description);
        }
    }
    public class Person
    {
        public virtual string Name { get; set; }
        public PoliceRecord PoliceRecord { get; private set; }

        public Person()
        {
            PoliceRecord = new PoliceRecord();
        }

    }
    public class Company
    {
        public ICollection<Employee> Employees { get; set; }

        public Company()
        {
            Employees = new List<Employee>();
        }
        public void Hire(Person person)
        {
            if(person.PoliceRecord.HasCrimes)
                throw new PolicyException(Messages.CannotHireCriminals);

            var employee = new Employee(person);
            Employees.Add(employee);
        }
    }
    public class Employee : Person
    {
        private readonly Person _person;
        public override string Name { get { return _person.Name; }}
        public Guid EmployeeId { get; protected set; }
        
        public Employee(Person person)
        {
            _person = person;
            EmployeeId = Guid.NewGuid();
        }
    }

    // ==============================================================
    // Story: Company Hires an Employee                        
    // --------------------------------------------------------------
    // Narrative: As a Company                                 
    //            I want to hire an employee                   
    //            So that I can distribute the incoming work      
    // --------------------------------------------------------------                                                          
    // Confirmations: A person cannot be hired if they have a criminal
    //                record.
    // ==============================================================

    public class  HireAnEmployee : HireAnEmployeeSteps
    {
        [Test]
        public void TestingGenericScenario()
        {
            new Scenario<TestStepsContainer>()
                .Given(x => x.I_set_something_up())
                .When(x => x.A_method_is_called())
                .And(x => x.Something_else_is_called())
                .Then(x => x.The_result_should_be_true())
                .Run();
        }

        [Test]
        public void HiringAnEmployee()
        {
            new Scenario()
                .Given(a_company_with_no_employees)
                .And(a_person)
                .When(the_company_attempts_to_hire_the_person)
                .Then(they_should_be_an_employee_of_the_company)
                .And(they_should_have_an_employeeId)
                .Run();
        }

        [Test]
        public void HiringAFelon()
        {
            new Scenario()
                .Given(a_company_with_no_employees)
                .And(a_person_whos_committed_a_crime)
                .When(the_company_attempts_to_hire_the_person)
                .Then(a_policy_violation_should_occur)
                .And(the_person_should_not_become_an_employee)
                .Run();
        }

        [Test]
        public void UsingDefaultConstructor_CompnayShouldBeInValidState()
        {
            new Scenario()
                .When(the_company_is_instantiated)
                .Then(it_should_be_in_a_valid_state)
                .Run();
        }

        private void the_company_is_instantiated()
        {
            company = new Company();
        }
        private void it_should_be_in_a_valid_state()
        {
            company.ShouldNotBeNull();
        }
    }

    public class HireAnEmployeeSteps : Feature
    {
        protected Person person;
        protected Company company;

        protected void a_company_with_no_employees()
        {
            company = new Company();
        }
        protected void a_person()
        {
            person = new Person { Name = "John Brownington" };
        }
        protected void the_company_attempts_to_hire_the_person()
        {
            try
            {
                company.Hire(person);
            }
            catch (Exception ex)
            {
                ScenarioContext.ThrownException = ex;
            }
            
        }
        protected void they_should_be_an_employee_of_the_company()
        {
            company.Employees.Any(x => x.Name == "John Brownington").ShouldBeTrue();
        }
        protected void they_should_have_an_employeeId()
        {
            company.Employees.Single(e => e.Name == "John Brownington")
                        .EmployeeId.ShouldNotEqual(Guid.Empty);
        }
        protected void a_person_whos_committed_a_crime()
        {
            person = new Person { Name = "John Brownington" };
            person.PoliceRecord.AddCrime("Armed Robery");
        }
        protected void a_policy_violation_should_occur()
        {
            var exception = ( PolicyException )ScenarioContext.ThrownException;

            exception.Message.ShouldEqual( Messages.CannotHireCriminals );
        }
        protected void the_person_should_not_become_an_employee()
        {
            company.Employees.ShouldBeEmpty();
        }
    }

    public class TestStepsContainer
    {
        public bool Result = false;

        public void I_set_something_up()
        {
            Console.WriteLine("-- ::setting something up:: --");
        }

        public void A_method_is_called()
        {
            Console.WriteLine( "-- ::calling a method:: --" );
        }

        public void Something_else_is_called()
        {
            Console.WriteLine( "-- ::calling another method:: --" );
            Result = true;
        }

        public void The_result_should_be_true()
        {
            Result.ShouldBeTrue();
        }
    }
}
