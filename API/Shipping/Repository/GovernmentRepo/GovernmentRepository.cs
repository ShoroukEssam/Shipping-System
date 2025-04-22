using Shipping.Models;

namespace Shipping.Repository.GovernmentRepo
{
    public class GovernmentRepository : IGovernmentRepository
    {
        private readonly ShippingContext _myContext;

        public GovernmentRepository(ShippingContext myContext)
        {
            _myContext = myContext;
        }

        public void Add(Government government)
        {
            _myContext.Governments.Add(government);
            _myContext.SaveChanges();
        }

        public List<Government> GetAll()
        {
            return _myContext.Governments.Where(g => !g.IsDeleted).ToList();
        }

        public List<string> GetAllNames()
        {
            return _myContext.Governments.Where(g => !g.IsDeleted && g.Status).Select(g => g.Name).ToList();
        }

        public Government GetById(int id)
        {
            return _myContext.Governments.FirstOrDefault(g => g.Id == id && !g.IsDeleted);
        }

        public void Update(int id, Government government)
        {
            var oldGovernment = GetById(id);
            if (oldGovernment != null)
            {
                oldGovernment.Name = government.Name;
                oldGovernment.Status = government.Status;
                oldGovernment.IsDeleted = government.IsDeleted;

                _myContext.SaveChanges();
            }
        }

        public void UpdateStatus(Government government, bool status)
        {
            if (government != null)
            {
                government.Status = status;
                _myContext.SaveChanges();
            }
        }
    }
}

