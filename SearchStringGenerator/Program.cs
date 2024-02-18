var pokemons = await GameDataParser.GetPokemons();

var markdown = new StringBuilder();
markdown.AppendLine("# Pokemon GO search strings")
        .AppendLine("This project reads the game master file and generates search strings for Pokemon GO."
                    + " The search strings can be used to filter Pokemon using the in the game search.")
        .AppendLine("## Usage")
        .AppendLine("Copy the search string and paste it in the search bar in Pokemon GO.")
        .AppendLine()
        .AppendLine("To save a search string:")
        .AppendLine(" 1. Clear the search string from the search bar")
        .AppendLine(" 1. [See more] to reveal Recent searches")
        .AppendLine(" 1. Long press on the search string under Recent to add it to favorites")
        .AppendLine(" 1. Long press on the search string under Favorites and give the search a name")
        .AppendLine()
        .AppendLine("![Screenshot from Favorite searches inside the game with all the search strings added](screenshot.png)");

markdown.AddSearchStringTable("BuddyDistance", pokemons.DistanceSearchStrings(), x => $"Buddy {x} km");
markdown.AddSearchStringTable("PurificationStardustNeeded", pokemons.PurificationStardustSearchStrings(), x => $"Purify {x}", x => $"shadow&{x}");

markdown.AppendLine("# Regenerating the README.md file")
        .AppendLine("To regenerate the README.md file, run the `SearchStringGenerator` project.")
        .AppendLine("```")
        .AppendLine("dotnet run --project SearchStringGenerator/SearchStringGenerator.csproj")
        .AppendLine("```");

FileHandler.SaveFile(markdown.ToString(), FileHandler.Readme);

Console.WriteLine("Markdown file has been successfully generated and saved.");