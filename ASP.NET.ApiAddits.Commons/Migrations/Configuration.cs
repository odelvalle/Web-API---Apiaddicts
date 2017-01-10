namespace ContosoUniversity.Migrations
{
    using ContosoUniversity.DAL;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SchoolContext context)
        {
            context.Seed();
        }
    }
}
