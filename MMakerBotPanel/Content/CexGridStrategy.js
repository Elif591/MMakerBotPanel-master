
$(document).ready(function () {

    AutoGridStart();
    ManualGridStart();
    ManualCalculate();
    GetTimeIntervalEnum();
    GetCalculate();

});

function ManualGridStart() {
    $("#manualGridStart").click(function () {
        ManualGridSaveParameter();

    });
    $("#manualGridStop").click(function () {
        StopGridStrategy();
    });
}

function AutoGridStart() {
    $("#automaticGridStart").click(function () {
        AutoGridSaveParameter();

    });
    $("#autoGridStop").click(function () {
        StopGridStrategy();
    });
}

function ManualGridSaveParameter() {
    var minUsdtAmount = document.getElementById('manualMinUsdtAmount');
    var amount = minUsdtAmount.placeholder;

    var manualMinAmount = $("#manualMinUsdtAmount").val();
    if (amount.substring(2) > manualMinAmount) {
        $("#dexMinWBNB").css('display', 'block');
    } else {
        $("#dexMinWBNB").css('display', 'none');
        var profitGrid = $("#manualProfitGrid").val();
        var profitGrids = profitGrid.split('~');
        minProfitPerGrid = profitGrids[1].trim();
        maxProfitPerGrid = profitGrids[0].trim();
        $.ajax({
            url: "/WorkerDetails/SaveGridParameter",
            type: "POST",
            dataType: 'json',
            data: {
                //string maxPrice, string minPrice, int gridCount, int workerID, string type, string minProfitPerGrid, string maxProfitPerGrid, string takeProfit, string stoplass, string mintether , string exchange , string parity
                maxPrice: $("#manualMaxPriceRange").val(),
                minPrice: $("#manualMinPriceRange").val(),
                gridCount: $("#manualGridCount").val(),
                workerID: $("#borderTopContent").attr('data'),
                type: "manual",
                minProfitPerGrid: minProfitPerGrid,
                maxProfitPerGrid: maxProfitPerGrid,
                takeProfit: $("#manualTakeProfit").val(),
                stoplass: $("#manualStopLoss").val(),
                mintether: $("#manualMinUsdtAmount").val(),
                exchange: $('#Detail_CexExchange').val(),
                parity: $('#paritySelect').val()
            },
            success: function (result) {
                ManualStartGridStrategy();


            },
            error: function (result) {

            }
        });
    }
}

function AutoGridSaveParameter() {
    var minUsdtAmount = document.getElementById('minUsdtAmount');
    var amount = minUsdtAmount.placeholder;
    var autoMinAmount = $("#minUsdtAmount").val();
    if (amount.substring(2) > autoMinAmount) {
        $("#autoMintether").css('display', 'block');
    } else {
        $("#autoMintether").css('display', 'none');
        var priceRange = $("#priceRange").val();
        var strings = priceRange.split("-");
        var maxPrice = strings[0].trim();
        var minPrice = strings[1].trim().replace("USDT", "");
        var profitGrid = $("#profitGrid").val();
        var profitGrids = profitGrid.split('~');
        minProfitPerGrid = profitGrids[1].trim();
        maxProfitPerGrid = profitGrids[0].trim();
        $.ajax({
            url: "/WorkerDetails/SaveGridParameter",
            type: "POST",
            dataType: 'json',
            data: {
                //string maxPrice, string minPrice, int gridCount, int workerID, string type, string minProfitPerGrid, string maxProfitPerGrid, string takeProfit, string stoplass, string mintether , string exchange , string parity
                maxPrice: maxPrice,
                minPrice: minPrice,
                gridCount: $("#gridCount").val(),
                workerID: $("#borderTopContent").attr('data'),
                type: "auto",
                minProfitPerGrid: minProfitPerGrid,
                maxProfitPerGrid: maxProfitPerGrid,
                takeProfit: $("#takeProfit").val(),
                stoplass: $("#stopLoss").val(),
                mintether: $("#minUsdtAmount").val(),
                exchange: $('#Detail_CexExchange').val(),
                parity: $('#paritySelect').val()
            },
            success: function (result) {
                AutoStartGridStrategy();


            },
            error: function (result) {

            }
        });
    }
}

function ManualStartGridStrategy() {
    var minAmount = $("#manualMinUsdtAmount").val();
    var workerId = $("#borderTopContent").attr('data');
    $.ajax({
        url: "/WorkerDetails/StartGridStrategy",
        type: "POST",
        dataType: 'json',
        data: {
            //string deposit , int workerID ,string type
            deposit: minAmount,
            workerID: workerId,
            type: "manual"
        },
        success: function (result) {

            if (!result) {
                $("#autoinvalidBalance").css('display', 'block');
            } else {
                $("#autoinvalidBalance").css('display', 'none');
            }

        },
        error: function (result) {

        }
    });
}

function AutoStartGridStrategy() {
    var autoMinAmount = $("#minUsdtAmount").val();
    var workerId = $("#borderTopContent").attr('data');
    $.ajax({
        url: "/WorkerDetails/StartGridStrategy",
        type: "POST",
        dataType: 'json',
        data: {
            //string deposit , int workerID ,string type
            deposit: autoMinAmount,
            workerID: workerId,
            type: "auto"
        },
        success: function (result) {

            if (!result) {
                $("#autoinvalidBalance").css('display', 'block');
            } else {
                $("#autoinvalidBalance").css('display', 'none');
            }

        },
        error: function (result) {

        }
    });
}

function StopGridStrategy() {
    var workerId = $("#borderTopContent").attr('data');
    $.ajax({
        url: "/WorkerDetails/StopGridStrategy",
        type: "POST",
        dataType: 'json',
        data: {
            workerID: workerId,
        },
        success: function (result) {

            if (!result) {
                $("#autoinvalidBalance").css('display', 'block');
            } else {
                $("#autoinvalidBalance").css('display', 'none');
            }

        },
        error: function (result) {

        }
    });
}

function GetCalculate() {
    $("#calculate").click(function () {
        var exchange = $("#Detail_CexExchange").val();
        var timeInterval = $("#timeRange").val();
        var parity = $("#paritySelect").val();

        if (exchange != null && timeInterval != null && parity != null) {
            GetGridParameter(exchange, timeInterval, parity);
        }
        else {

        }
    });
}

function GetGridParameter(exchange, timeInterval, parity) {
    var workerID = $("#borderTopContent").attr('data');
    $.ajax({
        url: '/WorkerDetails/GetGridParameter',
        type: 'Post',
        dataType: 'json',
        data: {
            exchange: exchange,
            timeInterval: timeInterval,
            symbol: parity,
            workerID: workerID
        },
        success: function (result) {
            var priceRange = result.MinPrice + " - " + result.MaxPrice + "  USDT";
            $("#priceRange").val(priceRange);
            $("#gridCount").val(result.GridCount);
            $("#profitGrid").val(result.MinProfitPerGrid + " % ~ " + result.MaxProfitPerGrid + " %");
            $("#takeProfit").val(result.TakeProfit);
            $("#stopLoss").val(result.StopLoss);
            var minUsdtAmount = document.getElementById('minUsdtAmount');
            var text = ">=" + result.MinTetherAmount;
            minUsdtAmount.placeholder = text;
        },
        error: function () {

        }
    });

}

function GetTimeIntervalEnum() {
    $.ajax({
        url: '/WorkerDetails/GetEnumTimeInterval',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            var selectpicker = $('#timeRange');

            $.each(data, function (index, item) {
                var option = $('<option>', {
                    value: item.Key,
                    text: item.Value
                });

                selectpicker.append(option);
            });

            selectpicker.selectpicker('refresh');
        },
        error: function () {

        }
    });
}

function ManualCalculate() {
    $("#manualCalculate").click(function () {
        var maxPrice = $("#manualMaxPriceRange").val();
        var minPrice = $("#manualMinPriceRange").val();
        var gridCount = $("#manualGridCount").val();
        var invalidGridCount = $("#invalid-GridCount");
        var workerID = $("#borderTopContent").attr('data');
        invalidGridCount.css('display', 'none');
        if (minPrice != null && maxPrice != null && gridCount != null) {

            if (parseFloat(gridCount) > 2 && parseFloat(gridCount) < 170) {
                invalidGridCount.css('display', 'none');
                if (parseFloat(minPrice) < parseFloat(maxPrice)) {
                    $.ajax({
                        url: "/WorkerDetails/GetManualGridParameter",
                        type: "POST",
                        dataType: 'json',
                        data: {
                            maxPrice: maxPrice,
                            minPrice: minPrice,
                            gridCount: gridCount,
                            workerID: workerID
                        },
                        success: function (result) {
                            $("#manualProfitGrid").val(result.MinProfitPerGrid + " % ~ " + result.MaxProfitPerGrid + " %");
                            $("#manualTakeProfit").val(result.TakeProfit);
                            $("#manualStopLoss").val(result.StopLoss);
                            var minUsdtAmount = document.getElementById('manualMinUsdtAmount');
                            var text = ">=" + result.MinTetherAmount;
                            minUsdtAmount.placeholder = text;
                        },
                        error: function (result) {

                        }
                    });
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