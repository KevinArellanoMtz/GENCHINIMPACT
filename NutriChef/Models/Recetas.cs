using SQLite;

public class Recetas
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Categoria { get; set; }

    public string Calorias { get; set; }

    public byte[] Imagen { get; set; }
    public string Ingredientes { get; set; }
    public string Pasos { get; set; }
}
