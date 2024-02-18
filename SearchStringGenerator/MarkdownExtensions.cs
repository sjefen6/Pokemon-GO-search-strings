namespace SearchStringGenerator;

public static class MarkdownExtensions
{
    public static void AddSearchStringTable(this StringBuilder sb, string title, Dictionary<string, string> searchStrings, Func<string, string> nameDecorator)
    {
        sb.AppendLine($"## {title}")
            .AppendLine("| Name | Search string |")
            .AppendLine("|------|---------------|");

        foreach (var item in searchStrings)
        {
            sb.AppendLine($"| `{nameDecorator(item.Key)}` | `{item.Value}` |");
        }
    }
}