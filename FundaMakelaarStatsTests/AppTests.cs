namespace FundaMakelaarStatsTests
{
    using FundaMakelaarStats;
    using FundaMakelaarStats.Models.ViewModels;
    using FundaMakelaarStats.Services;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AppTests
    {
        private readonly Mock<IMakelaarService> _serviceMock = new();
        private readonly Mock<IPrinter> _printerMock = new();
        private readonly App _sut;

        public AppTests()
        {
            _sut = new App(_serviceMock.Object, _printerMock.Object);
        }

        [Fact]
        public async Task RunAsync_ShouldCallServicesAndPrinter()
        {
            // Arrange
            var amsterdamData = new Dictionary<int, MakelaarsOffers>
            {
                { 1, new MakelaarsOffers(5, "Test Makelaar") }
            };

            var gardenData = new Dictionary<int, MakelaarsOffers>
            {
                { 2, new MakelaarsOffers(3, "Garden Specialist") }
            };

            _serviceMock.SetupSequence(x => x.GetMakelaarsOffersInfo(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(amsterdamData)
                .ReturnsAsync(gardenData);

            // Act
            await _sut.RunAsync(CancellationToken.None);

            // Assert
            _serviceMock.Verify(x => x.GetMakelaarsOffersInfo(false, It.IsAny<CancellationToken>()), Times.Once);
            _serviceMock.Verify(x => x.GetMakelaarsOffersInfo(true, It.IsAny<CancellationToken>()), Times.Once);

            _printerMock.Verify(x => x.PrintTop10Makelaars(amsterdamData), Times.Once);
            _printerMock.Verify(x => x.PrintTop10Makelaars(gardenData), Times.Once);
        }

        [Fact]
        public async Task RunAsync_ShouldHandleCancellation()
        {
            // Arrange
            var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await _sut.RunAsync(cts.Token);
            _serviceMock.Verify(
                x => x.GetMakelaarsOffersInfo(It.IsAny<bool>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
