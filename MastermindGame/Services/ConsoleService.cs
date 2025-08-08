namespace MastermindGame.Services;

/// <summary>
/// Handles all console input and output operations.
/// </summary>
public class ConsoleService : IConsoleService
{
    /// <summary>
    /// Reads a guess from the console input.
    /// </summary>
    /// <returns>The user's guess as a string.</returns>
    public string? ReadLine()
    {
        return Console.ReadLine();
    }

    /// <summary>
    /// Writes a message to the console output.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    /// <summary>
    /// Writes a message to the console output without a newline.
    /// </summary>
    /// <param name="message">The message to write.</param>
    public void Write(string message)
    {
        Console.Write(message);
    }
}

/// <summary>
/// Interface for console operations to enable testing.
/// </summary>
public interface IConsoleService
{
    string? ReadLine();
    void WriteLine(string message);
    void Write(string message);
}