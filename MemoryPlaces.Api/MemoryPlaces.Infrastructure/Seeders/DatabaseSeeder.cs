using System.Collections.Generic;
using System.Linq;
using MemoryPlaces.Domain.Entities;
using MemoryPlaces.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace MemoryPlaces.Infrastructure.Seeders;

public class DatabaseSeeder
{
    private readonly MemoryPlacesDbContext _dbContext;

    public DatabaseSeeder(MemoryPlacesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Seed()
    {
        if (await _dbContext.Database.CanConnectAsync())
        {
            var pendingMigrations = _dbContext.Database.GetPendingMigrations();

            if (pendingMigrations != null && pendingMigrations.Any())
            {
                _dbContext.Database.Migrate();
            }

            if (!_dbContext.Roles.Any())
            {
                var roles = GetRoles();
                _dbContext.Roles.AddRange(roles);
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Types.Any())
            {
                var types = GetTypes();
                _dbContext.Types.AddRange(types);
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Periods.Any())
            {
                var periods = GetPeriods();
                _dbContext.Periods.AddRange(periods);
                await _dbContext.SaveChangesAsync();
            }

            if (!_dbContext.Categories.Any())
            {
                var categories = GetCategories();
                _dbContext.Categories.AddRange(categories);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>()
        {
            new Role() { Name = "User" },
            new Role() { Name = "Moderator" },
            new Role() { Name = "Admin" }
        };

        return roles;
    }

    private IEnumerable<Domain.Entities.Type> GetTypes()
    {
        var types = new List<Domain.Entities.Type>()
        {
            new()
            {
                PolishName = "Cmentarz wojenny",
                EnglishName = "War cemetery",
                GermanName = "Kriegsfriedhof",
                RussianName = "Военное кладбище",
            },
            new()
            {
                PolishName = "Cmentarz cywilny",
                EnglishName = "Civilian cemetery",
                GermanName = "Zivilfriedhof",
                RussianName = "Гражданское кладбище",
            },
            new()
            {
                PolishName = "Miejsce pochówku",
                EnglishName = "Burial place",
                GermanName = "Begräbnisstätte",
                RussianName = "Место захоронения",
            },
            new()
            {
                PolishName = "Miejsce egzekucji, straceń",
                EnglishName = "Place of execution",
                GermanName = "Hinrichtungsort",
                RussianName = "Место казни",
            },
            new()
            {
                PolishName = "Miejsce bitwy, potyczki",
                EnglishName = "Place of battle/skirmish",
                GermanName = "Ort der Schlacht/Gefecht",
                RussianName = "Место битвы, стычки",
            },
            new()
            {
                PolishName = "Miejsce archeologiczne",
                EnglishName = "Archaeological site",
                GermanName = "Archäologischer Fundort",
                RussianName = "Археологический памятник",
            },
            new()
            {
                PolishName = "Kapliczka",
                EnglishName = "Wayside shrine",
                GermanName = "Bildstock",
                RussianName = "Капличка",
            },
            new()
            {
                PolishName = "Zabytek",
                EnglishName = "Historic monument",
                GermanName = "Kulturdenkmal",
                RussianName = "Культурное наследие",
            },
            new()
            {
                PolishName = "Tablica upamiętniająca",
                EnglishName = "Memorial plaque",
                GermanName = "Erinnerungstafel",
                RussianName = "Памятная табличка",
            },
            new()
            {
                PolishName = "Pomnik upamiętniający",
                EnglishName = "Memorial monument",
                GermanName = "Gedenkdenkmal",
                RussianName = "Памятный монумент",
            }
        };

        return types;
    }

    private IEnumerable<Period> GetPeriods()
    {
        var periods = new List<Period>()
        {
            new Period()
            {
                PolishName = "Polska przed 3cim rozbiorem < 1795",
                EnglishName = "Poland before the Third Partition < 1795",
                GermanName = "Polen vor der dritten Teilung < 1795",
                RussianName = "Польша до третьего раздела < 1795",
            },
            new Period()
            {
                PolishName = "Wojny Napoleońskie 1799 – 1815",
                EnglishName = "Napoleonic Wars 1799–1815",
                GermanName = "Napoleonische Kriege 1799–1815",
                RussianName = "Наполеоновские войны 1799–1815",
            },
            new Period()
            {
                PolishName = "Polska po rozbiorach 1795 – 1914",
                EnglishName = "Poland after the Partitions 1795–1914",
                GermanName = "Polen nach den Teilungen 1795–1914",
                RussianName = "Польша после разделов 1795–1914",
            },
            new Period()
            {
                PolishName = "I Wojna Światowa 1914 – 1918",
                EnglishName = "World War I 1914–1918",
                GermanName = "Erster Weltkrieg 1914–1918",
                RussianName = "Первая мировая война 1914–1918",
            },
            new Period()
            {
                PolishName = "Powstanie Wielkopolskie 1918 - 1919",
                EnglishName = "Greater Poland Uprising 1918–1919",
                GermanName = "Großpolnischer Aufstand 1918–1919",
                RussianName = "Великопольское восстание 1918–1919",
            },
            new Period()
            {
                PolishName = "Okres Międzywojenny 1918 – 1939",
                EnglishName = "Interwar Period 1918–1939",
                GermanName = "Zwischenkriegszeit 1918–1939",
                RussianName = "Межвоенный период 1918–1939",
            },
            new Period()
            {
                PolishName = "II Wojna Światowa 1939 – 1945",
                EnglishName = "World War II 1939–1945",
                GermanName = "Zweiter Weltkrieg 1939–1945",
                RussianName = "Вторая мировая война 1939–1945",
            },
            new Period()
            {
                PolishName = "Okres Stalinowski 1945 – 1953",
                EnglishName = "Stalinist Period 1945–1953",
                GermanName = "Stalinistische Ära 1945–1953",
                RussianName = "Сталинский период 1945–1953",
            }
        };

        return periods;
    }

    private IEnumerable<Category> GetCategories()
    {
        var categories = new List<Category>()
        {
            new Category()
            {
                PolishName = "Istniejące",
                EnglishName = "Existing",
                GermanName = "Bestehende",
                RussianName = "Существующие",
            },
            new Category()
            {
                PolishName = "Nieistniejące",
                EnglishName = "Non-existing",
                GermanName = "Nicht bestehend",
                RussianName = "Не существующие ",
            }
        };

        return categories;
    }
}
