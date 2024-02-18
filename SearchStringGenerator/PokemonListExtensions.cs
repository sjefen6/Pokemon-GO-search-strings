namespace SearchStringGenerator;

public static class PokemonListExtensions
{
    public static Dictionary<string, string> PurificationStardustSearchStrings(this List<Pokemon> pokemons)
    {
        return pokemons.GenerateSearchStrings(x => x.PurificationStardustNeeded);
    }

    public static Dictionary<string, string> DistanceSearchStrings(this List<Pokemon> pokemons)
    {
        return pokemons.GenerateSearchStrings(x => x.KmBuddyDistance);
    }
    
    public static Dictionary<string, string> CandyToEvolve(this List<Pokemon> pokemons)
    {
        return pokemons.GenerateSearchStrings(x => x.candyToEvolve);
    }
    
    private static Dictionary<string, string> GenerateSearchStrings(this IReadOnlyCollection<Pokemon> pokemons, Func<Pokemon, int?> propertySelector)
    {
        var distinctPropertyValues = pokemons
            .Select(propertySelector)
            .Where(value => value.HasValue)
            .Select(value => value!.Value)
            .Distinct()
            .ToList();
        distinctPropertyValues.Sort();

        var searchStrings = new Dictionary<string, string>();

        foreach (var propertyValue in distinctPropertyValues)
        {
            var pokemonSubset = pokemons
                .Where(pokemon => propertySelector(pokemon) == propertyValue)
                .ToList();

            var searchString = pokemonSubset.GetRanges().ToSearchString();

            searchStrings.Add(propertyValue.ToString(), searchString);
        }

        return searchStrings;
    }

    private static List<string> GetRanges(this IEnumerable<Pokemon> pokemons)
    {
        var numbers = pokemons.Select(x => x.PokemonNumber).Distinct().ToList();
        numbers.Sort();

        var ranges = new List<string>();
        var start = numbers[0];

        for (var i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] != numbers[i - 1] + 1)
            {
                ranges.Add(start == numbers[i - 1] ? start.ToString() : $"{start}-{numbers[i - 1]}");
                start = numbers[i];
            }
        }

        ranges.Add(start == numbers[^1] ? start.ToString() : $"{start}-{numbers[^1]}");
        return ranges;
    }
    
    private static string ToSearchString(this IEnumerable<string> searchTerms)
    {
        return string.Join(",", searchTerms);
    }
}