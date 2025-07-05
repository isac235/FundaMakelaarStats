namespace FundaMakelaarStatsTests
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Configs;
    using FundaMakelaarStats.Models.ApiResponse;
    using FundaMakelaarStats.Services;
    using Microsoft.Extensions.Options;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class MakelaarServiceTests
    {
        private readonly Mock<IFundaApiClient> _apiClientMock = new();

        private readonly FundaApiConfigurations _config = new()
        {
            BaseUrl = "https://partnerapi.funda.nl",
            FeedEndpoint = "feeds/Aanbod.svc/json",
            ApiKey = "test-key",
            Type = "koop",
            SearchCommand = "amsterdam",
            PageSize = 25,
            DelayBetweenRequestsMs = 100
        };

        private readonly MakelaarService _sut;

        public MakelaarServiceTests()
        {
            var optionsMock = new Mock<IOptions<FundaApiConfigurations>>();
            optionsMock.Setup(x => x.Value).Returns(_config);

            _sut = new MakelaarService(_apiClientMock.Object, optionsMock.Object);
        }

        [Fact]
        public async Task GetMakelaarsOffersInfo_ShouldAggregateOffersCorrectly()
        {
            // Arrange
            var mockResponse1 = new FundaApiResponse(
                AccountStatus: 0,
                EmailNotConfirmed: false,
                ValidationFailed: false,
                Metadata: new Metadata("x", "y", "z"),
                Objects: new List<RealEstateObject>
                {
                    new() { MakelaarId = 1, MakelaarNaam = "Agent A" },
                    new() { MakelaarId = 2, MakelaarNaam = "Agent B" },
                    new() { MakelaarId = 1, MakelaarNaam = "Agent A" } // Duplicate agent
                },
                Paging: new Paging(1, 1, "x", "y"),
                TotaalAantalObjecten: 3
            );

            var mockResponse2 = new FundaApiResponse(
                AccountStatus: 0,
                EmailNotConfirmed: false,
                ValidationFailed: false,
                Metadata: new Metadata("x", "y", "z"),
                Objects: new List<RealEstateObject>(), // Empty second page
                Paging: new Paging(1, 1, "x", "y"),
                TotaalAantalObjecten: 3
            );

            _apiClientMock.SetupSequence(x => x.GetOffers(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockResponse1)
                .ReturnsAsync(mockResponse2);

            // Act
            var result = await _sut.GetMakelaarsOffersInfo();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(2, result[1].NumberOfOffers); // Agent A should have 2 offers
            Assert.Equal(1, result[2].NumberOfOffers); // Agent B should have 1 offer
        }

        [Fact]
        public async Task GetMakelaarsOffersInfo_ShouldApplyTuinFilter()
        {
            // Arrange
            _apiClientMock.Setup(x => x.GetOffers(It.Is<string>(url => url.Contains("/tuin")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new FundaApiResponse(
                    AccountStatus: 0,
                    EmailNotConfirmed: false,
                    ValidationFailed: false,
                    Metadata: new Metadata("x", "y", "z"),
                    Objects: new List<RealEstateObject>(),
                    Paging: new Paging(1, 1, "x", "y"),
                    TotaalAantalObjecten: 0
                ));

            // Act
            await _sut.GetMakelaarsOffersInfo(hasTuinFilter: true);

            // Assert
            _apiClientMock.Verify(x => x.GetOffers(It.Is<string>(url => url.Contains("/tuin")), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task GetMakelaarsOffersInfo_ShouldHandlePagination()
        {
            // Arrange
            var fullPageResponse = new FundaApiResponse(
                AccountStatus: 0,
                EmailNotConfirmed: false,
                ValidationFailed: false,
                Metadata: new Metadata("x","y","z"),
                Objects: Enumerable.Repeat(new RealEstateObject { MakelaarId = 1, MakelaarNaam = "Agent" }, _config.PageSize).ToList(),
                Paging: new Paging(1, 1, "x", "y"),
                TotaalAantalObjecten: _config.PageSize * 2
            );

            var emptyResponse = new FundaApiResponse(
                AccountStatus: 0,
                EmailNotConfirmed: false,
                ValidationFailed: false,
                Metadata: new Metadata("x", "y", "z"),
                Objects: new List<RealEstateObject>(),
                Paging: new Paging(1,1,"x","y"),
                TotaalAantalObjecten: _config.PageSize * 2
            );

            _apiClientMock.SetupSequence(x => x.GetOffers(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(fullPageResponse)
                .ReturnsAsync(fullPageResponse)
                .ReturnsAsync(emptyResponse);

            // Act
            var result = await _sut.GetMakelaarsOffersInfo();

            // Assert
            _apiClientMock.Verify(x => x.GetOffers(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
            Assert.Equal(_config.PageSize * 2, result[1].NumberOfOffers);
        }
    }
}
