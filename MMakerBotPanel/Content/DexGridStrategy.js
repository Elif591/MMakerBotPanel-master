$(document).ready(function () {
    $(".placeholder").select2({
        placeholder: "Make a Selection",
        allowClear: false
    });
    DexConnectWallet();
});
function GetTokenSymbol(wrapped , provider) {
    const ERC20_ABI = [
        {
            constant: true,
            inputs: [],
            name: 'name',
            outputs: [{ name: '', type: 'string' }],
            payable: false,
            stateMutability: 'view',
            type: 'function'
        },
        {
            constant: true,
            inputs: [],
            name: 'symbol',
            outputs: [{ name: '', type: 'string' }],
            payable: false,
            stateMutability: 'view',
            type: 'function'
        }
    ];

    const getTokenSymbol = async (tokenAddress) => {
        const providerSymbol = ethers.getDefaultProvider(provider);
        const contract = new ethers.Contract(tokenAddress, ERC20_ABI, providerSymbol);
        const symbol = await contract.symbol();
        return symbol;
    };

    const tokenAddress = $("#tokenAddress").val();
    getTokenSymbol(tokenAddress)
        .then(symbol => {
            console.log(symbol);
            var html = "" + wrapped + " --> " + symbol + "";
            document.getElementById("dexPair").value = html;
        })
        .catch(error => {
            console.log(error)
            Snackbar.show({
                text: 'Please enter the token address',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
            const dexButton = document.querySelector('.dexConnectWallet');
            dexButton.innerText = "Connect Metamask";
        });
}
function CalculateTakeProfitAndStopLoss(MaxGridPriceRange, MinGridPriceRange) {
    let takeProfit = 0;
    let stopLoss = 0;

    takeProfit = MaxGridPriceRange * 1.05;
    stopLoss = MinGridPriceRange * 0.95;

    let takeProfitRoundedNumber = Math.round(takeProfit * 100) / 100;
    let stopLossRoundedNumber = Math.round(stopLoss * 100) / 100;

    return { takeProfit: takeProfitRoundedNumber, stopLoss: stopLossRoundedNumber };
}
function CalculateProfitPerGrid(gridCount, minPrice, maxPrice) {
    const commissionRate = 0.1;
    const priceDifference = (maxPrice - minPrice) / gridCount;

    let maxProfitPerGrid = (1 - commissionRate) * priceDifference / minPrice - 2 * commissionRate;
    let minProfitPerGrid = (maxPrice * (1 - commissionRate)) / (maxPrice - priceDifference) - 1 - commissionRate;

    maxProfitPerGrid = Math.round(Math.abs(maxProfitPerGrid * 100), 6);
    minProfitPerGrid = Math.round(Math.abs(minProfitPerGrid * 100), 6);
    return { minProfitPerGrid, maxProfitPerGrid };
}
function GetMinimumWBNBAmount(gridCount) {
    const comissionFee = gridCount * (0.0007 * 0.01);
    const minimumUsdt = (gridCount * 0.0007) + comissionFee;
    return minimumUsdt;
}
function DexCalculate() {
    $("#dexCalculate").click(function () {
        var maxPrice = $("#dexMaxPriceRange").val();
        var minPrice = $("#dexMinPriceRange").val();
        var gridCount = $("#dexGridCount").val();
        var invalidGridCount = $("#invalid-GridCount");
        invalidGridCount.css('display', 'none');
        if (minPrice != null && maxPrice != null && gridCount != null) {

            if (parseFloat(gridCount) > 2 && parseFloat(gridCount) < 170) {
                invalidGridCount.css('display', 'none');
                if (parseFloat(minPrice) < parseFloat(maxPrice)) {

                    var takeProfitAndStoploss = CalculateTakeProfitAndStopLoss(parseFloat(maxPrice), parseFloat(minPrice));
                    $("#dexTakeProfit").val(takeProfitAndStoploss.takeProfit);
                    $("#dexStopLoss").val(takeProfitAndStoploss.stopLoss);

                    var profitPerGrid = CalculateProfitPerGrid(gridCount, parseFloat(minPrice), parseFloat(maxPrice));
                    $("#dexProfitGrid").val(profitPerGrid.minProfitPerGrid + " % ~ " + profitPerGrid.maxProfitPerGrid + " %");


                    var minUsdtAmount = document.getElementById('dexMinWBNBAmount');
                    var text = ">=" + GetMinimumWBNBAmount(gridCount);
                    minUsdtAmount.placeholder = text;

                } else {

                    Snackbar.show({
                        text: 'The upper limit must be greater than the lower limit',
                        actionTextColor: '#fff',
                        backgroundColor: '#e7515a',
                        pos: 'top-left'
                    });
                }
            }
            else {
                invalidGridCount.css('display', 'block');
            }

        }
        else {

            Snackbar.show({
                text: 'Please fill in the blanks',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });

        }
    });

}
function DexConnectWallet() {
    $(".dexConnectWallet").click(function () {
        const dexButton = document.querySelector('.dexConnectWallet');
        let accounts = [];

        dexButton.addEventListener('click', () => {
            getAccount();
            getChainNetwork();

        });
        window.ethereum.on('networkChanged', function () {
            getChainNetwork();

        });
        async function getAccount() {
            accounts = await ethereum.request({ method: 'eth_requestAccounts' });
            dexButton.innerText = accounts;
        }
        async function getChainNetwork() {
            chainId = await ethereum.request({ method: 'eth_chainId' }).then(chainSymbol => {
                if ($("#tokenAddress").val() != "") {
                    var dexSelected = $('#Detail_DexExchange').val();
                    Options(dexSelected, chainSymbol, accounts[0]);
                } else {
                    Snackbar.show({
                        text: 'Please enter the token address',
                        actionTextColor: '#fff',
                        backgroundColor: '#e7515a',
                        pos: 'top-left'
                    });
                    const dexButton = document.querySelector('.dexConnectWallet');
                    dexButton.innerText = "Connect Metamask";
                }
            });
        }
    });
}
function Options(dexSelected, chainSymbol, account) {
    const dexButton = document.querySelector('.dexConnectWallet');
    if (dexSelected == "Pancakeswap") {
        if (chainSymbol == 0x38) {
            $('#Detail_DexExchange').on('change', function () {
                location.reload();
            });
            const pancakeswapAddress = {
                WBNB: '0xbb4cdb9cbd36b01bd1cbaebf2de08d9173bc095c',
                factory: '0xcA143Ce32Fe78f1f7019d7d551a6402fC5350c73',
                router: '0x10ED43C718714eb63d5aA57B78B54704E256024E',
            }
            const provider = "https://bsc-dataseed.binance.org/";
            GetTokenSymbol("WBNB", provider );
            DexCalculate();
            GetBalance(account, pancakeswapAddress.WBNB, "WBNB");
            $("#dexStart").click(function () {
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmount");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmount").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');
                   
                    PancakeSwapBuyTokens(account, pancakeswapAddress.router, pancakeswapAddress.WBNB, provider, pancakeswapAddress.factory);
                }
            });
            $("#dexStop").click(function () {
                clearInterval(startTradeInterval);
                StopTrade(pancakeswapAddress.WBNB);
            });
        }
        else {
            Snackbar.show({
                text: 'Please connect BNB Smart Chain',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
            dexButton.innerText = "Connect Metamask";
            accounts = [];
        }
    }
    if (dexSelected == "UniswapV2") {
        if (chainSymbol == 0x1) {
            $('#Detail_DexExchange').on('change', function () {
                location.reload();
            });
            const uniswapAddress = {
                ETH: '0xc02aaa39b223fe8d0a0e5c4f27ead9083c756cc2',
                factory: '0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f',
                router: '0x7a250d5630B4cF539739dF2C5dAcb4c659F2488D',
            }
            const provider = "https://mainnet.infura.io/v3/4db639d1a0dd4471ba7995ed8b4cc876";
            GetTokenSymbol("WETH", provider);
            DexCalculate();
            GetBalance(account, uniswapAddress.ETH, "WETH");
            $("#dexStart").click(function () {
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmount");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmount").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');
                   
                    PancakeSwapBuyTokens(account, uniswapAddress.router, uniswapAddress.ETH, provider, uniswapAddress.factory);
                }
            });
            $("#dexStop").click(function () {
                clearInterval(startTradeInterval);
                StopTrade(account, uniswapAddress.ETH);
            });

        } else {
            Snackbar.show({
                text: 'Please connect Ethereum Chain',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
            dexButton.innerText = "Connect Metamask";
            accounts = [];
        }
    }

    if (dexSelected == "UniswapV3") {
        if (chainSymbol == 0x1) {
            $('#Detail_DexExchange').on('change', function () {
                location.reload();
            });
            const uniswapAddress = {
                ETH: '0xC02aaA39b223FE8D0A0e5C4F27eAD9083C756Cc2',
                factory: '0x1F98431c8aD98523631AE4a59f267346ea31F984',
                router: '0xE592427A0AEce92De3Edee1F18E0157C05861564',
            }
            const provider = "https://mainnet.infura.io/v3/4db639d1a0dd4471ba7995ed8b4cc876";
            GetTokenSymbol("WETH", provider);
            DexCalculate();
            GetBalance(account, uniswapAddress.ETH, "WETH");
            $("#dexStart").click(function () {
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmount");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmount").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');
                  
                    UniswapBuyTokens(account, uniswapAddress.router, uniswapAddress.ETH, provider, uniswapAddress.factory);
                }
            });
            $("#dexStop").click(function () {
                clearInterval(startTradeInterval);
                StopTrade(account, uniswapAddress.ETH);
            });

        } else {
            Snackbar.show({
                text: 'Please connect Ethereum Chain',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
            dexButton.innerText = "Connect Metamask";
            accounts = [];
        }
    }
    if (dexSelected == "TraderJoe") {
        if (chainSymbol == 0xa86a) {
            $('#Detail_DexExchange').on('change', function () {
                location.reload();
            });
            const traderJoeAddress = {
                AVAX: 'FvwEAhmxKfeiG8SnEvq42hc6whRyY3EFYAvebMqDNDGCgxN5Z',
                factory: '0x9Ad6C38BE94206cA50bb0d90783181662f0Cfa10',
                router: '0x60aE616a2155Ee3d9A68541Ba4544862310933d4',
            }
            const provider = "https://api.avax.network/ext/bc/C/rpc";
            GetTokenSymbol("JOE", provider);
            DexCalculate();
            GetBalance(account, traderJoeAddress.AVAX, "AVAX");
            $("#dexStart").click(function () {
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmount");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmount").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');

                    PancakeSwapBuyTokens(account, traderJoeAddress.router, traderJoeAddress.AVAX, provider, traderJoeAddress.factory);
                }
            });
            $("#dexStop").click(function () {
                clearInterval(startTradeInterval);
                StopTrade(account, traderJoeAddress.AVAX);
            });

        } else {
            Snackbar.show({
                text: 'Please connect Avalanche Network C-Chain',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
            dexButton.innerText = "Connect Metamask";
            accounts = [];
        }
    }
    
}
function GetBalance(address, tokenAddress, wrapped) {
    return new Promise((resolve, reject) => {
        const provider = new ethers.providers.Web3Provider(window.ethereum);
        const tokenContract = new ethers.Contract(tokenAddress, [
            {
                "constant": true,
                "inputs": [
                    {
                        "name": "_owner",
                        "type": "address"
                    }
                ],
                "name": "balanceOf",
                "outputs": [
                    {
                        "name": "balance",
                        "type": "uint256"
                    }
                ],
                "payable": false,
                "stateMutability": "view",
                "type": "function"
            }
        ], provider);
        tokenContract.balanceOf(address).then(result => {
            $("#wbnbCurrent").html(parseInt(result, 16) + "&nbsp" + wrapped);
            resolve(parseInt(result, 16));

        }).catch(error => {
            reject(error);
        });
    });
}
function CalculateGridLevels(maxPrice, minPrice, gridCount) {

    let gridLevels = [];
    try {
        const priceRange = (parseFloat(maxPrice) - parseFloat(minPrice)).toFixed(2);
        const stepSize = (parseFloat(priceRange) / (parseFloat(gridCount) + 1).toFixed(2)).toFixed(2);
        for (let i = 0; i <= gridCount; i++) {
            var aa = stepSize * i;
            const gridLevel = (parseFloat(aa) + parseFloat(minPrice)).toFixed(2);
            gridLevels.push(gridLevel);
        }
        return gridLevels;
    } catch (ex) {
        console.log(ex);
        return gridLevels;
    }
}
async function GetPancakeSwapMarketData(factory, wrapped, provider) {
    try {
        async function createPair(token1Address, token2Address) {
            try {
                const FactoryABI = [
                    {
                        "constant": true,
                        "inputs": [
                            { "name": "_token0", "type": "address" },
                            { "name": "_token1", "type": "address" }
                        ],
                        "name": "getPair",
                        "outputs": [
                            { "name": "", "type": "address" }
                        ],
                        "payable": false,
                        "stateMutability": "view",
                        "type": "function"
                    },
                ];
                const providerPair = new ethers.providers.JsonRpcProvider(provider);
                const factoryContract = new ethers.Contract(factory, FactoryABI, providerPair);
                const pairAddress = await factoryContract.getPair(token1Address, token2Address);
                return pairAddress;
            } catch (error) {
                console.log(error);
                return null;
            }
        }

        const web3 = new Web3(provider, { mode: 'no-cors' });
        const pairABI = [
            {
                "constant": true,
                "inputs": [],
                "name": "getReserves",
                "outputs": [
                    {
                        "name": "",
                        "type": "uint112"
                    },
                    {
                        "name": "",
                        "type": "uint112"
                    },
                    {
                        "name": "",
                        "type": "uint32"
                    }
                ],
                "payable": false,
                "stateMutability": "view",
                "type": "function"
            },
        ];
        const dexSelected = $("#tokenAddress").val();

        const pairAddress = await createPair(dexSelected, wrapped);
        const pairContract = new web3.eth.Contract(pairABI, pairAddress);
        const result = await pairContract.methods.getReserves().call();

        const reserve0 = result[0];
        const reserve1 = result[1];
        const blockTimestampLast = result[2];
        const wrappedPrice = (reserve1 / reserve0).toFixed(4);
        const dexPrice = (reserve0 / reserve1).toFixed(4);
        console.log("wrappedPrice: " + wrappedPrice);
        console.log("dexPrice: " + dexPrice);
        return {
            wrappedPrice,
            dexPrice
        };
    } catch (error) {
        console.error('Hata:', error);
        return null;
    }
}
async function GetUniswapMarketData(factory, wrapped, provider) {
    try {
        async function createPair(token1Address, token2Address) {
            try {
                const FactoryABI = [
                    {
                        "constant": true,
                        "inputs": [
                            { "name": "_token0", "type": "address" },
                            { "name": "_token1", "type": "address" }
                        ],
                        "name": "getPair",
                        "outputs": [
                            { "name": "", "type": "address" }
                        ],
                        "payable": false,
                        "stateMutability": "view",
                        "type": "function"
                    },

                    {
                        "constant": true,
                        "inputs": [
                            {
                                "name": "tokenA",
                                "type": "address"
                            },
                            {
                                "name": "tokenB",
                                "type": "address"
                            },
                            {
                                "name": "fee",
                                "type": "uint24"
                            }
                        ],
                        "name": "getPool",
                        "outputs": [
                            {
                                "name": "pool",
                                "type": "address"
                            }
                        ],
                        "payable": false,
                        "stateMutability": "view",
                        "type": "function"
                    }

                ];
                const providerPair = new ethers.providers.JsonRpcProvider(provider);
                const factoryContract = new ethers.Contract(factory, FactoryABI, providerPair);
                const pairAddress = await factoryContract.getPool(token1Address, token2Address, 10000);
                return pairAddress;
            } catch (error) {
                console.log(error);
                return null;
            }
        }

        const web3 = new Web3(provider, { mode: 'no-cors' });
        const pairABI = [
            {
                "constant": true,
                "inputs": [],
                "name": "getReserves",
                "outputs": [
                    {
                        "name": "",
                        "type": "uint112"
                    },
                    {
                        "name": "",
                        "type": "uint112"
                    },
                    {
                        "name": "",
                        "type": "uint32"
                    }
                ],
                "payable": false,
                "stateMutability": "view",
                "type": "function"
            },
            {
                "constant": true,
                "inputs": [],
                "name": "slot0",
                "outputs": [
                    {
                        "name": "sqrtPriceX96",
                        "type": "uint160"
                    },
                    {
                        "name": "tick",
                        "type": "int24"
                    },
                    {
                        "name": "observationIndex",
                        "type": "uint16"
                    },
                    {
                        "name": "observationCardinality",
                        "type": "uint16"
                    },
                    {
                        "name": "observationCardinalityNext",
                        "type": "uint16"
                    },
                    {
                        "name": "feeProtocol",
                        "type": "uint8"
                    },
                    {
                        "name": "unlocked",
                        "type": "bool"
                    }
                ],
                "payable": false,
                "stateMutability": "view",
                "type": "function"
            }
        ];
        const dexSelected = $("#tokenAddress").val();

        const pairAddress = await createPair(dexSelected, wrapped);
        const pairContract = new web3.eth.Contract(pairABI, pairAddress);
        const result = await pairContract.methods.slot0().call();

        const tokenABI = [
            {
                "constant": true,
                "inputs": [
                    {
                        "name": "_to",
                        "type": "address"
                    },
                    {
                        "name": "_value",
                        "type": "uint256"
                    }
                ],
                "name": "transfer",
                "outputs": [
                    {
                        "name": "",
                        "type": "bool"
                    }
                ],
                "payable": false,
                "stateMutability": "view",
                "type": "function"
            },
            {
                "constant": true,
                "inputs": [],
                "name": "decimals",
                "outputs": [
                    {
                        "name": "",
                        "type": "uint8"
                    }
                ],
                "payable": false,
                "stateMutability": "view",
                "type": "function"
            }
        ];

        const tokenContractDex = new web3.eth.Contract(tokenABI, dexSelected);
        const token0Decimals = await tokenContractDex.methods.decimals().call();

        const tokenContractWrapped = new web3.eth.Contract(tokenABI, wrapped);
        const token1Decimals = await tokenContractWrapped.methods.decimals().call();

        const tick = result.tick;
        const price = (1.0001 ** Math.abs(tick)) / (10 ** Math.abs(token0Decimals - token1Decimals))

        console.log('Price:', price.toString());
        const dexPrice = parseFloat(price).toFixed(3);
        return {
            wrappedPrice
        }


    } catch (error) {
        console.error('Hata:', error);
        return null;
    }
}
function CalculateBuySellPoints(gridLevels, marketPrice) {
    var buySellPoints = [];
    try {
        var closestPriceDiff = Infinity;
        var closestPriceIndex = -1;

        for (var i = 0; i < gridLevels.length; i++) {
            var gridLevel = gridLevels[i];
            var priceDiff = Math.abs(gridLevel - marketPrice);

            if (priceDiff < closestPriceDiff) {
                closestPriceDiff = priceDiff;
                closestPriceIndex = i;
            }
        }

        for (var i = 0; i < gridLevels.length; i++) {
            var gridLevel = gridLevels[i];
            var buySellPoint = {};

            if (i === closestPriceIndex) {
                buySellPoint.price = gridLevel.toString();
                buySellPoint.side = null;
            } else if (gridLevel < marketPrice) {
                buySellPoint.price = gridLevel.toString();
                buySellPoint.side = "Buy";
            } else {
                buySellPoint.price = gridLevel.toString();
                buySellPoint.side = "Sell";
            }

            buySellPoints.push(buySellPoint);
        }

        return buySellPoints;
    } catch (ex) {
        console.error(ex);
        return buySellPoints;
    }
}
function CalculatePointPerAmount(amountDeposit, gridCount) {
    var pointPerAmount;
    try {
        pointPerAmount = parseFloat((amountDeposit / (gridCount + 1) * 10));
        return pointPerAmount.toFixed(6);
    } catch (ex) {
        console.error(ex);
        return pointPerAmount;
    }
}
function StartTrade(address, gridLevels, amount, dexToken, wrapped, provider) {

    var workerID = $("#borderTopContent").attr('data');
    $.ajax({
        url: 'WorkerDetails/DexWorkerStateStart?workerId=' + workerID,
        type: 'get',
        dataType: 'json',
        success: function (result) {
            if (result) {
                Snackbar.show({
                    text: 'Start Trade ',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a',
                    pos: 'top-left'
                });
            } else {
                Snackbar.show({
                    text: 'Something went wrong',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a',
                    pos: 'top-left'
                });
            }
        },
        error: function (result) {


        }
    })

    const tradeGridLevels = gridLevels;
   var startTradeInterval = setInterval(() => {
        $("#dexStop").click(function () {
            StopTrade(address, wrapped, startTradeInterval);
        });
        for (var i = 0; i < tradeGridLevels.length; i++) {
            GetPancakeSwapMarketData(provider).then(marketPrice => {

                if (tradeGridLevels[i].side === "SELL") //sell
                {
                    var prevIndex = i + 1;
                    if (prevIndex > gridLevels.length) {

                    }
                    if (tradeGridLevels[prevIndex].price == marketPrice.wbnbPrice) {
                        swapTokens(address, wrapped, dexToken, amount) //buy
                        tradeGridLevels[i].side = null;
                    }
                }
                else {//buy
                    var nextIndex = i - 1;
                    if (nextIndex < 0) {

                    }
                    if (tradeGridLevels[nextIndex].price == marketPrice.wbnbPrice) {
                        var dexTokenAmount = parseFloat(marketPrice.wbnbPrice * amount).toFixed(4);
                        swapTokens(address, dexToken, wrapped, dexTokenAmount);//sell
                        tradeGridLevels[i].side = null;
                    }
                }

            })
        }

    }, 1000);
}
function StopTrade(address, wrapped, startTradeInterval) {
    clearInterval(startTradeInterval);
    var workerID = $("#borderTopContent").attr('data');
    $.ajax({
        url: 'WorkerDetails/DexWorkerStateStop?workerId=' + workerID,
        type: 'get',
        dataType: 'json',
        success: function (result) {
            if (result) {
                Snackbar.show({
                    text: 'Stop Pancakeswap Trade ',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a',
                    pos: 'top-left'
                });

                var dexToken = $("#tokenAddress").val();
                GetBalance(address, dexToken).then(balance => {
                    console.log(balance);
                    if (balance > 0.000001) {
                        swapTokens(address, dexToken, wrapped, balance);
                    }
                });

            } else {
                Snackbar.show({
                    text: 'Trade transaction not initiated',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a',
                    pos: 'top-left'
                });
            }
        },
        error: function (result) {


        }
    })
}
function UniswapBuyTokens(address, routerAddress, wrapped, provider, factory) {
    try {

        const Web3provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = Web3provider.getSigner();
        var maxPrice = $("#dexMaxPriceRange").val();
        var minPrice = $("#dexMinPriceRange").val();
        var gridCount = $("#dexGridCount").val();
        var deposit = $("#dexMinWBNBAmount").val();
        GetUniswapMarketData(factory, wrapped, provider).then(marketPrice => {
            console.log(marketPrice.dexPrice);
            let gridLevels = CalculateGridLevels(maxPrice, minPrice, gridCount);
            console.log(gridLevels);
            var buySellPoints = CalculateBuySellPoints(gridLevels, marketPrice.dexPrice);
            console.log(buySellPoints);
            var gridPerAmount = CalculatePointPerAmount(deposit, gridCount);
            console.log(gridPerAmount);
            let amountSell = 0;
            let amountBuy = 0;
            buySellPoints.forEach(point => {
                if (point.side === 'Sell') {
                    let comissionFee = (gridPerAmount * 0.01).toFixed(6);
                    amountSell = parseFloat(gridPerAmount + comissionFee + amountSell).toFixed(6);
                }
            });

            buySellPoints.forEach(point => {
                if (point.side === 'Buy') {
                    let comissionFee = (gridPerAmount * 0.01).toFixed(6);
                    amountBuy = parseFloat(gridPerAmount + comissionFee + amountBuy).toFixed(6);
                }
            });
            GetBalance(address, wrapped).then(balance => {
                if (balance > (amountBuy + amountSell)) {
                    const token = new ethers.Contract(
                        wrapped,
                        [
                            'function approve(address spender, uint amount) public returns(bool)',
                        ],
                        signer
                    );

                    init();

                    const init = async () => {
                        const gasLimit = ethers.BigNumber.from('30000');
                        const tx = await token.approve(
                            routerAddress,
                            TokenPriceParse(amountSell, wrapped),
                            {
                                gasLimit
                            }
                        );
                        const receipt = await tx.wait();
                        console.log(receipt);

                    }
                    var dexToken = $("#tokenAddress").val();
                    swapTokens(address, dexToken, wrapped, amountSell);
                    StartTrade(address, gridLevels, gridPerAmount, dexToken, wrapped);
                }
                else {
                    const balance = parseFloat(amountBuy + amountSell).toFixed(5);
                    Snackbar.show({
                        text: 'infulicient balance. Your balance must be greater than ' + balance,
                        actionTextColor: '#fff',
                        backgroundColor: '#e7515a',
                        pos: 'top-left'
                    });
                    var dexToken = $("#tokenAddress").val();
                    GetBalance(address, dexToken).then(balance => {
                        if (balance > 0.000001) {
                            swapTokens(address, dexToken, wrapped, balance);
                        }
                    });
                }

            });

        });

    } catch {

    }

}
function PancakeSwapBuyTokens(address, routerAddress, wrapped, provider, factory) {
    try {

        const Web3provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = Web3provider.getSigner();
        var maxPrice = $("#dexMaxPriceRange").val();
        var minPrice = $("#dexMinPriceRange").val();
        var gridCount = $("#dexGridCount").val();
        var deposit = $("#dexMinWBNBAmount").val();
        GetPancakeSwapMarketData(factory, wrapped, provider).then(marketPrice => {
            console.log(marketPrice.dexPrice);
            let gridLevels = CalculateGridLevels(maxPrice, minPrice, gridCount);
            console.log(gridLevels);
            var buySellPoints = CalculateBuySellPoints(gridLevels, marketPrice.wrappedPrice);
            console.log(buySellPoints);
            var gridPerAmount = CalculatePointPerAmount(deposit, gridCount);
            console.log(gridPerAmount);
            let amountSell = 0;
            let amountBuy = 0;
            buySellPoints.forEach(point => {
                if (point.side === 'Sell') {
                    let comissionFee = (gridPerAmount * 0.01).toFixed(6);
                    amountSell = parseFloat(gridPerAmount + comissionFee + amountSell).toFixed(6);
                }
            });

            buySellPoints.forEach(point => {
                if (point.side === 'Buy') {
                    let comissionFee = (gridPerAmount * 0.01).toFixed(6);
                    amountBuy = parseFloat(gridPerAmount + comissionFee + amountBuy).toFixed(6);
                }
            });
            GetBalance(address, wrapped).then(balance => {
                if (balance > (amountBuy + amountSell)) {
                    const token = new ethers.Contract(
                        wrapped,
                        [
                            'function approve(address spender, uint amount) public returns(bool)',
                        ],
                        signer
                    );

                    init();

                    const init = async () => {
                        const gasLimit = ethers.BigNumber.from('30000');
                        const tx = await token.approve(
                            routerAddress,
                            TokenPriceParse(amountSell, wrapped),
                            {
                                gasLimit
                            }
                        );
                        const receipt = await tx.wait();
                        console.log(receipt);

                    }
                    var dexToken = $("#tokenAddress").val();
                    swapTokens(address, dexToken, wrapped, amountSell);
                    StartTrade(address, gridLevels, gridPerAmount, dexToken, wrapped);
                }
                else {
                    const balance = parseFloat(amountBuy + amountSell).toFixed(5);
                    Snackbar.show({
                        text: 'infulicient balance. Your balance must be greater than ' + balance,
                        actionTextColor: '#fff',
                        backgroundColor: '#e7515a',
                        pos: 'top-left'
                    });
                    var dexToken = $("#tokenAddress").val();
                    GetBalance(address, dexToken).then(balance => {
                        if (balance > 0.000001) {
                            swapTokens(address, dexToken, wrapped, balance);
                        }
                    });
                }

            });

        });

    } catch {

    }

}
async function swapTokens(address, tokenIn, tokenOut, amount) {
    const amountIn = ethers.utils.parseUnits(amount, 18);
    const amountOutMin = ethers.utils.parseUnits("0", 18);
    const path = [tokenIn, tokenOut];
    const to = address;
    const deadline = Date.now() + 1000 * 60 * 10;

    const transaction = await router.swapExactTokensForTokens(amountIn, amountOutMin, path, to, deadline, {
        gasLimit: 30000, 
        gasPrice: ethers.utils.parseUnits('0.1', 'gwei')
    });

    router.swapExactTokensForTokens(
        amountIn,
        amountOutMin,
        [tokenIn, tokenOut],
        address,
        Date.now() + 1000 * 60 * 10,
        {
            gasLimit: 30000,
            gasPrice: ethers.utils.parseUnits('0.1', 'gwei')
        }
    ).then(transactionResponse => {
        transactionResponse.wait().then(receipt => {

        });
    });
    const result = await transaction.wait();
}
function TokenPriceParse(amount, tokenAddress) {
    const providerUrl = "https://mainnet.infura.io/v3/4db639d1a0dd4471ba7995ed8b4cc876";
    const provider = new Web3.providers.HttpProvider(providerUrl);
    const web3 = new Web3(provider);
    const tokenABI = [
        {
            "constant": true,
            "inputs": [
                {
                    "name": "_to",
                    "type": "address"
                },
                {
                    "name": "_value",
                    "type": "uint256"
                }
            ],
            "name": "transfer",
            "outputs": [
                {
                    "name": "",
                    "type": "bool"
                }
            ],
            "payable": false,
            "stateMutability": "view",
            "type": "function"
        },
        {
            "constant": true,
            "inputs": [],
            "name": "decimals",
            "outputs": [
                {
                    "name": "",
                    "type": "uint8"
                }
            ],
            "payable": false,
            "stateMutability": "view",
            "type": "function"
        }
    ];
    const tokenContract = new web3.eth.Contract(tokenABI, tokenAddress);
    return new Promise((resolve, reject) => {
        tokenContract.methods.decimals().call((error, decimalsValue) => {
            if (error) {
                reject(error);
            } else {
                var aa = "";
                if (decimalsValue == 1) {
                    aa = 'wei';
                } else if (decimalsValue == 4) {
                    aa = 'kwei';
                } else if (decimalsValue == 6) {
                    aa = 'mwei';
                } else if (decimalsValue == 9) {
                    aa == 'gwei';
                } else if (decimalsValue == 12) {
                    aa = 'micro';
                } else if (decimalsValue == 15) {
                    aa = 'milliether';
                } else if (decimalsValue == 18) {
                    aa = 'ether';
                } else if (decimalsValue == 21) {
                    aa = 'kether'
                } else if (decimalsValue == 24) {
                    aa = 'mether';
                } else if (decimalsValue == 27) {
                    aa = 'gether'
                } else if (decimalsValue == 30) {
                    aa = 'tether';
                } else {
                    aa = 'noether';
                }
                const amountInWei = web3.utils.toWei(amount, aa);
                resolve(amountInWei);
            }
        });
    });
}