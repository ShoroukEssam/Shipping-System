using Shipping.Models;

namespace Shipping.Repository.GovernmentRepo
{
    public interface IGovernmentRepository
    {
        List<Government> GetAll();
        List<string> GetAllNames();
        void Add(Government government);
        Government GetById(int id);
        void Update(int id, Government government);
        void UpdateStatus(Government government, bool status);
    }
}
