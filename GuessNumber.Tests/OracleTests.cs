using Microsoft.Extensions.Configuration;

namespace GuessNumber.Tests;

public class OracleTests
{
    private const int _from = 0;
    private const int _to = 100;

    [Theory]
    [InlineData("abcdefg")]
    [InlineData("!@#$%^&*")]
    [InlineData("")]
    [InlineData("123#")]
    public void GuessedCanNotParseTest(string input)
    {
        var mock = new Mock<IConsoleIO>();

        mock.Setup(x => x.ReadLine()).Returns(input);

        var oracle = new Oracle(GetConfig(), mock.Object, new Randomizer());

        oracle.Guessed();
        
        mock.Verify(x => x.WriteLine("You need write the number! [#ffba33]Try again.[/]"), Times.Once);
    }
    
    [Theory]
    [InlineData("-1")]
    [InlineData("101")]
    [InlineData("-789")]
    [InlineData("1500")]
    public void GuessedNotInRangeTest(string input)
    {
        var mock = new Mock<IConsoleIO>();

        mock.Setup(x => x.ReadLine()).Returns(input);

        var oracle = new Oracle(GetConfig(), mock.Object, new Randomizer());

        oracle.Guessed();
        
        mock.Verify(x => 
            x.WriteLine($"Please type number from {_from} to {_to}. [#ffba33]Try again.[/]"), 
            Times.Once);
    }

    [Theory]
    [InlineData("0", 0, "YES")]
    [InlineData("0", 0, "NO")]
    [InlineData("1", 1, "YES")]
    [InlineData("1", 1, "NO")]
    [InlineData("100", 100, "YES")]
    [InlineData("100", 100, "NO")]
    [InlineData("50", 50, "YES")]
    [InlineData("50", 50, "NO")]
    [InlineData("13", 13, "YES")]
    [InlineData("13", 13, "NO")]
    public void GuessedIsGameOverTest(string input, int victory, string choice)
    {
        var mockConsole = new Mock<IConsoleIO>();
        var mockRandom = new Mock<IRandomizer>();

        mockConsole.Setup(x => x.ReadLine()).Returns(input);
        mockConsole.Setup(x => x.Prompt()).Returns(choice);
        mockRandom.Setup(x => x.RandomInRange(_from, _to + 1)).Returns(victory);

        var oracle = new Oracle(GetConfig(), mockConsole.Object, mockRandom.Object);
        
        oracle.Guessed();
        
        mockConsole.Verify(x => x.WriteLine("[#c833ff]You win!!![/]"), Times.Once);
        
        if (choice.Equals("NO"))
        {
            mockConsole.Verify(x => x.WriteLine("[bold red]GAME OVER![/]"), Times.Once);
            return;
        }
        
        mockConsole.Verify(x => x.WriteLine("[#33ff62]Welcome to new game![/]"), Times.Once);
    }
    
    [Theory]
    [InlineData("1", 0)]
    [InlineData("23", 1)]
    [InlineData("100", 99)]
    [InlineData("99", 50)]
    [InlineData("14", 13)]
    public void GuessedIsBiggerTest(string input, int victory)
    {
        var mockConsole = new Mock<IConsoleIO>();
        var mockRandom = new Mock<IRandomizer>();

        mockConsole.Setup(x => x.ReadLine()).Returns(input);
        mockRandom.Setup(x => x.RandomInRange(_from, _to + 1)).Returns(victory);

        var oracle = new Oracle(GetConfig(), mockConsole.Object, mockRandom.Object);
        
        oracle.Guessed();
        
        mockConsole.Verify(x => 
            x.WriteLine("Your number is [bold blue]bigger[/]. Try again."), Times.Once);
    }
    
    [Theory]
    [InlineData("0", 1)]
    [InlineData("22", 23)]
    [InlineData("0", 100)]
    [InlineData("98", 99)]
    [InlineData("13", 14)]
    public void GuessedIsSmallerTest(string input, int victory)
    {
        var mockConsole = new Mock<IConsoleIO>();
        var mockRandom = new Mock<IRandomizer>();

        mockConsole.Setup(x => x.ReadLine()).Returns(input);
        mockRandom.Setup(x => x.RandomInRange(_from, _to + 1)).Returns(victory);

        var oracle = new Oracle(GetConfig(), mockConsole.Object, mockRandom.Object);
        
        oracle.Guessed();
        
        mockConsole.Verify(x => 
            x.WriteLine("Your number is [bold green]smaller[/]. Try again."), Times.Once);
    }

    private static IConfiguration GetConfig()
    {
        var inMemory = new Dictionary<string, string>
        {
            { "AppSettings:DownEdge", $"{_from}" },
            { "AppSettings:UpEdge", $"{_to}" }
        };
        
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemory!)
            .Build();

        return configuration;
    }
}