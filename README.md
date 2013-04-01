<h2> Simple Behaviors </h2>
<i>A simple and easy to use BDD framework built on NUnit</i>


<h2>Sample Syntax</h2>


<h3>The Story</h3>

``` csharp
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

``` 


<h3>Test Structure #1: Using Inheritence</h3>

``` csharp
public class HireAnEmployee : HireAnEmployeeSteps
{
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
}

// The following "Steps" class contains one method for each
// step of the above feature.

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

```


<h3>The Test Console Output</h3>
If a test fails, you can easily find the point of failure
by examining the console output

``` csharp

Scenario: Hiring An Employee
----------------------------
Given a company with no employees (passed)
  And a person (passed)
When the company attempts to hire the person (passed)
Then they should be an employee of the company (passed)
  And they should have an employee id (passed)
``` 
<br>

Should a test fail, this will be the output:
``` csharp
Scenario: Hiring An Employee
----------------------------
Given a company with no employees (passed)
  And a person (passed)
When the company attempts to hire the person (passed)
Then they should be an employee of the company  <<< FAIL

-- Scenario Failed --
``` 

<br />

<h3>Test Structure #2: Using Step Container Classes</h3>

In this case, the class containing the tests doesn't need to inherit from
anything. You can tell the Scenario where the steps will come from.

This is helpful if you have a slot of steps, and perhaps want to
break them up into more manageable bits.


``` csharp
public class  HireAnEmployee
{
        [Test]
        public void HiringAnEmployee()
        {
            new Scenario<HireAnEmployeeSteps>()
                .Given( s => s.a_company_with_no_employees() )
                .And( s => s.a_person() )
                .When( s  => s.the_company_attempts_to_hire_the_person() )
                .Then( s => s.they_should_be_an_employee_of_the_company() )
                .And( s => s.they_should_have_an_employeeId() )
                .Run();
        }

  // ... omitted for brevity ...
}
``` 