using GuessNumber.Settings;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace GuessNumber;

public class App
{
    private readonly IConfiguration _configuration;

    public App(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task RunAsync()
    {
        var oracle = new Oracle(_configuration);
        
        oracle.Header();

        while (!oracle.Guessed()){}

        return Task.CompletedTask;
    }
}