/********************************************  
*   SkuAttrer 属性集合操作方法 
********************************************/
function SkuAttrer(divId, objName, attrId, attrName, attrValue, attrType) {
    this.DivId = divId;
    this.ObjName = objName;
    this.AttrId = attrId ? attrId : "attrId";
    this.AttrName = attrName ? attrName : "attrName";
    this.AttrValue = attrValue ? attrValue : "attrValue";
    this.AttrType = attrType ? attrType : "attrType";
    this.MyAttrId = "my" + this.AttrId;
    this.MyAttrName = "my" + this.AttrName;
    this.MyAttrValue = "my" + this.AttrValue;
    this.MyAttrType = "my" + this.AttrType;
}

SkuAttrer.prototype.Init = function (classId) {
    var divId = this.DivId;
    var objName = this.ObjName;
    var attrId = this.AttrId;
    var attrName = this.AttrName;
    var attrValue = this.AttrValue;
    var attrType = this.AttrType;

    $("#" + divId).html("");
    $.ajax({
        type: "get",
        url: window.manageurl + "/api/product/getproattr/" + classId,
        dataType: "json",
        success: function (result) {
            if (classId > 0) {
                if (result && result.state == 0) { ShowError(result.error); return false; }
                $.each(result.attr, function (i, item) {
                    var html = "";
                    //非空验证class
                    var classStr = item.IsNeeded ? "class='form-control' required='required'" : "class='form-control'";
                    html += "<tr><th align='right' width='150px'>" + item.name;
                    html += "<input type='hidden' name='" + attrId + "' value='" + item.id + "'/>";
                    html += "<input type='hidden' name='" + attrName + "" + item.id + "' value='" + item.name + "'/>";
                    html += "<input type='hidden' name='" + attrType + "" + item.id + "' value='" + item.type + "'/>";
                    html += "<input type='hidden' name='isSearch" + item.id + "' value='" + item.issearch + "'/>";
                    html += "</th>";
                    html += "<td width='750px'><div class='form-inline'>";
                    if (item.type == 1) { //文本框
                        html += "<input type='text' name='" + attrValue + item.id + "' style='width:150px' id='" + attrValue + item.id + "' value='" + item.value + "' " + classStr + "/>";
                    }
                    else if (item.type == 2) {   //下拉框
                        if (item.attrlist != null && item.attrlist != 'undefined') {
                            html += "<select name='" + attrValue + + item.id + "' id='" + attrValue + item.id + "' onchange='" + objName + ".SelectChange(this);return false;'  " + classStr + " style=\"width:200px\">";
                            html += "<option value=''>请选择选项值(默认为空)</option>";
                            $.each(item.attrlist, function (i, item2) {
                                html += "<option value='" + (item2.id > 0 ? item2.value : 0) + "'>" + item2.value + "</option>";
                            })
                            html += "</select>";
                            html += "<span style='display:none;width:150px;' class='layui-word-aux'><input type='text' name='" + this.AttrValue + "Other" + item.id + "'  id='" + attrValue + "Other" + item.id + "' value='' class='form-control' style=\"width:150px\"/></span>";
                        }
                    }
                    else {   //单选框
                        if (item.attrlist != null && item.attrlist != 'undefined') {
                            html += "<label><input type='radio' checked name='" + attrValue + item.id + "' value='' onchange='" + objName + ".SelectChange(this);return false;' />空</label>";
                            $.each(item.attrlist, function (i, item2) {
                                html += "<label><input type='radio' name='" + attrValue + item.id + "' value='" + (item2.id > 0 ? item2.value : 0) + "' onchange='" + objName + ".SelectChange(this);return false;' " + classStr + " />" + item2.value + "</label>";
                            })
                            html += "<span style='display:none;width:150px;' class='layui-word-aux'><input type='text' name='" + attrValue + "Other" + item.id + "'  id='" + attrValue + "Other" + item.id + "' value='' class='form-control' style=\"width:150px\"/></span>";
                        }
                    }
                    html + "</div></td>"
                    //if (item.issearch)
                    //    html += "<span class='gray'>[可筛选属性]</span>";
                    //html += " <div style='float: right'><input type='button' value='↑' onclick='upMove(this)' /> <input type='button' value='↓' onclick='downMove(this)' /></div></td></tr>";
                    $("#" + divId).append(html);
                    //初始化验证
                    $("#" + divId).parents("form").eq(0).validate();
                });
            }
            $("#" + divId).append("<tr><th width='150px'></th><td><a href='javascript:void(0)' class='btn btn-default' onclick='" + objName + ".AddSku(this)'><i class=\"fa fa-plus\"></i> 添加</a></td></tr>");
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowError(errorThrown);
        }
    });
}

SkuAttrer.prototype.AddSku = function (obj) {
    var maxId = parseInt($("input[name='" + this.MyAttrId + "']").last().val()) + 1 || 0;
    $(obj).parents("tr").eq(0).before("<tr><th align='right' width='150px'><input type='hidden' name='" + this.MyAttrId + "' value='" + maxId + "'><input type='text' style='width:150px' name='" + this.MyAttrName + maxId + "' value='' class='form-control' required='required'></th><td><div class='form-inline'><input type='text' style='width:150px' name='" + this.MyAttrValue + maxId + "' value='' class='form-control' required='required'> <input type=\"button\" value=\"↑\" class=\"btn btn-default\" onclick='" + this.ObjName + ".Up(this)'> <input type=\"button\" value=\"↓\" class=\"btn btn-default\" onclick='" + this.ObjName + ".Down(this)'> <a href='javascript:void(0)' onclick='" + this.ObjName + ".DelSku(this)' class='btn btn-danger btn-sm'>删 除</a></div></td></tr>");
}

SkuAttrer.prototype.DelSku = function (obj) {
    $(obj).parents("tr").eq(0).remove();
}

SkuAttrer.prototype.SelectChange = function (obj) {
    var selVal = $(obj).find("option:selected").val() || $(obj).val()
    if (selVal == "0")
        $(obj).parent().parent().find('span:not(.gray)').show();
    else
        $(obj).parent().parent().find('span:not(.gray)').hide();
}

SkuAttrer.prototype.Up = function (obj) {
    var objParentTR = $(obj).parents("tr").eq(0);
    var prevTR = objParentTR.prev();
    if (prevTR.length > 0 && prevTR.find("input[name='" + this.MyAttrId + "']").length > 0) {
        prevTR.insertAfter(objParentTR);
    }
}

SkuAttrer.prototype.Down = function (obj) {
    var objParentTR = $(obj).parents("tr").eq(0);
    var nextTR = objParentTR.next();
    if (nextTR.length > 0 && nextTR.find(".form-inline").length > 0) {
        nextTR.insertBefore(objParentTR);
    }
}

//分类下拉框监视事件
$("#classid").change(function () {
    var $this = $(this);
    var $thisSelect = $this.find("option:selected");
    var value = $thisSelect.val();
    var childNum = $thisSelect.attr("child");
    var loadattr = $this.attr("loadattr");//是否加载属性

    if (value == '' || value == 0) {
        if (loadattr && loadattr == 'true') sku.Init(0);
    } else {
        if (childNum > 0) {
            $("#classPathStr").html("请选择最底层的分类");
            $("[type='submit']").prop("disabled", true);
            if (loadattr && loadattr == 'true') sku.Init(0);
        } else {
            if (loadattr && loadattr == 'true') sku.Init(value);
            $("#classPathStr").html($thisSelect.attr("attr"));
            $("[type='submit']").prop("disabled", false);
        }
    }
});

function InitClassSelect(domId, parId, selectvalue) {
    $.ajax({
        type: "get",
        url: window.manageurl + "/api/product/ShowClass",
        dataType: "json",
        data: { parId: parId, selectId: selectvalue },
        success: function (data) {
            if (data != "") {
                $("#" + domId).append(data);
            }
        }
    });
}
