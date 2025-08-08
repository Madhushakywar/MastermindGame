using MastermindGame.Services;

namespace MastermindGame.GameLogic;

/// <summary>
/// Represents the core logic of the Mastermind game.
/// </summary>
public class MastermindGame
{
    private readonly IConsoleService _consoleService;
    private readonly string _secretCode;
    private int _remainingAttempts;

    /// <summary>
    /// Initializes a new instance of the MastermindGame class.
    /// </summary>
    /// <param name="consoleService">The console service for input/output.</param>
    /// <param name="secretCode">The secret code to be guessed.</param>
    public MastermindGame(IConsoleService consoleService, string secretCode)
    {
        _consoleService = consoleService ?? throw new ArgumentNullException(nameof(consoleService));
        _secretCode = secretCode ?? throw new ArgumentNullException(nameof(secretCode));
        _remainingAttempts = GameConstants.MaxAttempts;
    }

    /// <summary>
    /// Starts the game loop.
    /// </summary>
    public void Play()
    {
        _consoleService.WriteLine($"Welcome to Mastermind! Guess the {GameConstants.CodeLength}-digit code (digits {GameConstants.MinDigit}-{GameConstants.MaxDigit}).");
        _consoleService.WriteLine($"You have {_remainingAttempts} attempts.");

        while (_remainingAttempts > 0)
        {
            _consoleService.Write($"Attempt {GameConstants.MaxAttempts - _remainingAttempts + 1}: ");
            var guess = _consoleService.ReadLine();

            if (!IsValidGuess(guess))
            {
                _consoleService.WriteLine($"Invalid input. Please enter {GameConstants.CodeLength} digits between {GameConstants.MinDigit} and {GameConstants.MaxDigit}.");
                continue;
            }

            var result = EvaluateGuess(guess!);
            _consoleService.WriteLine($"Result: {result}");

            if (result == new string(GameConstants.CorrectDigitCorrectPosition, GameConstants.CodeLength))
            {
                _consoleService.WriteLine("Congratulations! You've guessed the code correctly!");
                return;
            }

            _remainingAttempts--;
        }

        _consoleService.WriteLine($"Game over! You've run out of attempts. The secret code was: {_secretCode}");
    }

    /// <summary>
    /// Validates the player's guess.
    /// </summary>
    /// <param name="guess">The guess to validate.</param>
    /// <returns>True if the guess is valid, otherwise false.</returns>
    public bool IsValidGuess(string? guess)
    {
        if (guess?.Length != GameConstants.CodeLength)
            return false;

        foreach (var c in guess)
        {
            if (!char.IsDigit(c))
                return false;

            var digit = c - '0';
            if (digit < GameConstants.MinDigit || digit > GameConstants.MaxDigit)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Evaluates the player's guess against the secret code.
    /// </summary>
    /// <param name="guess">The player's guess.</param>
    /// <returns>A string consisting of '+' and '-' indicating correct digits.</returns>
    public string EvaluateGuess(string guess)
    {
        var correctPositions = 0;
        var correctDigits = 0;
        var secretCodeDigits = new int[GameConstants.MaxDigit + 1];
        var guessDigits = new int[GameConstants.MaxDigit + 1];

        // Count correct positions and track digit frequencies
        for (var i = 0; i < GameConstants.CodeLength; i++)
        {
            if (guess[i] == _secretCode[i])
            {
                correctPositions++;
            }
            else
            {
                var secretDigit = _secretCode[i] - '0';
                var guessDigit = guess[i] - '0';
                secretCodeDigits[secretDigit]++;
                guessDigits[guessDigit]++;
            }
        }

        // Count correct digits in wrong positions
        for (var i = GameConstants.MinDigit; i <= GameConstants.MaxDigit; i++)
        {
            correctDigits += Math.Min(secretCodeDigits[i], guessDigits[i]);
        }

        return new string(GameConstants.CorrectDigitCorrectPosition, correctPositions) +
               new string(GameConstants.CorrectDigitWrongPosition, correctDigits);
    }

    /// <summary>
    /// Generates a random secret code for the game.
    /// </summary>
    /// <returns>A randomly generated secret code.</returns>
    public static string GenerateSecretCode()
    {
        var random = new Random();
        var code = new char[GameConstants.CodeLength];

        for (var i = 0; i < GameConstants.CodeLength; i++)
        {
            code[i] = (char)(random.Next(GameConstants.MinDigit, GameConstants.MaxDigit + 1) + '0');
        }

        return new string(code);
    }
}