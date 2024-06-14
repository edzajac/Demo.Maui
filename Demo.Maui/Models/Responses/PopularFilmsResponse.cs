namespace Demo.Maui.Models.Responses;

public class PopularFilmsResponse
{
    public int page { get; set; }
    public IEnumerable<Film>? results { get; set; }
}

