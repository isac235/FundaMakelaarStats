# Funda Makelaar Stats

A .NET application that analyzes and displays the top 10 real estate agents in Amsterdam based on property listings from the Funda API.

## Features

- **Top 10 Agent Rankings**:
  - Regular listings
  - Listings with gardens (`tuin`)
- **Resilient API Client**:
  - Automatic retries with exponential backoff
  - Rate limit handling
- **Clean Architecture**:
  - Separation of concerns
  - Dependency Injection
  - Testable design

## Technologies

- .NET 8
- HttpClient with Polly resilience policies
- xUnit for testing
- Moq for test mocking

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Funda API key 76666a29898f491480386d966b75f949 (temporary key provided)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/isac235/FundaMakelaarStats.git
   cd funda-makelaar-stats

2. Configure the aplication settings:
   ```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "System.Net.Http.HttpClient": "Information" 
    }
  },
  "FundaApiConfigurations": {
    "BaseUrl": "http://partnerapi.funda.nl", 
    "FeedEndpoint": "feeds/Aanbod.svc/json", 
    "ApiKey": "76666a29898f491480386d966b75f949",
    "Type": "koop", 
    "SearchCommand": "amsterdam", 
    "PageSize": 25, 
    "DelayBetweenRequestsMs": 400  
  }
}
```	 

 3. Build and run the application:   // Make sure you are in the project directory (It should take about 2 and a half minutes to run)
   ```bash
   dotnet run --project FundaMakelaarStats   
   
   Example output:
   ```
   Top 10 Makelaars in Amsterdam
-------------------------------------------------------------------
#     Makelaar                                        Offers
-------------------------------------------------------------------
1     Hoekstra en van Eck Amsterdam Noord              142
2     MVA Makelaardij                                  127
3     Amsterdam Housing                                119
...
