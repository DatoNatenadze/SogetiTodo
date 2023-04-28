using SogetiTODO.Domain.POCOs;

namespace SogetiTODO.Repositories.Abstractions;

public interface IDataSeeder
{
    Task<List<Todo>> SeedTodos();
}