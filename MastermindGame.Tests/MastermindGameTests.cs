using MastermindGame.GameLogic;
using MastermindGame.Services;
using Moq;
using Xunit;

namespace MastermindGame.Tests;

public class MastermindGameTests
{
    private readonly Mock<IConsoleService> _consoleMock;
    private readonly GameLogic.MastermindGame _game; 

    public MastermindGameTests()
    {
        _consoleMock = new Mock<IConsoleService>();
        _game = new GameLogic.MastermindGame(_consoleMock.Object, "1234"); 
    }

    [Theory]
    [InlineData("1234", true)]
    [InlineData("1111", true)]
    [InlineData("5555", true)]
    [InlineData("12", false)] // Too short
    [InlineData("12345", false)] // Too long
    [InlineData("abcd", false)] // Non-digits
    [InlineData("7890", false)] // Digits out of range
    [InlineData("0234", false)] // Digit 0 is invalid
    [InlineData("1237", false)] // Digit 7 is invalid
    public void IsValidGuess_ValidatesCorrectly(string guess, bool expected)
    {
        var result = _game.IsValidGuess(guess);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("1234", "++++")] // All correct
    [InlineData("1243", "++--")] // 2 correct position, 2 correct digit wrong position
    [InlineData("4321", "----")] // All correct digits wrong positions
    [InlineData("5555", "+")] // Only one correct digit
    [InlineData("1111", "+")] // Only one correct digit
    [InlineData("5612", "--+")] // Mixed case
    public void EvaluateGuess_ReturnsCorrectHints(string guess, string expectedHint)
    {
        var result = _game.EvaluateGuess(guess);
        Assert.Equal(expectedHint, result);
    }

    [Fact]
    public void Play_EndsWhenCorrectGuess()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleService>();
        consoleMock.SetupSequence(c => c.ReadLine())
            .Returns("1234"); // Correct guess on first attempt

        var game = new GameLogic.MastermindGame(consoleMock.Object, "1234"); 

        // Act
        game.Play();

        // Assert
        consoleMock.Verify(c => c.WriteLine("Congratulations! You've guessed the code correctly!"), Times.Once);
    }

    [Fact]
    public void Play_EndsWhenOutOfAttempts()
    {
        // Arrange
        var consoleMock = new Mock<IConsoleService>();
        // Setup 10 invalid attempts
        for (int i = 0; i < GameConstants.MaxAttempts; i++)
        {
            consoleMock.Setup(c => c.ReadLine()).Returns($"111{i % 10}"); // Invalid guesses
        }

        var game = new GameLogic.MastermindGame(consoleMock.Object, "1234"); 

        // Act
        game.Play();

        // Assert
        consoleMock.Verify(c => c.WriteLine(It.Is<string>(s => s.Contains("Game over!"))), Times.Once);
    }

    [Fact]
    public void GenerateSecretCode_ReturnsValidCode()
    {
        // Act
        var code = GameLogic.MastermindGame.GenerateSecretCode(); 

        // Assert
        Assert.Equal(GameConstants.CodeLength, code.Length);
        foreach (var c in code)
        {
            var digit = c - '0';
            Assert.InRange(digit, GameConstants.MinDigit, GameConstants.MaxDigit);
        }
    }
}