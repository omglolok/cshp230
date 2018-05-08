using LearningCenter.ProductDatabase;

namespace LearningCenter.Repository
{
    public class DatabaseAccessor
    {
        private static readonly SchoolEntities entities;

        static DatabaseAccessor()
        {
            entities = new SchoolEntities();
            entities.Database.Connection.Open();
        }

        public static SchoolEntities Instance
        {
            get
            {
                return entities;
            }
        }
    }
}
