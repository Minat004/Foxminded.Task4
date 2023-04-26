using GuessNumber.Wrapper;
using Microsoft.Extensions.Configuration;

namespace GuessNumber;

public class App
{
    private readonly IConfiguration _configuration;
    private readonly IConsoleIO _console;
    private readonly IRandomizer _random;

    public App(IConfiguration configuration, IConsoleIO console, IRandomizer random)
    {
        _configuration = configuration;
        _console = console;
        _random = random;
    }

    public Task RunAsync()
    {
        var oracle = new Oracle(_configuration, _console, _random);
        
        oracle.StartHeader();

        while (!oracle.Guessed()){}

        return Task.CompletedTask;
    }
}