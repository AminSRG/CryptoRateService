# CryptoRateService

## Overview

`CryptoRateService` provides an API to retrieve the latest exchange rates for a given cryptocurrency symbol (e.g., `BTC`, `ETH`). The service fetches data from CoinMarketCap and supports multiple currencies, including USD, EUR, BRL, GBP, and AUD.

## Features

- Fetches the latest exchange rates for a cryptocurrency.
- Supports multiple currencies.
- Implements Basic Authentication for secure access.

## Authentication

This API uses **Basic Authentication**. To access the endpoints, use the following credentials:

- **Username**: `admin`
- **Password**: `password123`

The credentials are encoded in Base64 format and passed in the `Authorization` header.

## API Endpoints

### `POST /api/cryptorates`

#### Description:
Retrieves the exchange rates for the specified cryptocurrency symbol.

#### Request Body:
- **symbol**: The cryptocurrency symbol (e.g., `BTC`, `ETH`).

#### Response:

- **200 OK**: Successfully retrieved exchange rates.
- **400 Bad Request**: If the symbol is null or empty.
- **401 Unauthorized**: If authentication fails.
- **500 Internal Server Error**: If an unexpected error occurs.

#### Response Details:
The response will include a list of exchange rates for the given cryptocurrency symbol in multiple currencies (USD, EUR, BRL, GBP, and AUD).

## Installation

1. Clone the repository:

```
git clone https://github.com/yourusername/CryptoRateService.git
```

2. Navigate to the project directory:
```
cd CryptoRateService
```

3. Install dependencies:
```
dotnet restore
```

4. Run the application: 
```
dotnet run
```

## Testing
To run unit tests, use the following command:
```
dotnet test
```
