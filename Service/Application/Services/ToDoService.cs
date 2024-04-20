﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Examples.Service.Application.Dtos;
using Examples.Service.Application.Interfaces;
using Examples.Service.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examples.Service.Application.Services
{
    public class TodoService : ITodoService
    {
        private readonly IDbContext dbContext;
        private readonly IMapper mapper;

        public TodoService(IDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public Task<TodoDto> AddAsync(TodoDto todo)
        {
            todo.Id = todo.Id ?? Guid.NewGuid();
            todo.CreateDate = DateTime.Now;
            todo.UpdateDate = DateTime.Now;
            dbContext.Add(mapper.Map<TodoEntity>(todo));
            return Task.FromResult(todo);
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
            return mapper.Map<TodoDto>(dbTodo);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            dbContext.Remove(await dbContext.Query<TodoEntity>().FirstOrDefaultAsync(x => x.Id == id));
            await dbContext.SaveChangesAsync();
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