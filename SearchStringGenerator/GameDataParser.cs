namespace SearchStringGenerator;

public class GameDataParser
{
    public static async Task<List<Pokemon>> GetPokemons()
    {
        const string gameMastersUrl =
            @"https://raw.githubusercontent.com/PokeMiners/game_masters/master/latest/latest.json";
        await FileDownloader.DownloadFileIfChangedAsync(gameMastersUrl, GameMastersFile);
        var gameMastersData = LoadGameMastersData(GameMastersFile);
        return FilterAndGroupPokemonData(gameMastersData).ToList();
    }

    private static JArray LoadGameMastersData(string filePath)
    {
        using var streamReader = new StreamReader(filePath);
        using var jsonReader = new JsonTextReader(streamReader);
        return JArray.Load(jsonReader);
    }

    private static IEnumerable<Pokemon> FilterAndGroupPokemonData(JArray gameMastersData)
    {
        var pokemons = gameMastersData
            .Select(x => new Pokemon(x))
            .Where(x => x.IsPokemon)
            .GroupBy(x => new { x.PokemonId, x.KmBuddyDistance, x.PurificationStardustNeeded })
            .Select(g => g.First());
        return pokemons;
    }

    public static string GameMastersFile => FileHandler.PathFromSolution(@"game_masters.json");
}