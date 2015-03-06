<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IOManage.aspx.cs" Inherits="IOManage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>首页</title>
    <script type="text/javascript" src="js/jquery.js"></script>
    <script type="text/javascript" src="js/ajaxfileupload.js"></script>
    <script type="text/javascript" src="js/IOManage.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    当前文件目录：<span id="file_path"></span>&nbsp;&nbsp;
    <a href="javascript:void(null)" id="look_back">后退</a><br />
    文件夹列表：&nbsp;&nbsp;<a href="javascript:void(null)" id="create_folder">创建文件夹</a><br />
    <div >
        <ul id="folder_list"></ul>
    </div>
    文件列表：&nbsp;&nbsp;<input type="file" id="file_name" name="file_name"/>
    <a href="javascript:void(null)" id="upload_file">上传文件</a><br />
    <div>
        <ul id="file_list"></ul>
    </div>
    </div>
    </form>
</body>
</html>