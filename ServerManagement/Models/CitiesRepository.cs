namespace ServerManagement.Models;

public static class CitiesRepository
{
    private static readonly List<string> cities = new()
    {
        "Toronto",
        "Montreal",
        "Ottawa",
        "Calgary",
        "Halifax"
    };

    public static List<string> GetCities()
    {
        return cities;
    }
}