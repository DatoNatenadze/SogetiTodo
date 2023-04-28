using SogetiTODO.Domain.POCOs;
using SogetiTODO.Repositories.Abstractions;

namespace SogetiTODO.Repositories.Implementations;

public class DataSeeder : IDataSeeder
{
    private readonly List<Todo> _todos = new()
    {
        new Todo
        {
            Id = Guid.NewGuid(),
            Title = "Create a Todo app",
            Description =
                "Develop an application for a product owner with a 'groundbreaking', 'revolutionary', and 'never-seen-before' idea",
            FinishUntil = DateTime.Now.AddHours(3)
        },
        new Todo
        {
            Id = Guid.NewGuid(),
            Title = "Debug using horoscope advice",
            Description = "Tackle your next bug by consulting your daily horoscope for some unconventional wisdom",
            FinishUntil = DateTime.Now.AddHours(5)
        },
        new Todo
        {
            Id = Guid.NewGuid(),
            Title = "Debug the unexplainable",
            Description = "Find and fix a bug so elusive that it makes the Loch Ness Monster seem like a regular visitor",
            FinishUntil = DateTime.Now.AddDays(2)
        },
        new Todo
        {
            Id = Guid.NewGuid(),
            Title = "Buy groceries",
            Description = "Milk, eggs, bread, and a few other things (casual and banal)",
            FinishUntil = DateTime.Now.AddDays(1)
        }
    };

    public async Task<List<Todo>> SeedTodos()
    {
        return await Task.FromResult(_todos);
    }
}