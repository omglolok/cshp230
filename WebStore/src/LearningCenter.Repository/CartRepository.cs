using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningCenter.Repository
{
    public interface ICartRepository
    {
        CartModel Add(int userId, int classId);
        void Enroll(int userId, int classId);
        bool Remove(int userId, int classId);
        CartModel[] GetAllClasses(int userid);
    }
    public class CartModel
    {
        public int UserId { get; set; }
        public int ClassId { get; set; }
    }
    class CartRepository : ICartRepository
    {
        public CartModel Add(int userId, int classId)
        {
            var subject = DatabaseAccessor.Instance.UserCarts.Add(
                new ProductDatabase.UserCart
                {
                    UserId = userId,
                    ClassId = classId
                });
            DatabaseAccessor.Instance.SaveChanges();
            return new CartModel
            {
                UserId = subject.UserId,
                ClassId = subject.ClassId
            };
        }
        public void Enroll(int userId, int classId)
        {
            var subject = DatabaseAccessor.Instance.ClassMasters
                .First(t => t.ClassId == classId);
           DatabaseAccessor.Instance.Users
                .First(t => t.UserId == userId)
                .ClassMasters
                .Add(new ProductDatabase.ClassMaster
                {
                    ClassName = subject.ClassName,
                    ClassDescription = subject.ClassDescription,
                    ClassPrice = subject.ClassPrice,
                    ClassSessions = subject.ClassSessions,
                    ClassId = subject.ClassId
                });
            DatabaseAccessor.Instance.SaveChanges();
        }
        public CartModel[] GetAllClasses(int userId)
        {
            var classes = DatabaseAccessor.Instance.UserCarts
                .Where(t => t.UserId == userId)
                .Select(t => new CartModel
                {
                    UserId = t.UserId,
                    ClassId = t.ClassId
                })
                .ToArray();
            return classes;
        }

        public bool Remove(int userId, int classId)
        {
            var classes = DatabaseAccessor.Instance.UserCarts
                .Where(t => t.UserId == userId && t.ClassId == classId);
            if (classes.Count() == 0)
            {
                return false;
            }
            DatabaseAccessor.Instance.UserCarts.Remove(classes.First());
            DatabaseAccessor.Instance.SaveChanges();
            return true;
        }
    }
}
