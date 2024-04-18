$(document).ready(function () {
    ExtendTimeInfo();
    SendTransection();
});


function handleMessage(transection, accountWallet, receiverWallet, priceShortName, price, yearlyCheck, WorkerId, success, gasLimit) {
    $.ajax({
        url: "/Workers/PurchaseHistory",
        type: "POST",
        dataType: "json",
        data: {
            transection: transection,
            accountWallet: accountWallet,
            receiverWallet: receiverWallet,
            priceShortName: priceShortName,
            price: price,
            yearlyCheck: yearlyCheck,
            productId: "0",
            workerId: WorkerId,
            success: success,
            gasfee: gasLimit
        },
        success: function (result) {
            if (success) {
                window.location.href = "/Workers/Index";
            }
        },
        error: function (result) {
            console.log(result);
        }
    });
}

function BuyWithToken(accounts, walletAddress, priceParse, priceShortName, price, yearlyCheck, productId) {
    var gas = "3000";
    $.ajax({
        url: "/Workers/GetContractAddress?shortName=" + priceShortName,
        type: "GET",
        dataType: "json",
        success: function (result) {
            ethereum.request({
                method: 'eth_sendTransaction',
                params: [{
                    from: accounts[0],
                    to: result,
                    data: getDataFieldValue(walletAddress, priceParse),
                    gas: gas.toString(16),
                    maxPriorityFeePerGas: null,
                    maxFeePerGas: null,
                }],
            }).then((txId) => handleMessage(txId, accounts[0], walletAddress, priceShortName, price, yearlyCheck, productId, true, '0x76c0'))
                .catch((err) => handleMessage(err.message, accounts[0], walletAddress, priceShortName, price, yearlyCheck, productId, false, '0x76c0'));

        }
    });
}

function getDataFieldValue(tokenRecipientAddress, tokenAmount) {
    const web3 = new Web3();
    const TRANSFER_FUNCTION_ABI = {
        "constant": false,
        "inputs": [{ "name": "_to", "type": "address" }, { "name": "_value", "type": "uint256" }],
        "name": "transfer",
        "outputs": [],
        "payable": false,
        "stateMutability": "nonpayable",
        "type": "function"
    };

    return web3.eth.abi.encodeFunctionCall(TRANSFER_FUNCTION_ABI, [
        tokenRecipientAddress,
        tokenAmount
    ]);
}

function BuyMetamask(accounts, walletAddress, priceParse, priceShortName, price, yearlyCheck, productId) {
    var gas = "3000";
    ethereum.request({
        method: 'eth_sendTransaction',
        params: [{
            from: accounts[0],
            to: walletAddress,
            gas: gas.toString(16), // 30400
            /* gasPrice: '0x9184e72a000', // 10000000000000,*/
            value: priceParse,
        }],
    }).then((txId) => handleMessage(txId, accounts[0], walletAddress, priceShortName, price, yearlyCheck, productId, true, gas))
        .catch((err) => handleMessage(err.message, accounts[0], walletAddress, priceShortName, price, yearlyCheck, productId, false, gas));

}

function EthPriceParse(amount) {
    var price = Number(amount * 1e18).toString(16);
    return price;
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

function GetWallet() {
    return $.ajax({
        url: '/Workers/GetWallet',
        type: "GET",
        dataType: "json",
        success: function (result) {


        }
    });
}

function SendTransection() {
    const ethereumButton = document.querySelector('.enableEthereumButton');
    let accounts = [];

    ethereumButton.addEventListener('click', () => {
        getAccount();
        getChainNetwork();

    });
    window.ethereum.on('networkChanged', function () {
        getChainNetwork();

    });
    async function getAccount() {
        accounts = await ethereum.request({ method: 'eth_requestAccounts' });
        ethereumButton.innerText = accounts;
    }

    $('.BuyButton').click(function () {
        if (accounts.length == 0) {
            Snackbar.show({
                text: 'Please connect your metamask wallet',
                actionTextColor: '#fff',
                backgroundColor: '#e7515a',
                pos: 'top-left'
            });
        }
    });

    async function getChainNetwork() {

        chainId = await ethereum.request({ method: 'eth_chainId' }).then(chainSymbol => {
            if (chainSymbol == 0x1) {
                $('.BuyButton').click(function () {
                    if (chainSymbol == 0x1) {
                        var yearlyCheck = $("input[name='custom-radio-1']:checked").val();
                        var priceShortName = $('#priceName').val();
                        var contractAdress = document.querySelector('#priceName option:checked').getAttribute('data');
                        if (yearlyCheck == 1) {
                            yearlyCheck = true;
                        } else {
                            yearlyCheck = false;
                        }

                        var WorkerId = $('#ExtendTime').attr('data');
                        GetWallet().done(function (result) {
                            if (yearlyCheck) {
                                var priceYearly = $('#price').html();
                                if (priceYearly != "") {
                                    if (priceShortName == "ETH") {
                                        BuyMetamask(accounts, result, EthPriceParse(priceYearly), priceShortName, priceYearly, yearlyCheck, WorkerId);
                                    }
                                    else {
                                        TokenPriceParse(priceYearly, contractAdress)
                                            .then((amountInWei) => {
                                                BuyWithToken(accounts, result, amountInWei, priceShortName, priceYearly, yearlyCheck, WorkerId);
                                            })
                                            .catch((error) => {
                                                // Hata durumunda burası çalışır
                                                console.error("Hata:", error);
                                            });
                                    }
                                    //BuyWithToken(accounts, result, TokenPriceParse(priceYearly), priceShortName, priceYearly, yearlyCheck, WorkerId);

                                } else {
                                    Snackbar.show({
                                        text: 'Please select yearly or monthly',
                                        actionTextColor: '#fff',
                                        backgroundColor: '#e7515a',
                                        pos: 'top-left'
                                    });
                                }

                            }
                            else {
                                var priceMonthly = $('#price').html();
                                if (priceMonthly != "") {
                                    if (priceShortName == "ETH") {
                                        BuyMetamask(accounts, result, EthPriceParse(priceMonthly), priceShortName, priceMonthly, yearlyCheck, WorkerId);
                                    }
                                    else {
                                        TokenPriceParse(priceMonthly, contractAdress)
                                            .then((amountInWei) => {
                                                BuyWithToken(accounts, result, amountInWei, priceShortName, priceMonthly, yearlyCheck, WorkerId);
                                            })
                                            .catch((error) => {
                                                // Hata durumunda burası çalışır
                                                console.error("Hata:", error);
                                            });
                                    }


                                    //BuyWithToken(accounts, result, TokenPriceParse(priceMonthly), priceShortName, priceMonthly, yearlyCheck, WorkerId);

                                } else {
                                    Snackbar.show({
                                        text: 'Please select yearly or monthly',
                                        actionTextColor: '#fff',
                                        backgroundColor: '#e7515a',
                                        pos: 'top-left'
                                    });
                                }

                            }
                        });
                    }
                    else {
                        Snackbar.show({
                            text: 'Please connect Ethereum Mainnetwork ',
                            actionTextColor: '#fff',
                            backgroundColor: '#e7515a',
                            pos: 'top-left'
                        });
                    }
                });

            }
            else {
                Snackbar.show({
                    text: 'Please connect Ethereum Mainnet',
                    actionTextColor: '#fff',
                    backgroundColor: '#e7515a',
                    pos: 'top-left'
                });
                ethereumButton.innerText = "Connect Metamask";
                accounts = [];
            }
        });
    }
}

function ExtendTimeInfo() {
    $('#ExtendTime').click(function () {
        var workerId = $(this).attr('data');
        $.ajax({
            url: "/Workers/ExtendTimeInfo?workerId=" + workerId,
            type: "Get",
            dataType: "json",
            success: function (result) {
                if (!result) {

                }
                else {
                    $('#ProductName').html(result.ProductName);
                    $('#Product').html(result.ProductName);
                    $('#Product').attr('value', result.ProductId);
                    $('#State').html(result.WorkingState);
                    $('#StartDate').html(result.StartDate);
                    $('#EndDate').html(result.EndDate);
                    $('#Days').html(result.DaysRemaining);
                    var productId = $('#Product').attr('value');
                    $.ajax({
                        url: '/Workers/GetSelectProductPrice?productId=' + productId,
                        type: "GET",
                        dataType: "json",
                        success: function (result) {
                            $('#priceName').find('option').remove();
                            for (var i = 0; i < result.length; i++) {
                                $("#priceName").append("<option  style='color:white'  data='" + result[i].Contract + "' >" + result[i].PriceName + "</option>").selectpicker('refresh');
                            }
                            $('#priceName').selectpicker({
                                size: '2'
                            });
                            GetTotalPrice();

                            document.getElementById('priceName').addEventListener('change', function () {
                                var radioButtons = document.getElementsByName('custom-radio-1');
                                var radioPriceName = 0;
                                for (var i = 0; i < radioButtons.length; i++) {
                                    if (radioButtons[i].checked) {
                                        var checkedRadioButton = radioButtons[i];
                                        radioPriceName = checkedRadioButton.value;
                                        break;
                                    }
                                }
                                var priceName = $("#priceName").val();
                                var productId = $('#Product').attr('value');
                                $.ajax({
                                    url: '/Workers/GetTotalPrice?productId=' + productId + "&priceRadio=" + radioPriceName + "&priceName=" + priceName,
                                    type: "GET",
                                    dataType: "json",
                                    success: function (result) {
                                        $('#price').html(result);
                                    },
                                    error: function () {

                                    }
                                });
                            });

                        },
                        error: function () {

                        }
                    });
                }
            },
            error: function (result) {

            }
        });
    });
}

function GetTotalPrice() {
    $(function () {
        $(document).on('change', 'input[name="custom-radio-1"]', function () {
            var radioPriceName = $(this).val();
            var priceName = $("#priceName").val();
            var productId = $('#Product').attr('value');
            $.ajax({
                url: '/Workers/GetTotalPrice?productId=' + productId + "&priceRadio=" + radioPriceName + "&priceName=" + priceName,
                type: "GET",
                dataType: "json",
                success: function (result) {
                    $('#price').html(result);
                },
                error: function () {

                }
            });
        });
    });
}
