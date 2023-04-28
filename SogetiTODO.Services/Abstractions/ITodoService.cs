using SogetiTODO.Domain.Filters;
using SogetiTODO.Services.Models.ServiceModels;

namespace SogetiTODO.Services.Abstractions;

public interface ITodoService
{
    Task<IEnumerable<TodoServiceModel>> GetAllAsync(PaginationFilter paginationFilter = null);
    Task<Guid> UpdateStateAsync(Guid id, bool IsCompleted);
    Task<TodoServiceModel> GetDetailsAsync(Guid id);
    Task<TodoServiceModel> AddAsync(TodoServiceModel todo);
    Task DeleteByIdAsync(Guid id);
}