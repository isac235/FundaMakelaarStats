namespace FundaMakelaarStatsTests
{
    using FundaMakelaarStats.Models.ViewModels;
    using FundaMakelaarStats.Services;
    using System;
    using System.Collections.Generic;

    public class PrinterTests
    {
        private readonly Printer _printer;

        private readonly StringWriter _consoleOutput;

        public PrinterTests()
        {
            _printer = new Printer();
            _consoleOutput = new StringWriter();
            Console.SetOut(_consoleOutput);
        }

        [Fact]
        public void PrintTop10Makelaars_ShouldPrintCorrectHeader()
        {
            // Arrange
            var input = new Dictionary<int, MakelaarsOffers>();

            // Act
            _printer.PrintTop10Makelaars(input);
            var output = _consoleOutput.ToString();

            // Split into lines and get the header parts
            var lines = output.Split(Environment.NewLine);

            // Assert - Check header components separately
            Assert.Equal("Top 10 Makelaars by Number of Offers", lines[0].Trim());
            Assert.Equal(new string('-', 67), lines[1].Trim());

            // More flexible header line check
            var headerLine = lines[2];
            Assert.Contains("#", headerLine);
            Assert.Contains("Makelaar", headerLine);
            Assert.Contains("Offers", headerLine);

            // Verify the final separator line
            Assert.Equal(new string('-', 67), lines[3].Trim());
        }

        [Fact]
        public void PrintTop10Makelaars_ShouldPrintCorrectFormatting()
        {
            // Arrange
            var input = new Dictionary<int, MakelaarsOffers>
    {
        { 1, new MakelaarsOffers(5, "Real Estate King") },
        { 2, new MakelaarsOffers(3, "House Masters") }
    };

            // Act
            _printer.PrintTop10Makelaars(input);
            var output = _consoleOutput.ToString();

            // Assert - More flexible matching
            var lines = output.Split(Environment.NewLine);

            // Find the line with "Real Estate King"
            var realEstateKingLine = lines.FirstOrDefault(l => l.Contains("Real Estate King"));
            Assert.NotNull(realEstateKingLine);

            // Verify the format (rank, name, count)
            Assert.Matches(@"^1\s+Real Estate King\s+5$", realEstateKingLine.Trim());

            // Find the line with "House Masters"
            var houseMastersLine = lines.FirstOrDefault(l => l.Contains("House Masters"));
            Assert.NotNull(houseMastersLine);

            // Verify the format (rank, name, count)
            Assert.Matches(@"^2\s+House Masters\s+3$", houseMastersLine.Trim());
        }

        [Fact]
        public void PrintTop10Makelaars_ShouldOnlyShowTop10()
        {
            // Arrange
            var input = new Dictionary<int, MakelaarsOffers>();
            for (int i = 1; i <= 15; i++)
            {
                input.Add(i, new MakelaarsOffers(15 - i, $"Agent {i}"));
            }

            // Act
            _printer.PrintTop10Makelaars(input);
            var output = _consoleOutput.ToString();

            // Assert
            // Should include Agent 1 through 10
            Assert.Contains("Agent 1", output);
            Assert.Contains("Agent 10", output);

            // Should not include Agent 11 through 15
            Assert.DoesNotContain("Agent 11", output);
            Assert.DoesNotContain("Agent 15", output);
        }

        [Fact]
        public void PrintTop10Makelaars_ShouldOrderByOffersDescending()
        {
            // Arrange
            var input = new Dictionary<int, MakelaarsOffers>
            {
                { 1, new MakelaarsOffers(5, "Agent A") },
                { 2, new MakelaarsOffers(10, "Agent B") },
                { 3, new MakelaarsOffers(3, "Agent C") }
            };

            // Act
            _printer.PrintTop10Makelaars(input);
            var output = _consoleOutput.ToString();

            // Assert
            var lines = output.Split(Environment.NewLine);
            var agentBIndex = Array.FindIndex(lines, l => l.Contains("Agent B"));
            var agentAIndex = Array.FindIndex(lines, l => l.Contains("Agent A"));
            var agentCIndex = Array.FindIndex(lines, l => l.Contains("Agent C"));

            Assert.True(agentBIndex < agentAIndex);  // Agent B should come first
            Assert.True(agentAIndex < agentCIndex);  // Then Agent A, then Agent C
        }

        [Fact]
        public void PrintTop10Makelaars_ShouldHandleEmptyInput()
        {
            // Arrange
            var input = new Dictionary<int, MakelaarsOffers>();

            // Act
            _printer.PrintTop10Makelaars(input);
            var output = _consoleOutput.ToString();

            // Assert
            Assert.Contains("Top 10 Makelaars by Number of Offers", output);
            Assert.DoesNotContain("1     ", output); // No entries should be printed
        }
    }
}
