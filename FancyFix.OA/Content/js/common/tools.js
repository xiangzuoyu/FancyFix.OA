/*  
**  框架公共工具类  
**  Auth:Harry
**  Date:2017-04-15
**  Depenency: LayUI,jQuery,Bootstrap Datatable
*/

//===============Bootstrap Datatable===============//
var $table = $("#table");
//重新加载
function Reload() {
    $table.bootstrapTable('refresh');
}
function ReloadPage() {
    setTimeout(function () {
        window.location.reload();
    }, 1000);
}
//获取勾选的记录
function GetSelections() {
    return $table.bootstrapTable('getSelections');
}
//验证多选
function CheckSelections() {
    var array = GetSelections();
    if (array.length == 0) {
        ShowError('请至少选择一行数据');
        return false;
    }
    return true;
}

//隐藏列
function HideColumns(array, minwidth) {
    if (typeof (minwidth) == 'undefined' || minwidth <= 0) {
        minwidth = 768;
    }
    if (typeof (array) != 'undefined' && array.length > 0) {
        if (document.body.clientWidth <= minwidth) {
            for (var i in array) {
                $table.bootstrapTable('hideColumn', array[i]);
            }
        }
        $(window).resize(function () {
            if (document.body.clientWidth <= minwidth) {
                for (var i in array) {
                    $table.bootstrapTable('hideColumn', array[i]);
                }
            } else {
                for (var i in array) {
                    $table.bootstrapTable('showColumn', array[i]);
                }
            }
        });
    }
}
//验证单选
function CheckSelectionsSingle() {
    var array = GetSelections();
    if (array.length == 0) {
        ShowError('请至少选择一行数据');
        return false;
    }
    if (array.length != 1) {
        ShowError('最多选择一行数据');
        return false;
    }
    return true;
}

//===============Layer消息===============//
//提示消息
function ShowMsg(msg) {
    try {
        layer.msg(msg);
    } catch (ex) {
        alert(msg);
    }
}
//提示成功消息
function ShowSuccess(msg) {
    try {
        layer.msg(msg, { icon: 1 });
    } catch (ex) {
        alert(msg);
    }
}
//提示错误消息
function ShowError(msg) {
    try {
        layer.msg(msg, { icon: 5 });
    } catch (ex) {
        alert(msg);
    }
}

//===============LayUI选项卡===============//

//打开一个tab
function OpenTab(title, url) {
    try {
        window.parent.addTab(title, url);
    } catch (ex) {
        console.log(ex);
    }
}
//关闭当前tab
function CloseTab() {
    var layid = parent.$("li.layui-this").attr("lay-id");
    CloseTabById(layid);
}
//关闭指定tab
function CloseTabById(layid) {
    try {
        window.parent.closeTab(layid);
    } catch (ex) {
        console.log(ex);
    }
}

//===============Layer弹窗===============//
//打开一个窗口,自定义长宽，可设百分比
function OpenWin(title, url, width, height) {
    try {
        layer.open({
            title: title,
            type: 2,
            content: url,
            area: document.body.clientWidth > 450 ? [width, height] : [window.screen.width + 'px', (window.screen.height - 150) + 'px'],
        });
    } catch (ex) {
        alert('弹出窗口失败！')
    }
}
//打开一个窗口-最大化
function OpenWinMax(title, url) {
    try {
        var win = layer.open({
            title: title,
            type: 2,
            content: url,
            maxmin: true
        });
        layer.full(win);
    } catch (ex) {
        alert('弹出窗口失败！')
    }
}
//关闭当前窗口
function CloseWin() {
    var index = parent.layer.getFrameIndex(window.name);
    CloseWinByIndex(index);
}
//关闭指定窗口
function CloseWinByIndex(index) {
    try {
        parent.layer.close(index);
    } catch (ex) {
        console.log(ex);
    }
}





