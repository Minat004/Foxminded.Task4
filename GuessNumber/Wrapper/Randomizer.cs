namespace GuessNumber.Wrapper;

public class Randomizer : IRandomizer
{
    public int RandomInRange(int from, int to)
    {
        return new Random().Next(from, to);
    }
}