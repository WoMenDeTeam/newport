/// <reference path="jquery.js" />
/// <reference path="Pager.js" />
/// <reference path="common.js" />

$(document).ready(function () {
    $(".header").attr("style", "background:url(img/header_bg3.jpg) no-repeat center top;");
    Common.LoginEventFn = function (data) {
        var type = $.query.get('type');
        if (type) {
            $("#report_type").val(type);
        }
        AccidentReportList.LSqlList = new SqlList();
        AccidentReportList.Init();
        AccidentReportList.InnitClickFn();
        AccidentReportList.BindOrderFilde();
    }
    Common.CheckUser();


    //    AccidentReportList.LSqlList = new SqlList();
    //    AccidentReportList.Init();
    //    AccidentReportList.InnitClickFn();
    //    AccidentReportList.BindOrderFilde();
    //    Common.InnitCopyright();
});

var AccidentReportList = {
    LSqlList: new SqlList(),
    CityList: ["北京", "天津", "河北", "山西", "内蒙古", "辽宁", "吉林", "黑龙江", "上海", "江苏", "浙江", "安徽", "福建", "江西", "山东", "河南", "湖北", "湖南", "广东", "广西", "海南", "重庆", "四川", "贵州", "云南", "西藏", "陕西", "甘肃", "青海", "宁夏", "新疆", "大连", "宁波", "厦门", "青岛", "深圳", "新疆建设兵团", "所有省份", ""],
    ExportDataId: [],
    Init: function () {
        var defaultTime = new Date().getFullYear() + "-01-01";
        AccidentReportList.LSqlList.SqlQueryParams["strwhere"] = " State =-1 AND PublishTime >= '" + defaultTime + "'";
        AccidentReportList.LSqlList.SqlQueryParams["action"] = "accidentreportlist";
        AccidentReportList.LSqlList.SqlQueryParams["strorder"] = " PublishTime DESC ";
        AccidentReportList.GetPageList();

        $("#dy_list ul li").remove();
        for (var i = 0, j = AccidentReportList.CityList.length; i < j; i++) {
            var cityname = AccidentReportList.CityList[i];
            var li_width = "";
            if (cityname == "新疆建设兵团") {
                li_width = "width:121px;";
            }
            _li = "<li style=\"" + li_width + "\" v='" + cityname + "'><a href=\"javascript:void(null);\" v=\"" + cityname + "\">" + cityname + "</a></li>";
            $("#dy_list ul").append(_li);
        }
        $("#start_time").val(defaultTime);

        var fadeoutTime = 400;

        $("#search_time_txt").focusin(function () {
            $("#sjlx_list,#subform_mark_layer").show();
        }).blur(function () {
            $("#sjlx_list").fadeOut(fadeoutTime);
            $("#subform_mark_layer").hide();
        });

        $("#search_jgjc_txt").focusin(function () {
            $("#jgbm_list,#subform_mark_layer").show();
        }).blur(function () {
            $("#jgbm_list").fadeOut(fadeoutTime);
            $("#subform_mark_layer").hide();
        });

        $("#search_sglx_txt").focusin(function () {
            $("#sglx_list,#subform_mark_layer").show();
        }).blur(function () {
            $("#sglx_list").fadeOut(fadeoutTime);
            $("#subform_mark_layer").hide();
        });

        $("#search_dy_txt").focusin(function () {
            $("#dy_list,#subform_mark_layer").show();
        }).blur(function () {
            $("#dy_list").fadeOut(fadeoutTime);
            $("#subform_mark_layer").hide();
        });

        $("#jgbm_list a").unbind().bind("click", function () {
            $("#search_jgjc_txt").val($(this).attr("v"));
            AccidentReportList.InnitClickFn();
        });
        $("#sglx_list a").unbind().bind("click", function () {
            $("#search_sglx_txt").val($(this).attr("v"));
            AccidentReportList.InnitClickFn();
        });
        $("#dy_list a").unbind().bind("click", function () {
            $("#search_dy_txt").val($(this).attr("v"));
            AccidentReportList.InnitClickFn();
        });
        $("#dy_list li").unbind().bind("click", function () {
            $("#search_dy_txt").val($(this).attr("v"));
            AccidentReportList.InnitClickFn();
        });

        $("#sjlx_list a").unbind().bind("click", function () {
            var type_txt = $(this).attr("v");
            $("#search_time_txt").val(type_txt);
            AccidentReportList.InnitClickFn();
        });

        //导出excel报告
        $("#export_btn").unbind().bind("click", AccidentReportList.ExportExcel);
        $("#ck_list").change(function () {
            var ischecked = $(this).attr("checked");
            $("#SearchResult [name = chkItem]:checkbox").attr("checked", ischecked);
            $("#SearchResult [name = chkItem]:checkbox").each(function (a, b) {
                var ischk = $(this).attr("checked");
                var _value = $(this).attr("value");
                AccidentReportList.OperatingArray(ischk, _value);
            });
        });

    }, GetPageList: function () {
        AccidentReportList.LSqlList.initData["page_size"] = 30;
        AccidentReportList.LSqlList.DisplayHtml = function (data, l_obj) {
            AccidentReportList.DisplayHtml(data, l_obj);
        }
        AccidentReportList.LSqlList.Search();
    },
    ChangeTime: function () {
        AccidentReportList.InnitClickFn();
    },
    DisplayHtml: function (data, l_obj) {
        if (parseInt(data["totalcount"]) > 0) {
            var entitylist = data["entitylist"];
            delete entitylist["SuccessCode"];
            var content = [];
            for (var item in entitylist) {

                var entity = entitylist[item];
                var id = entity["id"];
                var title = unescape(entity["title"]);
                var url = unescape(entity["url"]);
                var occurrencetime = unescape(entity["occurrencetime"]);
                var publishtime = unescape(entity["publishtime"]);

                var department = unescape(entity["department"]);
                var regulatorydepartment = unescape(entity["regulatorydepartment"]);
                var accidentlevel = unescape(entity["accidentlevel"]);
                var area = unescape(entity["area"]);
                var createtime = unescape(entity["createtime"]);

                regulatorydepartment = regulatorydepartment == "" ? "- -" : regulatorydepartment;
                accidentlevel = accidentlevel == "" ? "- -" : accidentlevel;
                area = area == "" ? "- -" : area;

                if (title == "undefined") {
                    continue;
                }
                content.push("<li>");
                content.push("<span class=\"input\">");
                content.push("<input name='chkItem'  type=\"checkbox\" value=\"" + id + "\" />");
                content.push("</span>");
                content.push("<a title=\"" + title + "\" href=\"" + url + "\" target=\"_blank\">");
                content.push(title + "</a>");
                content.push(" <span class=\"text\">" + department);
                content.push("</span>");
                content.push("<span class=\"date\">发生日期:" + occurrencetime.split(' ')[0] + " 发布日期:" + publishtime.split(' ')[0] + "</span>");
                content.push("</li>");

            }

            $("#" + l_obj.result_id).empty().html(content.join(""));
            $("#" + l_obj.result_id).find("input[type='checkbox']").unbind().change(function () {
                var isChecked = $(this).attr("checked");
                var id = $(this).attr("value");
                AccidentReportList.OperatingArray(isChecked, id);
            });
            AccidentReportList.ReductionCheckbox();
        } else {
            $("#" + l_obj.result_id).empty().html("<li><center>对不起，没有数据。</center></li>");
        }
    },
    InnitClickFn: function () {

        var strwhere = [];
        var starttime = $("#start_time").val();
        var endtime = $("#end_time").val();
        var jgjc = $("#search_jgjc_txt").val();
        var sgjb = $("#search_sglx_txt").val();
        var area = $("#search_dy_txt").val();
        //var timetype = "发生日期"; // $("#search_time_txt").val();
        var timetype = $("#search_time_txt").val();
        if (timetype == "发布日期") {
            filed = " PublishTime ";
        } else if (timetype == "发生日期") {
            filed = " OccurrenceTime ";
        }

        strwhere.push(" State =-1 AND");
        if (starttime && timetype != "所有时间") {
            strwhere.push(filed + " >='" + starttime + "' AND");
        }
        if (endtime && timetype != "所有时间") {
            strwhere.push(filed + " <='" + endtime + "' AND");
        }
        if (jgjc != "所有部门") {
            strwhere.push(" RegulatoryDepartment like '%" + jgjc + "%' AND");
        }
        if (sgjb != "所有事故") {
            strwhere.push(" AccidentLevel = '" + sgjb + "' AND");
        }
        if (area != "所有省份") {
            strwhere.push(" Area = '" + area + "' AND");
        }
        AccidentReportList.LSqlList.SqlQueryParams["strwhere"] = strwhere.join("").slice(0, -3);
        AccidentReportList.GetPageList();

    },
    BindOrderFilde: function () {
        $("#orderTabUl li[name='sort_search_type']").unbind().bind("click", function () {
            var orderbyFile = $(this).attr("pid");
            $("#orderTabUl li[name='sort_search_type']").removeClass().addClass("tab_off");
            $(this).addClass("tab_on");
            AccidentReportList.LSqlList.SqlQueryParams["strorder"] = orderbyFile; //+ " DESC ";
            AccidentReportList.GetPageList();
        });
    }, ExportExcel: function () {
        var stime = $("#start_time").val();
        var etime = $("#end_time").val();
        var area = $("#search_dy_txt").val();
        if (area == "所有省份") {
            area = "全国";
        }
        if (AccidentReportList.ExportDataId.length > 0) {
            $("#export_btn").html("生成中...");
            var data = { "action": "getreport", "ids": AccidentReportList.ExportDataId.join(","), "stime": stime, "etime": etime, "area": area };
            $.getJSON("Handler/ReoptrToExcel.ashx", data, function (response) {
                if (response["success"] === "1") {
                    location.href = "reportfile/" + response["backurl"];

                } else {
                    alert("当前服务器忙请稍后在试");
                }
                $("#export_btn").html("全部导出");
            });
        }
    }, OperatingArray: function (ischk, value) {
        if (ischk) {
            AccidentReportList.AddExportDataID(value)
        } else {
            AccidentReportList.DeleteExportDataID(value);
        }
    },
    AddExportDataID: function (value) {
        var isExist = true;
        for (var i = 0, j = AccidentReportList.ExportDataId.length; i < j; i++) {
            if (AccidentReportList.ExportDataId[i] == value) {
                isExist = false;
            }
        }
        if (isExist) {
            AccidentReportList.ExportDataId.push(value);
        }
        AccidentReportList.RefreshCheckedNumber();
    }, DeleteExportDataID: function (value) {
        for (var i = 0, j = AccidentReportList.ExportDataId.length; i < j; i++) {
            if (AccidentReportList.ExportDataId[i] == value) {
                AccidentReportList.ExportDataId.splice(i, 1);
                i = j;
            }
        }
        AccidentReportList.RefreshCheckedNumber();
    }, RefreshCheckedNumber: function () {
        $("#checknumber").html(AccidentReportList.ExportDataId.length);
    }, ReductionCheckbox: function () {
        $("#SearchResult [name = chkItem]:checkbox").each(function (a, b) {
            var value = $(this).attr("value");
            for (var i = 0, j = AccidentReportList.ExportDataId.length; i < j; i++) {
                if (AccidentReportList.ExportDataId[i] == value) {
                    var value = $(this).attr("checked", true);
                    i = j;
                }
            }
        });
    }
}