using GuessNumber.Settings;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace GuessNumber;

public class Oracle
{
    private readonly AppSettingsModel? _settings;
    private int _victoryNumber;

    public Oracle(IConfiguration configuration)
    {
        _settings = configuration.GetRequiredSection("AppSettings").Get<AppSettingsModel>();
        _victoryNumber = new Random().Next(_settings!.DownEdge, _settings.UpEdge + 1);
    }

    public void Header()
    {
        AnsiConsole.Write(
            new FigletText("Guess Number")
                .LeftJustified()
                .Color(Color.Red));

        AnsiConsole.MarkupLineInterpolated(
            $"[red]Can you guest the number from {_settings!.DownEdge} to {_settings.UpEdge}?[/]");
    }
    public bool Prediction()
    {
        const bool endGame = false;
        
        AnsiConsole.Markup("Please type your number: ");
        
        if (!int.TryParse(Console.ReadLine(), out var number))
        {
            AnsiConsole.MarkupLine("You need write the number! [#ffba33]Try again.[/]");
            return !endGame;
        }

        if (number > _settings!.UpEdge || number < _settings.DownEdge)
        {
            AnsiConsole.MarkupLine(
                $"Please type number from {_settings.DownEdge} to {_settings.UpEdge}. [#ffba33]Try again.[/]");
            return !endGame;
        }

        if (number == _victoryNumber)
        {
            AnsiConsole.MarkupLine("[#c833ff]You win!!![/]");
            
            var selected = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Do you want [red]play[/] again?")
                    .PageSize(10)
                    .AddChoices("YES", "NO"));

            if (selected == "NO")
            {
                AnsiConsole.MarkupLine("[bold red]GAME OVER![/]");
                return endGame;
            }
            
            _victoryNumber = new Random().Next(_settings!.DownEdge, _settings.UpEdge + 1);
            AnsiConsole.MarkupLine("[#33ff62]Welcome to new game![/]");
            return !endGame;
        }

        if (number > _victoryNumber)
        {
            AnsiConsole.MarkupLine("Your number is [bold blue]bigger[/]. Try again.");
            return !endGame;
        }

        AnsiConsole.MarkupLine("Your number is [bold green]smaller[/]. Try again.");
        return !endGame;
    }
}