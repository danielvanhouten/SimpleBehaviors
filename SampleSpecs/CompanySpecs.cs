using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Should;
using SimpleBehaviors;

namespace SampleSpecs
{
    public class Person
    {
        public virtual string Name { get; set; }
        public bool IsFelon { get; set; }
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
            if(person.IsFelon)
                throw new PolicyException("We cannot hire felons");

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
    // Confirmations: A person cannot be hired if they are a felon
    // ==============================================================

    public class  HireAnEmployee : HireAnEmployeeSteps
    {
        [Scenario]
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

        [Scenario]
        public void HiringAFelon()
        {
            new Scenario()
                .Given(a_company_with_no_employees)
                .And(a_person_whos_been_convicted_of_a_felon)
                .When(the_company_attempts_to_hire_the_person)
                .Then(a_policy_violation_should_occur)
                .And(the_person_should_not_become_an_employee)
                .Run();
        }
    }

    public class HireAnEmployeeSteps : Feature
    {
        private Person person;
        private Company company;

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
                ScenarioContext["ex"] = ex;
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
        protected void a_person_whos_been_convicted_of_a_felon()
        {
            person = new Person { Name = "John Brownington", IsFelon = true};
        }
        protected void a_policy_violation_should_occur()
        {
            var exception = (PolicyException) ScenarioContext["ex"];
           
            exception.Message.ShouldEqual("We cannot hire felons");
        }
        protected void the_person_should_not_become_an_employee()
        {
            company.Employees.ShouldBeEmpty();
        }
    }
}
