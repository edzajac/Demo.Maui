using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace Demo.Maui.Models;

public class Genre : ObservableObject
{
    public int id { get; set; }
    public string name { get; set; }

    [JsonIgnore]
    public List<Film> associated_films { get; set; }
}

