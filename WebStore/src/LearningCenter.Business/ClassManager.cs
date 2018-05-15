using System.Linq;
using LearningCenter.Repository;

namespace LearningCenter.Business
{
    public interface IClassManager
    {
        ClassModel[] ClassesByCategory(int categoryId);
        ClassModel[] ClassesByUser(int userId);
        ClassModel ClassById(int classId);
    }

    public class ClassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Sessions { get; set; }
    }

    public class ClassManager : IClassManager
    {
        private readonly IClassRepository classRepository;

        public ClassManager(IClassRepository classRepository)
        {
            this.classRepository = classRepository;
        }

        public ClassModel[] ClassesByCategory(int categoryId)
        {
            return classRepository.ClassesByCategory(categoryId)
                   .Select(t => new ClassModel
                   {
                       Id = t.Id,
                       Name = t.Name,
                       Description = t.Description,
                       Price = t.Price,
                       Sessions = t.Sessions
                   })
                .ToArray();
        }
        public ClassModel[] ClassesByUser(int userId)
        {
            return classRepository.ClassesByUser(userId)
                   .Select(t => new ClassModel
                   {
                       Id = t.Id,
                       Name = t.Name,
                       Description = t.Description,
                       Price = t.Price,
                       Sessions = t.Sessions
                   })
                .ToArray();
        }
        public ClassModel ClassById(int classId)
        {
            var classModel = classRepository.ClassById(classId);

            return new ClassModel
            {
                Id = classModel.Id,
                Name = classModel.Name,
                Description = classModel.Description,
                Price = classModel.Price,
                Sessions = classModel.Sessions
            };
        }
    }
}