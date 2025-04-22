using Shipping.Models;
using System.Collections.Generic;
namespace Shipping.Repository.CityRepo
{
    public interface ICityRepository
    {
        void AddToGovernment(int governmentId, City city);
        List<City> GetAllByGovernmentId(int governmentId);
        IEnumerable<City> GetAllByGovernmentName(string government);
        City GetByName(string city);
        City GetById(int id);
        void Update(int id, City city);
    }
}

