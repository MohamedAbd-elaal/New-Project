using Microsoft.EntityFrameworkCore;

namespace MVC_FILE_VIEW_.Model
{
    public static class ModelBuilderExtentions
    {
        public static void seed(this ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Mohamed",
                    Hoppies = Dept.Zamalek
                },
                new Employee
                {
                    Id = 2,
                    Name = "Essam",
                    Hoppies = Dept.Ahaly
                },
                new Employee
                {
                    Id = 3,
                    Name = "Walid",
                    Hoppies = Dept.Pyramids
                },
                new Employee
                {
                    Id = 4,
                    Name = "Hamada",
                    Hoppies = Dept.Pyramids
                }
                );
        }
    }
}
