# Robinhood CLI

## Project Status

[![Build status](https://ci.appveyor.com/api/projects/status/hed8j6vdhog1d9al/branch/master?svg=true)](https://ci.appveyor.com/project/adriangodong/robinhood-cli-9hirg/branch/master)

**REAL DATA IS RECEIVED AND SENT TO THE ACTUAL ROBINHOOD API.**

This application is for educational purposes only, provided as-is, and without any warranty. It may not work and it may cause irreversible damage to your account. Use it at your own risk.

## Running the CLI

You need .NET Core 1.1 SDK, Preview 2. If you don't know what this is, stop and use the official Robinhood app instead.

Clone the repository and the submodule using the following command:

    git clone https://github.com/adriangodong/robinhood-cli --recursive

Build and run using the following command:

    cd robinhood-cli
    dotnet restore && dotnet run --project src/RobinhoodCli

## Usage / Commands

### Login

    login username password [--save]
    - --save: optional switch parameter to save login for future runs

### Place an order

    buy|b|sell|s symbol [size] [limit price]
    - size: required for buy orders

Example 1: Place a market buy order for MSFT, 100 shares

    b msft 100

Example 2: Place a buy order for MSFT, 200 shares, limit $50.5 per share

    buy msft 200 50.5

Example 3: Place a market sell order for MSFT, 50 shares

    s msft 50

Example 4: Place a sell order for MSFT, 100 shares, limit $35 per share

    sell msft 100 35

Example 5: Exit all MSFT position

    sell msft
    s msft

Both commands above will create the same order.
They will create a market sell order for MSFT, order size is equal to open shares count.

### Cancel an open order

    cancel order-index

Cancel an open order at order-index position. You can retrieve open orders using the `orders` command.

Example: Cancel the first order in the list

    orders

    Open orders:
    [1] Sell HOOD 1.00 1.00
    [2] Buy HOOD 3.00 2.00

    cancel 1

### Other commands

    positions

Update and show current open positions.

    orders

Update and show current open orders.

    logout

Clears authentication token from stored configuration and current session.

    exit

Exits the application.

## Developer Notes

* [Unofficial API Documentation](https://github.com/sanko/Robinhood)
* [Unofficial .NET Client SDK](https://github.com/thiagoamarante/Deadlock.Robinhood)