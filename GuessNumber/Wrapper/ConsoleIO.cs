using Spectre.Console;
using Spectre.Console.Rendering;

namespace GuessNumber.Wrapper;

public class ConsoleIO : IConsoleIO
{
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    public void WriteLine(FormattableString value)
    {
        AnsiConsole.MarkupLineInterpolated(value);
    }

    public void WriteLine(string value)
    {
        AnsiConsole.MarkupLine(value);
    }

    public void Write(IRenderable value)
    {
        AnsiConsole.Write(value);
    }

    public void Write(FormattableString value)
    {
        AnsiConsole.MarkupInterpolated(value);
    }

    public void Write(string value)
    {
        AnsiConsole.Markup(value);
    }

    public string Prompt(IPrompt<string> value)
    {
        return AnsiConsole.Prompt(value);
    }

    public string Prompt()
    {
        var select = new SelectionPrompt<string>()
            .Title("Do you want [red]play[/] again?")
            .PageSize(10)
            .AddChoices("YES", "NO");
        
        return AnsiConsole.Prompt(select);
    }
}