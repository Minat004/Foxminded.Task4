using Spectre.Console;
using Spectre.Console.Rendering;

namespace GuessNumber.Wrapper;

public interface IConsoleIO
{
    public string? ReadLine();
    public void WriteLine(FormattableString value);
    
    public void WriteLine(string value);
    
    public void Write(IRenderable value);
    
    public void Write(FormattableString value);
    
    public void Write(string value);
    
    public string Prompt(IPrompt<string> value);
}