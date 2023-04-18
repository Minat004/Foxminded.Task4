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
    
    public bool Guessed()
    {
        AnsiConsole.Markup("Please type your number: ");
        
        if (!int.TryParse(Console.ReadLine(), out var number))
        {
            CanNotParse();
            return false;
        }

        // switch (number)
        // {
        //     case var _ when number == _victoryNumber:
        //         return IsGameOver();
        //     case var _ when number > _victoryNumber:
        //         IsBigger();
        //         return false;
        //     case var _ when number < _victoryNumber:
        //         IsSmaller();
        //         return false;
        //     default:
        //         NotInRange();
        //         return false;
        // }

        if (number > _settings!.UpEdge || number < _settings.DownEdge)
        {
            NotInRange();
            return false;
        }

        if (number == _victoryNumber)
        {
            return IsGameOver();
        }

        if (number > _victoryNumber)
        {
            IsBigger();
            return false;
        }

        IsSmaller();
        return false;
    }

    private static void IsSmaller()
    {
        AnsiConsole.MarkupLine("Your number is [bold green]smaller[/]. Try again.");
    }

    private static void IsBigger()
    {
        AnsiConsole.MarkupLine("Your number is [bold blue]bigger[/]. Try again.");
    }

    private bool NewGame()
    {
        _victoryNumber = new Random().Next(_settings!.DownEdge, _settings.UpEdge + 1);
        AnsiConsole.MarkupLine("[#33ff62]Welcome to new game![/]");
        return false;
    }

    private static string Choice()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Do you want [red]play[/] again?")
                .PageSize(10)
                .AddChoices("YES", "NO"));
        return selected;
    }

    private static void YouWin()
    {
        AnsiConsole.MarkupLine("[#c833ff]You win!!![/]");
    }

    private void NotInRange()
    {
        AnsiConsole.MarkupLine(
            $"Please type number from {_settings!.DownEdge} to {_settings.UpEdge}. [#ffba33]Try again.[/]");
    }

    private static void CanNotParse()
    {
        AnsiConsole.MarkupLine("You need write the number! [#ffba33]Try again.[/]");
    }

    private bool IsGameOver()
    {
        YouWin();
            
        var selected = Choice();

        return selected == "NO" ? GameOver() : NewGame();
    }

    private static bool GameOver()
    {
        AnsiConsole.MarkupLine("[bold red]GAME OVER![/]");
        return true;
    }
}