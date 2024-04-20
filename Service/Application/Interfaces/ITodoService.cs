using Examples.Service.Application.Dtos;

namespace Examples.Service.Application.Interfaces
{
    public interface ITodoService
    {
        Task<TodoDto> AddAsync(TodoDto todo);
        Task<bool> DeleteAsync(Guid id);
        Task<List<TodoDto>> GetAllAsync();
        Task<TodoDto> GetAsync(Guid id);
        Task<TodoDto> UpdateAsync(TodoDto todo);
    }
}