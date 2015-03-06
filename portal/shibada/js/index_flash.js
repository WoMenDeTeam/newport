<!--
var c = 0 //当前第几个
var n  //图片张数
var wid // 重复序列
function autoPlay() // 自动播放函数
{
        c++ //指向下张图  
		if(c>= n) c = 0   //循环回去
		$($("#num li")[c]).addClass("current")  //设置当前
		$($("#num li")[c]).siblings().removeClass("current")
		$($("#scrollArea li")[c]).fadeTo(300,1)
		$($("#scrollArea li")[c]).siblings().hide()
		$($("#focus h2")[c]).show()
		$($("#focus h2")[c]).siblings("h2").hide()
			
}
$(function(){
    n = $("#num li").size() //图片张数
	$("#num li:first").addClass("current")
	$("#scrollArea li:first").show()
	$("#focus h2:first").show()
    $("#num li").css("right",function(i){ return (n -i) * 18 })
	
	$("#num li").hover(
	function(){
	    window.clearInterval(wid)   //停止序列
	    $(this).addClass("current")
		$(this).siblings().removeClass("current")
		c = $(this).index()
		$($("#scrollArea li")[c]).fadeTo(300,1)
		$($("#scrollArea li")[c]).siblings().hide()
		$($("#focus h2")[c]).show()
		$($("#focus h2")[c]).siblings("h2").hide()
	
	},
	function(){ wid = window.setInterval("autoPlay()",3000) } //鼠标拿开重新设置序列
	)
	
	wid = window.setInterval("autoPlay()",3000)  //启动自动播放函数
	
     
});
//-->