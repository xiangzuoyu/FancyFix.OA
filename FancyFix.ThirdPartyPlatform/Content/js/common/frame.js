/*  
**  顶层框架功能初始化类  
**  Auth:Harry
**  Date:2017-04-15
**  Depenency: LayUI,jQuery
*/

//===============Tab加载===============//
layui.use(['element', 'laytpl', 'layer'], function () {
    var laytpl = layui.laytpl;
    var element = layui.element();
    var layer = layui.layer;

    //加载菜单
    var $tpl = $("#menu-tpl");
    var menuId = $tpl.data("menuid");
    var url = $tpl.data("url");
    $.ajax({
        type: "get",
        url: url,
        dataType: "json",
        success: callback
    });

    function callback(data) {
        if (data.Status) {
            //加载模版
            laytpl($tpl.html()).render(data.List, function (render) {
                document.getElementById(menuId).innerHTML = render;
                //SetMenuScrollHeight();
            });
            //选中菜单
            var location = window.location.pathname.toLowerCase();
            var select = $("#main-menu a[data-url^='" + location + "']");
            if (select && location != "/") {
                select.parent("li").addClass("active");
                select.parents(".treeview-menu").addClass("menu-open").css("display", "block");
                select.parents(".treeview").addClass("active");
            }
            //菜单选中事件
            $("#main-menu").on('click', 'li', function () {
                var $this = $(this);
                //判断是否还有子级
                if ($this.find("ul").size() == 0) {
                    layer.load(1);
                    var id = $this.find("a").data("id");
                    var url = $this.find("a").data("url");
                    var title = $this.find("a").data("title");
                    window.location.href = url;
                    //$("#iframepage").attr("src", url).data("id", id);
                    //判断是否存在tab
                    //if ($("ul.layui-tab-title li[lay-id='" + id + "']").size() == 0) {
                    //    var content = "<iframe id='" + id + "' frameborder=\"0\" width=\"100%\" height=\"667px\" src=\"" + url + "\"></iframe>";
                    //    element.tabAdd('tabs', {
                    //        title: title,
                    //        content: content,
                    //        id: id
                    //    });
                    //}
                    //选中tab
                    //element.tabChange('tabs', id);
                }
            });
        } else {
            layer.open({
                title: '错误',
                content: data.Remark
            });
        }
    }

    //iframe高度自适应
    //function SetFrameHeight() {
    //    var id = $("div.layui-show iframe").attr("id");
    //    setTimeout(function () {
    //        var ifm = document.getElementById(id);
    //        if (ifm) {
    //            ifm.height = document.documentElement.clientHeight - ifm.offsetTop;
    //        }
    //    }, 100);
    //}
    //左侧菜单滚动
    //function SetMenuScrollHeight() {
    //    $("#main-menu").slimScroll({
    //        height: document.documentElement.clientHeight - (document.documentElement.clientWidth > 750 ? 135 : 185),
    //        alwaysVisible: true,
    //    });
    //}
    //SetFrameHeight();

    //$(window).resize(function () {
    //    SetFrameHeight();
    //    SetMenuScrollHeight();
    //});

    //设置过期处理
    $.ajaxSetup({
        complete: function (xMLHttpRequest, textStatus) {
            var header = xMLHttpRequest.getResponseHeader('Timeout-Head');
            if (header != undefined && header == "timeout") {
                top.location.href = '/auth/login';
            }
        },
        cache: false
    })

    //tab切换监听事件
    //element.on('tab(tabs)', function (data) {
    //    SetFrameHeight();
    //});

    //刷新按钮
    $("#btnRepeat").on('click', function () {
        window.location.reload();
        //var $ifm = $(".layui-show iframe").eq(0);
        //$ifm.attr("src", $ifm.attr("src"));
        //layer.load(1);
    });

    //Tab拖动排序
    //var list = document.getElementsByClassName("layui-tab-title")[0];
    //Sortable.create(list, { group: "li" });
});

//添加Tab
//function addTab(title, url) {
//    var element = layui.element();
//    var id = url;
//    if ($("ul.layui-tab-title li[lay-id='" + id + "']").size() == 0) {
//        var content = "<iframe id='" + id + "' frameborder=\"0\" width=\"100%\" height=\"667px\" src=\"" + url + "\"></iframe>";

//        element.tabAdd('tabs', {
//            title: title,
//            content: content,
//            id: id
//        });
//    }
//    element.tabChange('tabs', id);
//}
//关闭Tab
//function closeTab(layid) {
//    var element = layui.element();
//    element.tabDelete('tabs', layid)
//}

