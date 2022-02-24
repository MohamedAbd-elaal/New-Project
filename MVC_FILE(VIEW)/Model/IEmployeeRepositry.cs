using System.Collections.Generic;
using System.Linq;
using MVC_FILE_VIEW_.Model;

namespace MVC_FILE_VIEW_.Model
{
    public class IEmployeeRepositry : IEmployee
    {
        List<Employee> list = new List<Employee>()
        {
            new Employee(){Id=5,Hoppies=Dept.Ahaly ,Name="Mohamed"},
            new Employee(){Id=8,Hoppies=Dept.Zamalek,Name="Essam"},
            new Employee(){Id=9,Hoppies=Dept.Pyramids,Name="Walid"},

        };

        public Employee Add(Employee employee)
        {
            employee.Id=list.Max(m => m.Id) + 1;
            list.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee= list.FirstOrDefault(e => e.Id == id);
           if(employee!=null)
            {
                list.Remove(employee);
            }
            return employee;
        }

        public Employee GetName(int id)
        {
            return this.list.FirstOrDefault(e => e.Id == id);
        }
        public IEnumerable<Employee> IenumMethod()
        {
            return list;
        }

        public Employee Update(Employee changes)
        {

            Employee employee = list.FirstOrDefault(e => e.Id == changes.Id);
            if (employee != null)
            {
                 employee.Id= changes.Id;
                 employee.Name= changes.Name;
                 employee.Hoppies= changes.Hoppies;

            }
            return employee;
        }
    }
}
