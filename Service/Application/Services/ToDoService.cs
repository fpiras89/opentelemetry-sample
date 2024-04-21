using AutoMapper;
using AutoMapper.QueryableExtensions;
using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using Examples.Service.Application.Metrics;
using Examples.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examples.Service.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IDbContext dbContext;
        private readonly IMapper mapper;
        private readonly TodoMetrics todoMetrics;

        public TodoService(IDbContext dbContext, IMapper mapper, TodoMetrics todoMetrics)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.todoMetrics = todoMetrics;
        }

        public async Task<TodoDto> AddAsync(TodoDto todo)
        {
            todo.Id = todo.Id ?? Guid.NewGuid();
            todo.CreateDate = DateTime.Now;
            todo.UpdateDate = DateTime.Now;
            dbContext.Add(mapper.Map<TodoEntity>(todo));
            await dbContext.SaveChangesAsync();

            todoMetrics.TodoCreated();

            return todo;
        }

        public Task<TodoDto> GetAsync(Guid id)
        {
            return dbContext
                .Query<TodoEntity>()
                .ProjectTo<TodoDto>(mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<TodoDto>> GetAllAsync()
        {
            return dbContext
                .Query<TodoEntity>()
                .ProjectTo<TodoDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<TodoDto> UpdateAsync(TodoDto todo)
        {
            var dbTodo = await dbContext
                .Query<TodoEntity>()
                .FirstOrDefaultAsync();

            if (dbTodo == null)
            {
                return null;
            }

            dbTodo = mapper.Map(todo, dbTodo);
            dbContext.Update(dbTodo);
            await dbContext.SaveChangesAsync();

            if (dbTodo.Done.Value) todoMetrics.TodoDone();

            return mapper.Map<TodoDto>(dbTodo);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            dbContext.Remove(await dbContext.Query<TodoEntity>().FirstOrDefaultAsync(x => x.Id == id));
            await dbContext.SaveChangesAsync();

            todoMetrics.TodoDeleted();

            return true;
        }

        private class Mapping : Profile
        {
            public Mapping()
            {
                CreateMap<TodoDto, TodoEntity>().ReverseMap();
            }
        }
    }
}
