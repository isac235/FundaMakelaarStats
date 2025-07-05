namespace FundaMakelaarStatsTests
{
    using FundaMakelaarStats.APIClients;
    using FundaMakelaarStats.Models.ApiResponse;
    using FundaMakelaarStats.Utils;
    using Moq;
    using RichardSzalay.MockHttp;
    using System.Net;
    using System.Text;
    using Xunit;

    public class FundaApiClientTests
    {
        private readonly Mock<IFundaJsonSerializer> _serializerMock = new();
        private readonly MockHttpMessageHandler _httpHandler = new();
        private readonly HttpClient _httpClient;
        private readonly FundaApiClient _sut;

        public FundaApiClientTests()
        {
            _httpClient = new HttpClient(_httpHandler);
            _sut = new FundaApiClient(_httpClient, _serializerMock.Object);
        }

        [Fact]
        public async Task GetOffers_ValidRequest_ReturnsDeserializedResponse()
        {
            // Arrange
            var expectedResponse = new FundaApiResponse(
            AccountStatus: 0,
            EmailNotConfirmed: false,
            ValidationFailed: false,
            Metadata: null,
            Objects: new List<RealEstateObject>(),
            Paging: null,
            TotaalAantalObjecten: 0
        );
            var json = "{ \"Objects\": [] }";

            _httpHandler.When("http://test.com")
                       .Respond(HttpStatusCode.OK,
                               new StringContent(json, Encoding.UTF8, "application/json"));

            _serializerMock.Setup(x => x.DeserializeAsync<FundaApiResponse>(
                               It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.GetOffers("http://test.com");

            // Assert
            Assert.Same(expectedResponse, result);
            _serializerMock.VerifyAll();
        }

        [Fact]
        public async Task GetOffers_HttpError_ThrowsException()
        {
            // Arrange
            var testUrl = "http://test.com";
            _httpHandler.When(testUrl)
                    .Respond(HttpStatusCode.InternalServerError);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _sut.GetOffers(testUrl));
        }

        [Fact]
        public async Task GetOffers_CancelledToken_ThrowsTaskCanceledException()
        {
            // Arrange
            var testUrl = "http://test.com";
            var cts = new CancellationTokenSource();
            cts.Cancel(); // Immediately cancel

            // Act & Assert
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                _sut.GetOffers(testUrl, cts.Token));
        }
    }
}