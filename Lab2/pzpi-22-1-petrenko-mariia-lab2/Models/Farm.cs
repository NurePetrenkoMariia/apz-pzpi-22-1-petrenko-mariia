namespace FarmKeeper.Models
{
    public class Farm
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public ICollection<User> Workers { get; set; }
        public ICollection<User> Administrators { get; set; }
        public ICollection<Stable> Stables { get; set; }

    }
}
