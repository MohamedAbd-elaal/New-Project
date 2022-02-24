using System.Collections.Generic;

namespace MVC_FILE_VIEW_.Model
{
    public interface IEmployee
    {
        public Employee GetName(int id);
        public IEnumerable<Employee> IenumMethod();
        public Employee Add(Employee employee);
        public Employee Update(Employee changes);
        public Employee Delete(int id);
    }
}
