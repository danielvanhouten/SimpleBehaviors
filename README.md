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


<h3>Using Inheritence</h3>

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





<br />

<h3>Using Step Container Classes</h3>
... docs coming soon