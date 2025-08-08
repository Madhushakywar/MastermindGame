using MastermindGame.Services;

// Initialize dependencies
var consoleService = new ConsoleService();
var secretCode = MastermindGame.GameLogic.MastermindGame.GenerateSecretCode();

// Create and start the game
var game = new MastermindGame.GameLogic.MastermindGame(consoleService, secretCode);
game.Play();