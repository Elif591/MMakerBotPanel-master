$(document).ready(function () {

    var valueSelected = $('#Detail_CexExchange').val();
    if (valueSelected != "0") {
        getCexExchangeParity(valueSelected);
    }
    $('#Detail_CexExchange').on('change', function (e) {
        deleteSelectOption();
        GetCurrentUSDT();
        $("#priceRange").val('');
        $("#gridCount").val('');
        $("#profitGrid").val('');
        $("#takeProfit").val('');
        $("#stopLoss").val('');
        $("#currentUSDT").val('');
        var valueSelected = $(this).val();
        if (valueSelected == "0") {
            deleteSelectOption();
        } else {
            clearInterval(dynamicData);
            getCexExchangeParity(valueSelected);
        }
    });

    $('#paritySelect').on('change', function (e) {
        clearInterval(dynamicData);
        selectedParity();
    });
});

function GetCurrentUSDT() {
    var exchange = $("#Detail_CexExchange").val();
    $.ajax({
        url: '/WorkerDetails/GetCurrentUSDT?exchange=' + exchange,
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            if (result == "400") {
                $("#currentUSDT").html("0.000");
                $("#manualCurrentUSDT").html("0.000");
            } else {
                $("#currentUSDT").html(result);
                $("#manualCurrentUSDT").html(result);
            }
        },
        error: function () {

        }
    });

}
function deleteSelectOption() {
    var options = $('#paritySelect option');
    $.map(options, function (option) {
        option.remove();
    });
}
function getCexExchangeParity(valueSelected) {
    $.ajax({
        url: "/Workers/GetParity",
        type: "POST",
        dataType: 'json',
        data: {
            "CexExchange": valueSelected
        },
        success: function (result) {
            var mySelect = $('#paritySelect');
            mySelect.empty(); // Parityleri temizle
            $.each(result.Pairs, function (i) {
                mySelect.append(
                    $('<option></option>').val(result.Pairs[i].symbol).html(result.Pairs[i].symbol)
                );
            });
            setTimeout(function () {
                var paritySelected = $('#paritySelect').val();
                if (paritySelected != null) {
                    selectedParity();
                }
            }, 1000);

        },
        error: function (result) {

        }
    });
}
function selectedParity() {
    var selectedParity = $('#paritySelect').val();
    var valueSelected = $('#Detail_CexExchange').val();
    if (selectedParity != "0" && valueSelected != "0") {
        $.ajax({
            url: "/Workers/GetChart",
            type: "POST",
            dataType: 'json',
            data: {
                "CexExchange": valueSelected,
                "symbol": selectedParity
            },
            success: function (result) {
                $('#chartContainer').html('<div id="chartcontrols" style="padding-left:15px"></div><div id="chartdiv"></div>');
                $('.chart').css("display", "inline");

                am5.ready(function () {

                    // Create root element
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                    var root = am5.Root.new("chartdiv");
                    // Set themes
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/concepts/themes/
                    root.setThemes([
                        am5themes_Dark.new(root)
                    ]);
                    // Create a stock chart
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/stock-chart/#Instantiating_the_chart
                    var stockChart = root.container.children.push(
                        am5stock.StockChart.new(root, {})
                    );

                    // Set global number format
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/concepts/formatters/formatting-numbers/
                    root.numberFormatter.set("numberFormat", "####.0000");

                    // Create a main stock panel (chart)
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/stock-chart/#Adding_panels
                    var mainPanel = stockChart.panels.push(
                        am5stock.StockPanel.new(root, {
                            wheelY: "zoomX",
                            panX: true,
                            panY: true
                        })
                    );

                    // Create value axis
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                    var valueAxis = mainPanel.yAxes.push(
                        am5xy.ValueAxis.new(root, {
                            renderer: am5xy.AxisRendererY.new(root, {
                                pan: "zoom"
                            }),
                            extraMin: 0.1, // adds some space for for main series
                            tooltip: am5.Tooltip.new(root, {}),
                            numberFormat: "####.0000",
                            extraTooltipPrecision: 2
                        })
                    );

                    var dateAxis = mainPanel.xAxes.push(
                        am5xy.GaplessDateAxis.new(root, {
                            baseInterval: {
                                timeUnit: "hour",
                                count: 4
                            },
                            renderer: am5xy.AxisRendererX.new(root, {}),
                            tooltip: am5.Tooltip.new(root, {})
                        })
                    );

                    // add range which will show current value
                    var currentValueDataItem = valueAxis.createAxisRange(valueAxis.makeDataItem({ value: 0 }));
                    var currentLabel = currentValueDataItem.get("label");
                    if (currentLabel) {
                        currentLabel.setAll({
                            fill: am5.color(0xffffff),
                            background: am5.Rectangle.new(root, { fill: am5.color(0x000000) })
                        })
                    }

                    var currentGrid = currentValueDataItem.get("grid");
                    if (currentGrid) {
                        currentGrid.setAll({ strokeOpacity: 0.5, strokeDasharray: [2, 5] });
                    }


                    // Add series
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                    var valueSeries = mainPanel.series.push(
                        am5xy.CandlestickSeries.new(root, {
                            name: selectedParity,
                            clustered: false,
                            valueXField: "Date",
                            valueYField: "Close",
                            highValueYField: "High",
                            lowValueYField: "Low",
                            openValueYField: "Open",
                            calculateAggregates: true,
                            xAxis: dateAxis,
                            yAxis: valueAxis,
                            legendValueText:
                                "open: [bold]{openValueY}[/] high: [bold]{highValueY}[/] low: [bold]{lowValueY}[/] close: [bold]{valueY}[/]",
                            legendRangeValueText: ""
                        })
                    );

                    // Set main value series
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/stock-chart/#Setting_main_series
                    stockChart.set("stockSeries", valueSeries);

                    // Add a stock legend
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/stock-chart/stock-legend/
                    var valueLegend = mainPanel.plotContainer.children.push(
                        am5stock.StockLegend.new(root, {
                            stockChart: stockChart
                        })
                    );

                    // Set main series
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/stock-chart/#Setting_main_series
                    valueLegend.data.setAll([valueSeries]);



                    // Create volume axis
                    var volumeAxisRenderer = am5xy.AxisRendererY.new(root, {
                        inside: true
                    });

                    volumeAxisRenderer.labels.template.set("forceHidden", true);
                    volumeAxisRenderer.grid.template.set("forceHidden", true);

                    var volumeValueAxis = mainPanel.yAxes.push(am5xy.ValueAxis.new(root, {
                        numberFormat: "#.#a",
                        height: am5.percent(20),
                        y: am5.percent(100),
                        centerY: am5.percent(100),
                        renderer: volumeAxisRenderer
                    }));

                    // Add series
                    var volumeSeries = mainPanel.series.push(am5xy.ColumnSeries.new(root, {
                        name: "Volume",
                        clustered: false,
                        valueXField: "Date",
                        valueYField: "Volume",
                        xAxis: dateAxis,
                        yAxis: volumeValueAxis,
                        legendValueText: "[bold]{valueY.formatNumber('#,###.0a')}[/]"
                    }));

                    volumeSeries.columns.template.setAll({
                        strokeOpacity: 0,
                        fillOpacity: 0.5
                    });

                    // color columns by stock rules
                    volumeSeries.columns.template.adapters.add("fill", function (fill, target) {
                        var dataItem = target.dataItem;
                        if (dataItem) {
                            return stockChart.getVolumeColor(dataItem);
                        }
                        return fill;
                    })


                    // Set main series
                    stockChart.set("volumeSeries", volumeSeries);
                    valueLegend.data.setAll([valueSeries, volumeSeries]);


                    // Add cursor(s)
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/xy-chart/cursor/
                    mainPanel.set(
                        "cursor",
                        am5xy.XYCursor.new(root, {
                            yAxis: valueAxis,
                            xAxis: dateAxis,
                            snapToSeries: [valueSeries],
                            snapToSeriesBy: "y!"
                        })
                    );

                    // Add scrollbar
                    // -------------------------------------------------------------------------------
                    // https://www.amcharts.com/docs/v5/charts/xy-chart/scrollbars/
                    var scrollbar = mainPanel.set(
                        "scrollbarX",
                        am5xy.XYChartScrollbar.new(root, {
                            orientation: "horizontal",
                            height: 50
                        })
                    );
                    stockChart.toolsContainer.children.push(scrollbar);

                    var sbDateAxis = scrollbar.chart.xAxes.push(
                        am5xy.GaplessDateAxis.new(root, {
                            baseInterval: {
                                timeUnit: "hour",
                                count: 4
                            },
                            renderer: am5xy.AxisRendererX.new(root, {})
                        })
                    );

                    var sbValueAxis = scrollbar.chart.yAxes.push(
                        am5xy.ValueAxis.new(root, {
                            renderer: am5xy.AxisRendererY.new(root, {})
                        })
                    );

                    var sbSeries = scrollbar.chart.series.push(
                        am5xy.LineSeries.new(root, {
                            valueYField: "Close",
                            valueXField: "Date",
                            xAxis: sbDateAxis,
                            yAxis: sbValueAxis
                        })
                    );

                    sbSeries.fills.template.setAll({
                        visible: true,
                        fillOpacity: 0.3
                    });

                    // Set up series type switcher
                    var seriesSwitcher = am5stock.SeriesTypeControl.new(root, {
                        stockChart: stockChart
                    });

                    seriesSwitcher.events.on("selected", function (ev) {
                        setSeriesType(ev.item.id);
                    });

                    function getNewSettings(series) {
                        var newSettings = [];
                        am5.array.each(["name", "valueYField", "highValueYField", "lowValueYField", "openValueYField", "calculateAggregates", "valueXField", "xAxis", "yAxis", "legendValueText", "stroke", "fill"], function (setting) {
                            newSettings[setting] = series.get(setting);

                        });
                        return newSettings;
                    }
                    sbSeries.data.setAll(result);
                    function setSeriesType(seriesType) {
                        // Get current series and its settings
                        var currentSeries = stockChart.get("stockSeries");
                        var newSettings = getNewSettings(currentSeries);

                        // Remove previous series
                        var data = currentSeries.data.values;
                        mainPanel.series.removeValue(currentSeries);

                        // Create new series
                        var series;
                        switch (seriesType) {
                            case "line":
                                series = mainPanel.series.push(am5xy.LineSeries.new(root, newSettings));
                                break;
                            case "candlestick":
                            case "procandlestick":
                                newSettings.clustered = false;
                                series = mainPanel.series.push(am5xy.CandlestickSeries.new(root, newSettings));
                                if (seriesType == "procandlestick") {
                                    series.columns.template.get("themeTags").push("pro");
                                }
                                break;
                            case "ohlc":
                                newSettings.clustered = false;
                                series = mainPanel.series.push(am5xy.OHLCSeries.new(root, newSettings));
                                break;
                        }

                        // Set new series as stockSeries
                        if (series) {

                            valueLegend.data.removeValue(currentSeries);
                            series.data.setAll(data);

                            stockChart.set("stockSeries", series);

                            var cursor = mainPanel.get("cursor");
                            if (cursor) {
                                cursor.set("snapToSeries", [series]);

                            }
                            valueLegend.data.insertIndex(0, series);

                        }
                    }


                    // Stock toolbar
                    var toolbar = am5stock.StockToolbar.new(root, {
                        container: document.getElementById("chartcontrols"),
                        stockChart: stockChart,
                        controls: [

                            seriesSwitcher,
                            am5stock.DrawingControl.new(root, {
                                stockChart: stockChart
                            }),

                        ]
                    })

                    // set data to all series
                    valueSeries.data.setAll(result);
                    sbSeries.data.setAll(result);
                    volumeSeries.data.setAll(result);

                    dynamicData = setInterval(function () {
                        let lastDataObject = valueSeries.data.getIndex(valueSeries.data.length - 1);
                        if (lastDataObject != undefined) {
                            var selectedParity = $('#paritySelect').val();
                            var valueSelected = $('#Detail_CexExchange').val();

                            if (selectedParity != "0" && valueSelected != "0") {
                                $.ajax({
                                    url: "/Workers/GetChartDate",
                                    type: "POST",
                                    dataType: 'json',
                                    data: {
                                        "CexExchange": valueSelected,
                                        "symbol": selectedParity
                                    },
                                    success: function (resultData) {
                                        if (resultData.length < 1) {

                                        } else {

                                            var index = resultData.length - 1;
                                            if (index >= 0) {
                                                valueSeries.data.setIndex(index, {
                                                    Close: resultData[0].Close,
                                                    Date: resultData[0].Date,
                                                    High: resultData[0].High,
                                                    Low: resultData[0].Low,
                                                    Open: resultData[0].Open,
                                                    Volume: resultData[0].Volume
                                                });
                                                volumeSeries.data.setIndex(index, {
                                                    Close: resultData[0].Close,
                                                    Date: resultData[0].Date,
                                                    High: resultData[0].High,
                                                    Low: resultData[0].Low,
                                                    Open: resultData[0].Open,
                                                    Volume: resultData[0].Volume
                                                });
                                                sbSeries.data.setIndex(index, {
                                                    Close: resultData[0].Close,
                                                    Date: resultData[0].Date,
                                                    High: resultData[0].High,
                                                    Low: resultData[0].Low,
                                                    Open: resultData[0].Open,
                                                    Volume: resultData[0].Volume
                                                });


                                            }
                                            // update current value
                                            if (currentLabel) {

                                                currentValueDataItem.animate({ key: "value", to: resultData[0].Close, duration: 500, easing: am5.ease.out(am5.ease.cubic) });
                                                currentLabel.set("text", stockChart.getNumberFormatter().format(resultData[0].Close));
                                                var bg = currentLabel.get("background");
                                                if (bg) {



                                                    if (lastDataObject.Close > resultData[0].Close) {
                                                        bg.set("fill", root.interfaceColors.get("negative"));
                                                    }
                                                    else {
                                                        bg.set("fill", root.interfaceColors.get("positive"));
                                                    }
                                                }
                                            }
                                        }
                                    }

                                });
                            }
                        }
                    }, 1000);


                }); // end am5.ready()


            },
        });
    }


}