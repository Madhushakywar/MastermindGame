namespace MastermindGame.GameLogic;

/// <summary>
/// Contains constants used throughout the Mastermind game.
/// </summary>
public static class GameConstants
{
    public const int CodeLength = 4;
    public const int MinDigit = 1;
    public const int MaxDigit = 6;
    public const int MaxAttempts = 10;
    public const char CorrectDigitCorrectPosition = '+';
    public const char CorrectDigitWrongPosition = '-';
}