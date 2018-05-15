using System.Linq;
using LearningCenter.Repository;

namespace LearningCenter.Business
{
    public interface ICartManager
    {
        CartModel Add(int userId, int classId);
        void Enroll(int userId, int classId);
        bool Remove(int userId, int productId);
        CartModel[] GetAllClasses(int userId);
    }

    public class CartModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    class CartManager : ICartManager
    {
        private readonly ICartRepository cartRepository;
        private readonly IClassRepository classRepository;
        public CartManager(ICartRepository cartRepository, IClassRepository classRepository)
        {
            this.cartRepository = cartRepository;
            this.classRepository = classRepository;
        }
        public CartModel Add(int userId, int classId)
        {
            var item = cartRepository.Add(userId, classId);
            var subject = classRepository.ClassById(classId);
            return new CartModel
            {
                Id = subject.Id,
                Name = subject.Name,
                Price = subject.Price
            };
        }

        public void Enroll(int userId, int classId)
        {
            cartRepository.Enroll(userId, classId);
        }
        public CartModel[] GetAllClasses(int userId)
        {
            var classes = cartRepository.GetAllClasses(userId)
                .Select(t =>
                {
                    var subject = classRepository.ClassById(t.ClassId);
                    return new CartModel
                    {
                        Id = subject.Id,
                        Name = subject.Name,
                        Price = subject.Price
                    };
                })
                .ToArray();
            return classes;
        }
        public bool Remove(int userId, int classId)
        {
            return cartRepository.Remove(userId, classId);
        }
    }
}
