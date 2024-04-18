$(document).ready(function () {

    GetRiskLevel();
    MakerStart();

});




function GetRiskLevel() {
    $.ajax({
        url: "/WorkerDetails/GetRiskLevel",
        type: "get",
        dataType: "json",
        success: function (result) {
            if (result == 1) {
                $("#riskLevel").val("Low");
                $("#makerStart").css('display', 'block');
            } else if (result == 2) {
                $("#riskLevel").val("Medium");
                $("#makerStart").css('display', 'block');
            } else if (result == 3) {
                $("#riskLevel").val("High");
                $("#makerStart").css('display', 'block');
            } else {
                $("#riskLevel").val("Please solve your risk test");
                $("#makerStart").css('display', 'none');
            }
        },
        error: function (result) {

        }
    })
}

function SaveMakerParameter() {
    var minMakerUsdt = document.getElementById("minUsdtMMakerBot");
    var amount = minMakerUsdt.placeholder;
    var makerUsdtAmount = $("#minUsdtMMakerBot").val();
    if (amount.substring(2) > makerUsdtAmount) {
        $("#minTetherMMakerBot").css('display', 'block');
    } else {
        $("#minTetherMMakerBot").css('display', 'none');
        var takeProfit = $("#takeProfitMaker").val();
        var stopLoss = $("#stopLossMaker").val();
    }

    $.ajax({
        url: '/WorkerDetails/SaveMakerParameter',
        type: 'post',
        dataType: 'json',
        data: {
            workerID: $("#borderTopContent").attr('data'),
            takeProfit: takeProfit,
            stoplass: stopLoss,
            mintether: makerUsdtAmount,
            exchange: $('#Detail_CexExchange').val(),
            parity: $('#paritySelect').val(),
        },
        success: function (result) {
            StartMakerStrategy();
        },
        error: function (result) {

        }
    })
}

function MakerStart() {
    $("#makerStart").click(function () {
        SaveMakerParameter();
    });
    $("#makerStop").click(function () {
        StopMakerStrategy();
    });
}

function StopMakerStrategy() {
    var workerId = $("#borderTopContent").attr('data');
    $.ajax({
        url: "/WorkerDetails/StopMakerStrategy?workerID=" + workerId,
        type: "get",
        dataType: "json",
        success: function (result) {
            if (!result) {
                $("#invalikBalanceMMakerBot").css('display', 'block');
            } else {
                $("#invalikBalanceMMakerBot").css('display', 'none');
            }
        },
        error: function (result) {

        }

    })
}

function StartMakerStrategy() {
    //string deposit, int workerID , string exchange , string parity
    $.ajax({
        url: "/WorkerDetails/StartMakerStrategy",
        type: "post",
        dataType: "json",
        data: {
            deposit: $("#minUsdtMMakerBot").val(),
            workerID: $("#borderTopContent").attr('data'),
            exchange: $('#Detail_CexExchange').val(),
            parity: $('#paritySelect').val()
        },
        success: function (result) {
            if (!result) {
                $("#invalikBalanceMMakerBot").css('display', 'block');
            } else {
                $("#invalikBalanceMMakerBot").css('display', 'none');
            }

        },
        errorr: function (result) {

        }
    })
}