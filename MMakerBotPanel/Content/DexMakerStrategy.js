$(document).ready(function () {
    $(".placeholder").select2({
        placeholder: "Make a Selection",
        allowClear: false
    });
    DexConnectWallet();
});

function DexConnectWallet() {
    $(".dexConnectWalletMaker").click(function () {
        const dexButton = document.querySelector('.dexConnectWalletMaker');
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
                if ($("#tokenAddressMaker").val() != "") {
                    var dexSelected = $('#Detail_DexExchange').val();
                    Options(dexSelected, chainSymbol, accounts[0]);
                } else {
                    Snackbar.show({
                        text: 'Please enter the token address',
                        actionTextColor: '#fff',
                        backgroundColor: '#e7515a',
                        pos: 'top-left'
                    });
                    const dexButton = document.querySelector('.dexConnectWalletMaker');
                    dexButton.innerText = "Connect Metamask";
                }
            });
        }
    });
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
            $("#wbnbCurrentMaker").html(parseInt(result, 16) + "&nbsp" + wrapped);
            resolve(parseInt(result, 16));

        }).catch(error => {
            reject(error);
        });
    });
}
function Options(dexSelected, chainSymbol, account) {
    const dexButton = document.querySelector('.dexConnectWalletMaker');
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
            GetTokenSymbol("WBNB", provider);
            DexCalculate(pancakeswapAddress.WBNB, provider, pancakeswapAddress.factory);
            GetBalance(account, pancakeswapAddress.WBNB, "WBNB");
            $("#dexStart").click(function () {
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmountMaker");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmountMaker").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');

                    PancakeSwapBuyTokens(account, pancakeswapAddress.router, pancakeswapAddress.WBNB, provider, pancakeswapAddress.factory);
                }
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
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmountMaker");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmountMaker").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');

                    PancakeSwapBuyTokens(account, uniswapAddress.router, uniswapAddress.ETH, provider, uniswapAddress.factory);
                }
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
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmountMaker");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmountMaker").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');

                    UniswapBuyTokens(account, uniswapAddress.router, uniswapAddress.ETH, provider, uniswapAddress.factory);
                }
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
                var dexMinWBNBAmount = document.getElementById("dexMinWBNBAmountMaker");
                var amount = dexMinWBNBAmount.placeholder;
                var dexMinWBNBAmount = $("#dexMinWBNBAmountMaker").val();
                if (amount.substring(2) > dexMinWBNBAmount) {
                    $("#dexMinWBNB").css('display', 'block');
                } else {
                    $("#dexMinWBNB").css('display', 'none');

                    PancakeSwapBuyTokens(account, traderJoeAddress.router, traderJoeAddress.AVAX, provider, traderJoeAddress.factory);
                }
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
function GetTokenSymbol(wrapped, provider) {
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

    const tokenAddress = $("#tokenAddressMaker").val();
    getTokenSymbol(tokenAddress)
        .then(symbol => {
            console.log(symbol);
            var html = "" + wrapped + " --> " + symbol + "";
            document.getElementById("dexPairMaker").value = html;
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
function DexCalculate(WRAPPER_ADDRESS, providerSymbol, factoryAddress) {
    $("#dexCalculateMaker").click(function () {
        var amount = $("#dexAmountMaker").val();
        var dexSelected = $('#Detail_DexExchange').val();
        if (amount != null) {
            GetPancakeSwapMarketData(factoryAddress, WRAPPER_ADDRESS, providerSymbol).then(marketPrice => {
                let volume = parseFloat(amount / marketPrice.wrappedPrice).toFixed(2);
                volume = volume * 360;
                $("#dexVolumeMaker").val(volume);
                return volume;
            })
        }
        if (amount != null && dexSelected == "UniswapV3") {
            GetUniswapMarketData(factoryAddress, WRAPPER_ADDRESS, providerSymbol).then(marketpRICE => {
                let volume = parseFloat(amount / marketPrice.wrappedPrice).toFixed(2);
                volume = volume * 360;
                $("#dexVolumeMaker").val(volume);
                return volume;
            });
        }
        if (amount == null) {
            Snackbar.show({
                text: 'Please fill in the amount blank',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
        }
    });
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
        const dexSelected = $("#tokenAddressMaker").val();

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
        const dexPrice = parseFloat(price).toFixed(3);
        return {
            wrappedPrice,
            dexPrice
        }


    } catch (error) {
        console.error('Hata:', error);
        return null;
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
        const dexSelected = $("#tokenAddressMaker").val();
        const pairAddress = await createPair(dexSelected, wrapped);
        const pairContract = new web3.eth.Contract(pairABI, pairAddress);
        const result = await pairContract.methods.getReserves().call();

        const reserve0 = result[0];
        const reserve1 = result[1];
        const blockTimestampLast = result[2];
        const wrappedPrice = (reserve1 / reserve0).toFixed(4);
        console.log(wrappedPrice);
        const dexPrice = (reserve0 / reserve1).toFixed(4);
        return {
            wrappedPrice,
            dexPrice
        };
    } catch (error) {
        return null;
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
function UniswapBuyTokens(address, routerAddress, wrapped, provider, factory) {
    try {

        const Web3provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = Web3provider.getSigner();
        var deposit = $("#dexMinWBNBAmountMaker").val();
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
                TokenPriceParse(deposit, wrapped),
                {
                    gasLimit
                }
            );
            const receipt = await tx.wait();
            console.log(receipt);
        }
        var dexToken = $("#tokenAddressMaker").val();
        GetUniswapMarketData(factory.wrapped, provider).then(price => {
            var dexAmount = parseFloat(amount * price).toFixed(3);
            const dextokenContract = new ethers.Contract(
                dexToken,
                [
                    'function approve(address spender, uint amount) public returns(bool)',
                ],
                signer
            );

            init();

            const init = async () => {
                const gasLimit = ethers.BigNumber.from('30000');
                const tx = await dextokenContract.approve(
                    routerAddress,
                    TokenPriceParse(dexAmount, wrapped),
                    {
                        gasLimit
                    }
                );
                const receipt = await tx.wait();
                console.log(receipt);
            }

            StartTrade(address, amount, dexAmount, dexToken, wrapped);
        })


    } catch {

    }

}
function PancakeSwapBuyTokens(address, routerAddress, wrapped, provider, factory) {
    try {
        const Web3provider = new ethers.providers.Web3Provider(window.ethereum);
        const signer = Web3provider.getSigner();
        var deposit = $("#dexMinWBNBAmountMaker").val();
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
                TokenPriceParse(deposit, wrapped),
                {
                    gasLimit
                }
            );
            const receipt = await tx.wait();
            console.log(receipt);
        }
        var dexToken = $("#tokenAddressMaker").val();
        GetPancakeSwapMarketData(factory.wrapped, provider).then(price => {
            var dexAmount = parseFloat(amount * price).toFixed(3);
            const dextokenContract = new ethers.Contract(
                dexToken,
                [
                    'function approve(address spender, uint amount) public returns(bool)',
                ],
                signer
            );

            init();

            const init = async () => {
                const gasLimit = ethers.BigNumber.from('30000');
                const tx = await dextokenContract.approve(
                    routerAddress,
                    TokenPriceParse(dexAmount, wrapped),
                    {
                        gasLimit
                    }
                );
                const receipt = await tx.wait();
                console.log(receipt);
            }

            StartTrade(address, amount, dexAmount, dexToken, wrapped);
        })

    } catch {

    }


}
function StartTrade(address, amount, dexAmount, dexToken, wrapped) {
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

    var startTradeInterval = setInterval(() => {
        $("#dexStopMaker").click(function () {
            StopTrade(address, wrapped, startTradeInterval);
        });
        GetBalance(address, wrapped).then(balance => {
            if (amount < balance) {
                swapTokens(address, wrapped, dexToken, amount);
            }
            else {
                StopTrade(address, wrapped, startTradeInterval)
                Snackbar.show({
                    text: 'Insufficient balance',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a',
                    pos: 'top-left'
                });
            }
        });
        setTimeout(() => {
            GetBalance(address, dexToken).then(balance => {
                if (dexAmount < balance) {
                    swapTokens(address, dexToken, wrapped, dexAmount);
                }
                else {
                    StopTrade(address, wrapped, startTradeInterval)
                    Snackbar.show({
                        text: 'Insufficient balance',
                        actionTextColor: '#fff',
                        backgroundColor: '#e7515a',
                        pos: 'top-left'
                    });
                }
            });

        }, 5000)
    }, 5000);

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