﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cities.API.Context;
using Cities.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cities.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext cityInfoContext)
        {
            _context = cityInfoContext ?? throw new ArgumentNullException(nameof(cityInfoContext));
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities.OrderBy(o=>o.Name).ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities.Include(c => c.PointsOfInterest)
                    .Where(c=>c.Id == cityId).FirstOrDefault();
            }
            return _context.Cities.Where(c => c.Id == cityId).FirstOrDefault();
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest.Where(p => p.CityId == cityId
            && p.Id == pointOfInterestId).FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest.Where(p => p.CityId == cityId)
                .ToList();
        }
        public bool CityExists(int cityId)
        {
            return _context.Cities.Any( c => c.Id == cityId);
        }

        public void AddPointOfInterest(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);

            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdatePointOfInterest(int cityId, PointOfInterest pointOfInterest)
        {

        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }
    }
}
