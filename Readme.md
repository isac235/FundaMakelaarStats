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
   cd funda-makelaar-stats```

2. Configure the aplication settings:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "System.Net.Http.HttpClient": "Information" // change this if you feel the console is too noisy.
    }
  },
  "FundaApiConfigurations": {
    "BaseUrl": "http://partnerapi.funda.nl", // Base URL for Funda API
    "FeedEndpoint": "feeds/Aanbod.svc/json", // Endpoint for property listings
    "ApiKey": "76666a29898f491480386d966b75f949", // Temporary Funda API key
    "Type": "koop", // Type of property listings (koop = sale)
    "SearchCommand": "amsterdam", // Search term for properties
    "PageSize": 25, // Number of listings per page (Funda API resturn a maximum of 25 per request)
    "DelayBetweenRequestsMs": 400  // Delay between requests to avoid hitting rate limits
  }
}
```
 

 3. Build and run the application: 
 
 
 Make sure you are in the project directory (It should take about 2 and a half minutes to run and get the output)
   ```bash
   dotnet run 
   
   Example output:
   
Top 10 Makelaars in Amsterdam
-------------------------------------------------------------------
#     Makelaar                                        Offers
-------------------------------------------------------------------
1     Hoekstra en van Eck Amsterdam Noord              142
2     MVA Makelaardij                                  127
3     Amsterdam Housing                                119
...
