using DAL.Repositories.Base;
using Entities;

namespace DAL.Repositories.Interfaces;

public interface IDogRepository : IRepo<Dog, string> { }