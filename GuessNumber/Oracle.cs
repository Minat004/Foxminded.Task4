using GuessNumber.Settings;
using GuessNumber.Wrapper;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace GuessNumber;

public class Oracle
{
    private readonly AppSettingsModel? _settings;
    private readonly IConsoleIO _console;
    private readonly IRandomizer _random;
    private int _victoryNumber;

    public Oracle(IConfiguration configuration, IConsoleIO console, IRandomizer random)
    {
        _console = console;
        _random = random;
        _settings = configuration.GetRequiredSection("AppSettings").Get<AppSettingsModel>();
        _victoryNumber = _random.RandomInRange(_settings!.DownEdge, _settings.UpEdge + 1);
    }

    public void StartHeader()
    {
        _console.Write(
            new FigletText("Guess Number")
                .LeftJustified()
                .Color(Color.Red));

        _console.WriteLine(
            $"[red]Can you guest the number from {_settings!.DownEdge} to {_settings.UpEdge}?[/]");
    }
    
    public bool Guessed()
    {
        _console.Write("Please type your number: ");
        
        if (!int.TryParse(_console.ReadLine(), out var number))
        {
            CanNotParse();
            return false;
        }

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

    private void IsSmaller()
    {
        _console.WriteLine("Your number is [bold green]smaller[/]. Try again.");
    }

    private void IsBigger()
    {
        _console.WriteLine("Your number is [bold blue]bigger[/]. Try again.");
    }

    private void NotInRange()
    {
        _console.WriteLine(
            $"Please type number from {_settings!.DownEdge} to {_settings.UpEdge}. [#ffba33]Try again.[/]");
    }

    private void CanNotParse()
    {
        _console.WriteLine("You need write the number! [#ffba33]Try again.[/]");
    }

    private bool IsGameOver()
    {
        YouWin();
            
        var selected = Choice();

        return selected == "NO" ? GameOver() : NewGame();
    }

    private void YouWin()
    {
        _console.WriteLine("[#c833ff]You win!!![/]");
    }

    private string Choice()
    {
        var selected = _console.Prompt(
            new SelectionPrompt<string>()
                .Title("Do you want [red]play[/] again?")
                .PageSize(10)
                .AddChoices("YES", "NO"));
        return selected;
    }

    private bool GameOver()
    {
        _console.WriteLine("[bold red]GAME OVER![/]");
        return true;
    }

    private bool NewGame()
    {
        _victoryNumber = _random.RandomInRange(_settings!.DownEdge, _settings.UpEdge + 1);
        _console.WriteLine("[#33ff62]Welcome to new game![/]");
        return false;
    }
}