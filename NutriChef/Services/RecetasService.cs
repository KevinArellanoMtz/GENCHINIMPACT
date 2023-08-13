using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

public class RecetaService
{
    private readonly SQLiteAsyncConnection _database;

    public RecetaService(string databasePath)
    {
        _database = new SQLiteAsyncConnection(databasePath);
        _database.CreateTableAsync<Recetas>().Wait();
    }

    public Task<List<Recetas>> GetRecetasAsync()
    {
        return _database.Table<Recetas>().ToListAsync();
    }

    public Task<Recetas> GetRecetaAsync(int id)
    {
        return _database.Table<Recetas>().FirstOrDefaultAsync(r => r.Id == id);
    }

    public Task<int> SaveRecetaAsync(Recetas receta)
    {
        if (receta.Id != 0)
        {
            return _database.UpdateAsync(receta);
        }
        else
        {
            return _database.InsertAsync(receta);
        }
    }

    public Task<int> DeleteRecetaAsync(Recetas receta)
    {
        return _database.DeleteAsync(receta);
    }
}
