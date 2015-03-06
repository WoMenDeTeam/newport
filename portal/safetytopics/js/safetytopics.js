/// <reference path="jquery-1.8.1.js" />
$(document).ready(function () {
    SafetyTopics.Init();
});
var SafetyTopics = {
    yuqingdata: Object,
    Init: function () {/// <reference path="" />
        $("#yqfx_tb ul li a").unbind().bind("click", function () {
            $("#yqfx_tb ul li a:visible").removeAttr("style");
            $(this).attr("style", "background-image: url(images/a2.png);color: #FF0000");
            var tbIndex = this.id.split('_')[2];
            $(".content2_top[group='chart']").hide();
            $(".content2_bottom[group=chanr_info]").hide();
            $("#yqfx_char_" + tbIndex).show();
            $("#yqfx_info_" + tbIndex).show();
        });

        $("#top_list_tb ul li a").unbind().bind("click", function () {
            $("#top_list_tb ul li a").removeAttr("style");
            $(this).attr("style", "background-image: url(images/a2.png);color: #FF0000");
            var tbIndex = this.id.split('_')[2];
            $(".content3_bottom_nr[group='topinfo']").hide();
            $("#top_info_" + tbIndex).show();
        });
    },
    getYuQingJson: function (callfun) {
        var t = new Date().getTime();
        $.getJSON("../jsondata/safetydata.json?v=" + t, function (data) {
            $("#tb_li ul").empty();
            $.each(data, function (index, content, i) {
                var _li = $("<li><a href=\"javascript:void(0);\">" + content.title + "</a></li>");
                var $li = $(_li).unbind("click").bind("click", function () {
                    alert(content.title);
                    SafetyTopics.chart(content.data);
                    SafetyTopics.listChartInfo(content.context);
                });
                $("#tb_li ul").append($li);
            });
        });
    },
    loadChart: function (item) {
    },
    listChartInfo: function (data) {
        $("#yuqinginfo_div ul").empty();
        $.each(data, function (index, comment) {
            var $li = $("<li><a href=" + comment.url + ">" + comment.title + "</a></li>");
            $("#yuqinginfo_div ul").append($li);
        });
    },
    chart: function (seriesdata) {

        var follwChart = new Highcharts.Chart({
            chart: {
                renderTo: 'container',
                type: 'spline',
                width: 833,
                height: 279
            },
            title: {
                text: ''
            },
            legend: { /*是否显示*/
                enabled: false
            },
            xAxis: {
                categories: ["周日", "周一", "周二", "周三", "周四", "周五", "周六"]
            },
            yAxis: {
                title: {
                    text: ''
                }
            },
            tooltip: {
                crosshairs: true,
                shared: true
            },
            plotOptions: {
                spline: {
                    marker: {
                        radius: 4,
                        lineColor: '#666666',
                        lineWidth: 1
                    }
                }
            },
            series: [{
                marker: {
                    symbol: 'square'
                },
                data: seriesdata

            }]
        });

    }
}