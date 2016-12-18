# Robinhood CLI

## Usage

### Ask for help

    help

### Login

    login username password
    - username
    - password

### Positions

    pos

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