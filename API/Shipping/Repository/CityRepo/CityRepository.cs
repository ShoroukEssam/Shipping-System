using Microsoft.EntityFrameworkCore;
using Shipping.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shipping.Repository.CityRepo
{
    public class CityRepository : ICityRepository
    {
        private readonly ShippingContext _myContext;

        public CityRepository(ShippingContext myContext)
        {
            _myContext = myContext;
        }

        public void AddToGovernment(int governmentId, City city)
        {
            city.GovernmentId = governmentId;
            _myContext.Cities.Add(city);
            _myContext.SaveChanges();
        }

        public List<City> GetAllByGovernmentId(int governmentId)
        {
            return _myContext.Cities.Where(c => c.GovernmentId == governmentId && !c.IsDeleted).ToList();
        }

        public City GetById(int id)
        {
            return _myContext.Cities.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public IEnumerable<City> GetAllByGovernmentName(string governmentName)
        {
            return _myContext.Cities
                .Where(c => c.Government.Name == governmentName && !c.IsDeleted)
                .ToList();
        }

        public City GetByName(string city)
        {
            return _myContext.Cities
                .Where(c => c.Name == city && !c.IsDeleted)
                .FirstOrDefault();
        }

        public void Update(int id, City city)
        {
            var oldCity = GetById(id);
            if (oldCity != null)
            {
                oldCity.Name = city.Name;
                oldCity.Status = city.Status;
                oldCity.GovernmentId = city.GovernmentId;
                oldCity.PickUpPrice = city.PickUpPrice;
                oldCity.ShippingPrice = city.ShippingPrice;
                oldCity.IsDeleted = city.IsDeleted;

                _myContext.SaveChanges();
            }
        }
    }
}
