using DAL.EF;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Entities;

namespace DAL.Repositories;

public class DogRepository : Repo<Dog, string>, IDogRepository
{
    public DogRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
}