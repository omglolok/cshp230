using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningCenter.Repository
{
    public interface IClassRepository
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

    class ClassRepository : IClassRepository
    {
        public ClassModel[] ClassesByCategory(int categoryId)
        {
            return DatabaseAccessor.Instance.Categories
                .First(t => t.CategoryId == categoryId)
                .ClassMasters
                .Select(t =>
                    new ClassModel
                    {
                        Id = t.ClassId,
                        Name = t.ClassName,
                        Description = t.ClassDescription,
                        Price = t.ClassPrice,
                        Sessions = t.ClassSessions
                    })
                .ToArray();
        }
        public ClassModel[] ClassesByUser(int userId)
        {
            return DatabaseAccessor.Instance.Users
                .First(t => t.UserId == userId)
                .ClassMasters
                .Select(t =>
                    new ClassModel
                    {
                        Id = t.ClassId,
                        Name = t.ClassName,
                        Description = t.ClassDescription,
                        Price = t.ClassPrice,
                        Sessions = t.ClassSessions
                    })
                .ToArray();
        }
        public ClassModel ClassById(int classId)
        {
            return DatabaseAccessor.Instance.ClassMasters
                .Where(t => t.ClassId == classId)
                .Select(t => new ClassModel
                {
                    Id = t.ClassId,
                    Name = t.ClassName,
                    Description = t.ClassDescription,
                    Price = t.ClassPrice,
                    Sessions = t.ClassSessions
                })
                .First();
        }
    }
}
