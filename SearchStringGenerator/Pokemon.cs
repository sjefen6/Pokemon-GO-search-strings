namespace SearchStringGenerator;

public class Pokemon(JToken token)
{
    private const string Data = "data";
    private const string PokemonSettings = "pokemonSettings";

    public bool IsPokemon => GetPokemonId() != null;
    public string PokemonId => GetPokemonId() ?? throw new InvalidOperationException();
    public int KmBuddyDistance => GetKmBuddyDistance();
    public int? PurificationStardustNeeded => GetPurificationStardustNeeded();
    public int PokemonNumber => GetPokemonNumber();
    public string? Form => GetForm();
    public string TemplateId => GetTemplateId();

    private string? GetPokemonId() => token[Data]?[PokemonSettings]?["pokemonId"]?.ToString();
    private int GetKmBuddyDistance() => GetInt(token[Data]?[PokemonSettings]?["kmBuddyDistance"]) ?? throw new InvalidOperationException();
    private int? GetPurificationStardustNeeded() => GetInt(token[Data]?[PokemonSettings]?["shadow"]?["purificationStardustNeeded"]);
    private string? GetForm() => token[Data]?[PokemonSettings]?["form"]?.ToString();
    private string GetTemplateId() => token["templateId"]?.ToString() ?? throw new InvalidOperationException();

    private int GetPokemonNumber()
    {
        const string pattern = @"V(\d+)_POKEMON_";
        var match = Regex.Match(TemplateId, pattern);
        var number = match.Groups[1].Value;
        return int.Parse(number);
    }
    
    private static int? GetInt(JToken? value)
    {
        if (value == null)
        {
            return null;
        }
        return int.TryParse(value.ToString(), out var result) ? result : (int?)null;
    }
}