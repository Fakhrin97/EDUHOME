namespace EDUHOME.DAL.Entities
{
    public class Category :Entity
    {
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
