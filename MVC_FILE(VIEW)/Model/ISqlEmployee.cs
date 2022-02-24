using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MVC_FILE_VIEW_.Model
{
    public class ISqlEmployee : IEmployee
    {
        
        private readonly dbcontextclass context;
        private readonly ILogger<ISqlEmployee> logger;

        public ISqlEmployee(dbcontextclass context,ILogger<ISqlEmployee> logger)
        {
           this.context = context;
            this.logger = logger;
        }
        public Employee Add(Employee employee)
        {
            
            context.Employees.Add(employee);
            context.SaveChanges();
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee=context.Employees.Find(id);
            if(employee!=null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            return employee;
        }

        public Employee GetName(int id)
        {
            logger.LogTrace("logger Trace");
            logger.LogDebug("logger Debug");
            logger.LogInformation("logger Information");
            logger.LogWarning("logger Warning");
            logger.LogError("logger Error");
            logger.LogCritical("logger Critical");
            return context.Employees.Find(id);
        }

        public IEnumerable<Employee> IenumMethod()
        {
            return context.Employees;
        }

        public Employee Update(Employee changes)
        {
           var employee= context.Employees.Attach(changes);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return changes;
        }
    }
}
