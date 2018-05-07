/********************************************  
*   SkuAttrer 产品属性集合操作方法 
********************************************/
function SkuAttrer(divId, objName, attrId, attrName, attrValue, attrType, callback) {
    this.DivId = divId;
    this.ObjName = objName;
    this.AttrId = attrId ? attrId : "attrId";
    this.AttrName = attrName ? attrName : "attrName";
    this.AttrValue = attrValue ? attrValue : "attrValue";
    this.AttrType = attrType ? attrType : "attrType";
    this.CallBack = callback;
    this.MyAttrId = "my" + this.AttrId;
    this.MyAttrName = "my" + this.AttrName;
    this.MyAttrValue = "my" + this.AttrValue;
    this.MyAttrType = "my" + this.AttrType;
}

//属性初始化
SkuAttrer.prototype.Init = function (classId) {
    var divId = this.DivId;
    var objName = this.ObjName;
    var attrId = this.AttrId;
    var attrName = this.AttrName;
    var attrValue = this.AttrValue;
    var attrType = this.AttrType;
    var callback = this.CallBack;

    $("#" + divId).html("");
    $.ajax({
        type: "post",
        url: "/api/product/getproattr/" + classId,
        dataType: "json",
        success: function (result) {
            if (classId > 0) {
                if (result && result.state == 0) { ShowError(result.error); return false; }
                $.each(result.attr, function (i, item) {
                    var html = "";
                    //非空验证class
                    var classStr = item.isrequired ? "class='form-control' required='required'" : "class='form-control'";
                    html += "<tr><th align='right' width='150px'>" + item.name;
                    html += "<input type='hidden' name='" + attrId + "' value='" + item.id + "'/>";
                    html += "<input type='hidden' name='" + attrName + "" + item.id + "' value='" + item.name + "'/>";
                    html += "<input type='hidden' name='" + attrType + "" + item.id + "' value='" + item.inputtype + "'/>";
                    html += "<input type='hidden' name='issort" + item.id + "' value='" + item.issort + "'/>";
                    html += "</th>";
                    html += "<td width='750px'><div class='form-inline'>";
                    if (item.inputtype == 1) { //文本框
                        html += "<input type='text' name='" + attrValue + item.id + "' style='width:150px' id='" + attrValue + item.id + "' value='" + item.value + "' " + classStr + "/>";
                    }
                    else if (item.inputtype == 2) {   //下拉框
                        if (item.attrlist != null && item.attrlist != 'undefined') {
                            html += "<select name='" + attrValue + + item.id + "' onchange='" + objName + ".SelectChange(this);return false;'  " + classStr + " style=\"width:200px\">";
                            html += "<option value=''>请选择选项值(默认为空)</option>";
                            $.each(item.attrlist, function (i, item2) {
                                html += "<option value='" + item2.id + "'>" + item2.value + "</option>";
                            })
                            html += "</select>";
                            html += "<span style='display:none;width:150px;' class='layui-word-aux'><input type='text' name='" + attrValue + "Other" + item.id + "' value='' class='form-control' style=\"width:150px\"/></span>";
                        }
                    }
                    else if (item.inputtype == 3) {   //多选框
                        if (item.attrlist != null && item.attrlist != 'undefined') {
                            $.each(item.attrlist, function (i, item2) {
                                if (item2.id > 0) {
                                    html += "<input type='checkbox' name='" + attrValue + item.id + "' value='" + item2.id + "'  onchange='" + objName + ".SelectChange(this);return false;' " + classStr + " />" + item2.value + "";
                                }
                            })
                        }
                    }
                    if (item.issort)
                        html += "<span class='layui-word-aux'>[可筛选属性]</span>";
                    html + "</div></td></tr>"
                    //html += " <div style='float: right'><input type='button' value='↑' onclick='upMove(this)' /> <input type='button' value='↓' onclick='downMove(this)' /></div></td></tr>";
                    $("#" + divId).append(html);
                    //初始化验证
                    $("#" + divId).parents("form").eq(0).validate();

                });
            }
            $("#" + divId).append("<tr><th width='150px'></th><td><a href='javascript:void(0)' class='btn btn-default' onclick='" + objName + ".AddSku(this)'><i class=\"fa fa-plus\"></i> 添加</a></td></tr>");
            if (callback) callback();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowError(errorThrown);
        }
    });
}

//添加属性
SkuAttrer.prototype.AddSku = function (obj) {
    var objName = this.ObjName;
    var maxId = parseInt($("input[name='" + this.MyAttrId + "']").last().val()) + 1 || 0;
    $(obj).parents("tr").eq(0).before("<tr><th align='right' width='150px'><input type='hidden' name='" + this.MyAttrId + "' value='" + maxId + "'><input type='text' style='width:150px' name='" + this.MyAttrName + maxId + "' value='' class='form-control' required='required'></th><td><div class='form-inline'><input type='text' style='width:150px' name='" + this.MyAttrValue + maxId + "' value='' class='form-control' required='required'> <input type=\"button\" value=\"↑\" class=\"btn btn-default\" onclick='" + objName + ".Up(this)'> <input type=\"button\" value=\"↓\" class=\"btn btn-default\" onclick='" + objName + ".Down(this)'> <a href='javascript:void(0)' onclick='" + objName + ".DelSku(this)' class='btn btn-danger btn-sm'>删 除</a></div></td></tr>");
}

//删除属性
SkuAttrer.prototype.DelSku = function (obj) {
    $(obj).parents("tr").eq(0).remove();
}

//下拉框监听
SkuAttrer.prototype.SelectChange = function (obj) {
    var selVal = $(obj).find("option:selected").val() || $(obj).val()
    if (selVal == "0")
        $(obj).parent().parent().find('span:not(.gray)').show();
    else
        $(obj).parent().parent().find('span:not(.gray)').hide();
}

//排序上移
SkuAttrer.prototype.Up = function (obj) {
    var objParentTR = $(obj).parents("tr").eq(0);
    var prevTR = objParentTR.prev();
    if (prevTR.length > 0 && prevTR.find("th > input[type='text']").length > 0) {
        prevTR.insertAfter(objParentTR);
    }
}

//排序下移
SkuAttrer.prototype.Down = function (obj) {
    var objParentTR = $(obj).parents("tr").eq(0);
    var nextTR = objParentTR.next();
    if (nextTR.length > 0 && nextTR.find(".form-inline").length > 0) {
        nextTR.insertBefore(objParentTR);
    }
}

//分类下拉框监视事件
SkuAttrer.prototype.BindSelect = function (selectId, selectTipId) {
    var obj = this;
    $("#" + selectId).change(function () {
        var $this = $(this);
        var $thisSelect = $this.find("option:selected");
        var value = $thisSelect.val();
        var childNum = $thisSelect.attr("child");
        var loadattr = $this.attr("loadattr"); //是否加载属性

        if (value == '' || value == 0) {
            if (loadattr && loadattr == 'true') obj.Init(0);
        } else {
            if (childNum > 0) {
                $("#" + selectTipId).html("请选择最底层的分类");
                $("[type='submit']").prop("disabled", true);
                if (loadattr && loadattr == 'true') obj.Init(0);
            } else {
                if (loadattr && loadattr == 'true') obj.Init(value);
                $("#" + selectTipId).html($thisSelect.attr("attr"));
                $("[type='submit']").prop("disabled", false);
            }
        }
    });
}

//加载分类下拉框
function InitClassSelect(domId, parId, selectvalue) {
    $.ajax({
        type: "get",
        url: "/api/product/ShowClass",
        dataType: "json",
        data: { parId: parId, selectId: selectvalue },
        success: function (data) {
            if (data != "") {
                $("#" + domId).append(data);
            }
        }
    });
}
