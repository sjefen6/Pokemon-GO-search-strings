namespace TestProject1;

public class UnitTest1
{
    [Fact]
    public async Task GetPokemons_Should_Return_2_Lugias_With_Different_Purification_Cost()
    {
        var pokemons = await GameDataParser.GetPokemons();

        var lugias = pokemons
            .Where(pokemon => pokemon.PokemonId == "LUGIA")
            .ToList();
        
        lugias.First(x => x.Form == null).PurificationStardustNeeded.Should().Be(20000);
        lugias.First(x => x.Form == "LUGIA_S").PurificationStardustNeeded.Should().Be(5000);
    }

    [Fact]
    public async Task GetPokemons_ShouldReturnAtLeastFirst900Pokemon()
    {
        var pokemons = await GameDataParser.GetPokemons();
        
        var pokemonIds = pokemons.Select(pokemon => pokemon.PokemonNumber).Distinct().ToList();
        var expectedIds = Enumerable.Range(1, 900).ToList();
        
        pokemonIds.Should().Contain(expectedIds);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetPokemons_ReturnsExpectedNumberOfPokemons(bool deleteFiles)
    {
        // Arrange
        if (deleteFiles)
        {
            File.Delete(FileDownloader.GetEtagPath(GameDataParser.GameMastersFile));
        }

        // Act
        var pokemons = await GameDataParser.GetPokemons();

        // Assert
        pokemons.Should().HaveCountGreaterThan(1000);
    }
}