/*
 
 @Name : 公共js数据访问
 @Author: jack
 @Date: 2014-11-5
 
 */
; !(function ($) {
    "use strict";//设立"严格模式"

    //全局对象 dal
    window.dal = {
        /*
        新增一条数据
        @param {String} url post方法的url参数
        @param {$对象}  myform表单对象
        @param {$对象}  bt 点击对象
        @param url post方法的url参数
        @param returnUrl 操作完成后，要跳转的url地址
        */
        insert: function (myform, bt, url, returnUrl, settings) {
            dal.execToUrl(url, returnUrl, $.extend({
                myform: myform, bt: bt, infoMsg: "新增"
            }, settings));
        },
        /*
        新增多条数据，使用json方式传递参数
        @param myform表单对象
        @param bt 点击对象
        @param url post方法的url参数
        @param returnUrl 操作完成后，要跳转的url地址
        @param data 要传人的参数，json数组
        */
        insertList: function (myform, bt, url, returnUrl, data, settings) {
            dal.execToUrl(url, returnUrl, $.extend({
                myform: myform, bt: bt, infoMsg: "新增",
                ajax: {
                    contentType: "application/json; charset=UTF-8",
                    data: JSON.stringify(data)
                }
            }, settings));
        },
        /*
        执行Update方法后，跳转到listUrl页面
        @param myform表单对象
        @param bt 点击对象
        @param url post方法的url参数
        @param returnUrl 要跳转的url地址
        */
        update: function (myform, bt, url, returnUrl) {
            dal.execToUrl(url, returnUrl, { myform: myform, bt: bt, infoMsg: "修改" });
        },

        /*
        根据主键修改数据状态,提示操作成功！或者操作失败！
        @param url ajax方法的url参数
        @param id 需要操作的数据id
        @param func 提示操作成功后，要执行的方法名
        @param msg 如果存在此参数，则给出confirm提示，否则无提示，直接执行
        */
        updateState: function (url, id, state, func, msg) {
            if (id != undefined && id != false) {
                if (msg != undefined && typeof msg === "string") {
                    dal.execConfirm(url, { id: id, state: state }, func, msg);
                } else {
                    dal.execNoConfirm(url, { id: id, state: state }, func);
                }
            }
        },

        /*
        批量修改数据状态,提示操作成功！或者操作失败！
        @param url ajax方法的url参数
        @param ids 使用,分割的主键集合ids
        @param func 提示操作成功后，要执行的方法名
        @param msg 如果存在此参数，则给出confirm提示，否则默认给出“确定要批量操作所选记录吗？？？”提示
        */
        updateStateBulk: function (url, ids, state, func, msg) {
            if (ids != undefined && ids != false) {
                if (msg != undefined && typeof msg === "string") {
                    dal.execConfirm(url, { ids: ids, state: state }, func, msg);
                } else {
                    dal.execConfirm(url, { ids: ids, state: state }, func, "确定要批量操作所选记录吗？？？");
                }
            }
        },

        /*
        根据主键删除数据,提示操作成功！或者操作失败！
        @param url ajax方法的url参数
        @param id 需要删除数据的id
        @param func 提示操作成功后，要执行的方法名
        */
        deleteById: function (url, id, func) {
            if (id != undefined && id != false) {
                dal.execConfirm(url, { id: id }, func, "确定要删除吗？");
            }
        },

        /*
        批量删除,提示操作成功！或者操作失败！
        @param url ajax方法的url参数
        @param ids 使用,分割的主键集合ids
        @param func 提示操作成功后，要执行的方法名
        */
        deleteBulk: function (url, ids, func) {
            if (ids != undefined && ids != false) {
                dal.execConfirm(url, { ids: ids }, func, "确定要删除所选记录吗？");
            }
        },


        /*
        返回部分视图html代码，替换指定标签下html
        @param url ajax方法的url参数
        @param dataDomId 要替换的数据的父标签Id （格式 "#divList"）
        @param data ajax方法的data参数（无参数，则不要此参数，或者传人{}）
        */
        getHtml: function (url, dataDomId, data) {
            dal.execAjax({
                ajax: {
                    type: "Get",
                    url: url,
                    data: data || {},
                    dataType: "html", //返回格式是html
                    async: false,
                    success: function (evt) {
                        //替换成新的数据
                        $(dataDomId).html(evt).trigger('create');
                    }
                }
            });
        },

        /*
        返回部分视图html代码，追加到指定标签下html后面
        @param url ajax方法的url参数
        @param dataDomId 要替换的数据的父标签Id （格式 "#divList"）
        @param data ajax方法的data参数（无参数，则不要此参数，或者传人{}）
        */
        getHtmlAppend: function (url, dataDomId, data) {
            dal.execAjax({
                ajax: {
                    type: "Get",
                    url: url,
                    data: data || {},
                    dataType: "html", //返回格式是html
                    async: false,
                    success: function (evt) {
                        //替换成新的数据
                        $(dataDomId).append(evt).trigger('create');
                    }
                }
            });
        }
    };



    /*************   私有对象块 ****************/

    //私有对象 构造方法
    var Class = function (setings, setings1) {
        setings = setings || {};
        setings.ajax = setings.ajax || {};
        setings1 = setings1 || {};
        setings1.ajax = setings1.ajax || {};

        var that = this, config = that.config;
        //$.extend的含义是将config,setings  合并到{}中,setings中会替换config中相同项，返回值为合并后的{}
        that.config = $.extend({}, config, setings, setings1);
        if (that.config.bt == null || that.config.bt == undefined) {
            that.config.existButton = false;
        }
        if (that.config.myform == null || that.config.myform == undefined) {
            that.config.existForm = false;
        }
        that.config.ajax = $.extend({}, config.ajax, {
            data: that.arrayForm(),
            beforeSend: function () {
                if ($.isFunction(that.config.sendBefore)) {
                    that.config.sendBefore();
                }
                return that.changeStyle();
            },
            success: function (data) {
                that.ajaxTrue(data);
            },
            error: function (xhr, textStatus, errorThrown) {
                try {
                    var json = $.parseJSON(xhr.responseText);
                    if (json.errorMessage) {
                        that.ajaxFalse(json.errorMessage);
                    } else if (json.msg) {
                        that.ajaxFalse(json.msg);
                    } else {
                        that.ajaxFalse('系统错误请联系管理员');
                    }
                } catch (e) {
                    that.ajaxFalse('系统错误请联系管理员');
                }
            }
        }, setings.ajax, setings1.ajax);
    };
    Class.pt = Class.prototype;

    //默认配置
    Class.pt.config = {
        //表单
        myform: null,
        //操作成功之后，跳转url
        returnUrl: '',
        //操作按钮
        bt: null,
        //按钮默认html
        btHtml: '<span class="common-sprite">确定</span>',
        btChangeHtml: '<span class="common-sprite">正在提交</span>',
        //不可用样式
        disableClass: 'btn-gray-h37',
        //可用样式
        enableClass: 'btn-red-h37',
        //默认提示信息前缀
        infoMsg: '操作',
        //操作成功提示
        infoMsgTrue: "",
        //操作失败提示
        infoMsgFalse: "",
        //是否存在表单
        existForm: true,
        //是否存在按钮
        existButton: true,
        //是否修改button样式
        changeBtStyle: true,
        //是否恢复button样式
        recoveryBtStyle: true,
        //是否取消按钮点击事件，不可恢复
        unBindBtClick: true,


        sendBefore: "",
        /*
        1：显示成功，跳转returnUrl；
        2：显示成功，刷新父窗体；
        3：显示成功，如果有ajaxSuccTrue，则执行；
        其它无操作，如果有ajaxSuccTrue，则执行
        */
        ajaxSuccType: 3,
        //ajax操作成功后，执行此方法
        ajaxSuccTrue: "",
        //ajax操作成功后，执行此方法
        ajaxSuccFalse: "",
        ajax: {
            type: "Post",
            //返回数据类型
            dataType: "json"
        }
    };

    //修改按钮样式，同时添加样式判断，防止重复提交，默认返回true
    Class.pt.changeStyle = function () {
        var that = this, config = that.config;
        if (config.existButton) {
            if (config.changeBtStyle) {
                var bt = config.bt;
                //取消绑定事件，无法恢复，防止重复提交
                if (config.unBindBtClick) {
                    bt.unbind("click");
                }
                //如果存在disable的样式，则不提交，防止重复提交       
                if (bt.hasClass(config.disableClass)) {
                    return false;
                } else {
                    //修改按钮文字
                    bt.html(config.btChangeHtml);
                    //修改按钮样式
                    bt.removeClass(config.enableClass).addClass(config.disableClass);
                }
            }
        }
        return true;
    };

    //恢复按钮样式，无返回值
    Class.pt.recoveryStyle = function () {
        var that = this, config = that.config;
        if (config.existButton) {
            if (config.changeBtStyle) {
                if (config.recoveryBtStyle) {
                    var bt = config.bt;
                    bt.html(config.btHtml);
                    bt.removeClass(config.disableClass).addClass(config.enableClass);
                }
            }
        }
    };

    //表单校验，默认返回true
    Class.pt.checkForm = function () {
        var that = this, config = that.config;
        if (config.existForm) {
            if ($.isFunction(config.myform.valid)) {
                return config.myform.valid();
            }
        }
        return true;
    };

    //序列化表单为数组，默认返回{}
    Class.pt.arrayForm = function () {
        var that = this, config = that.config;
        if (config.existForm) {
            return config.myform.serializeArray();
        }
        return {};
    };

    //显示错误消息
    Class.pt.ajaxFalse = function (data) {
        var that = this, config = that.config;
        if (data == "0") {
            if ($.trim(config.infoMsgFalse).length <= 0) {
                config.infoMsgFalse = config.infoMsg + "失败！";
            }
            lMsgErr(config.infoMsgFalse);
        } else {
            lMsgErr(data);
        }
        if ($.isFunction(config.ajaxSuccFalse)) {
            config.ajaxSuccFalse(data);
        }
        that.recoveryStyle();
    };

    /*
    ajax成功后 调用
    */
    Class.pt.ajaxTrue = function (data) {
        var that = this, config = that.config;
        
        if (data != "0") {
            if ($.trim(config.infoMsgTrue).length <= 1) {
                config.infoMsgTrue = config.infoMsg + "成功！";
            }
            switch (config.ajaxSuccType) {
                case -1:
                    if ($.isFunction(config.ajaxSuccTrue)) {
                        config.ajaxSuccTrue(data);
                    }
                    break;
                case 1:
                    //跳转页面                    
                    lMsgSucTo(config.infoMsgTrue, config.returnUrl);
                    break;
                case 2:
                    //刷新父窗体
                    lMsgSucParentReload(config.infoMsgTrue);
                    break;
                case 3:
                    lMsgSuc(config.infoMsgTrue);
                    if ($.isFunction(config.ajaxSuccTrue)) {
                        config.ajaxSuccTrue(data);
                    }
                    that.recoveryStyle();
                    break;
                case 4:
                    //提示成功，之后关闭所有层
                    lMsgSucClose(config.infoMsgTrue);
                    if ($.isFunction(config.ajaxSuccTrue)) {
                        config.ajaxSuccTrue(data);
                    }
                    that.recoveryStyle();
                    break;
                case -1:
                    if ($.isFunction(config.ajaxSuccTrue)) {
                        config.ajaxSuccTrue(data);
                    }
                    break;
                default://默认当json数据处理
                    dal.handleJson(data, function () {
                        config.ajaxSuccTrue(data)
                    }, function () {
                        that.ajaxFalse(data.msg);
                        //that.recoveryStyle();
                    });
                    that.recoveryStyle();
                    break;
            }
        } else {
            that.ajaxFalse(data);
        }
    };


    /**********  集成方法块  ********/

    //统一  处理返回的json数据
    dal.handleJson = function (json, suncTrue, suncFalse) {
        if (json.code == 200) {
            if ($.isFunction(suncTrue)) {
                suncTrue(json);
            }
        } else {
            if (json.msg != "") {
                lMsgErr(json.msg);
                if ($.isFunction(suncFalse)) {
                    suncFalse(json);
                }
            }
        }
    }

    /*
    将数组转换为json格式字符串
    @param array 待转换数组
    例：
    var jkbData = { model: dal.arrayToJsonStr(myform.serializeArray()), operLst: operLst };
    */
    dal.arrayToJsonStr = function (array) {
        var o = {};
        $.each(array, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    /*
    执行ajax操作
    @param setting 默认配置参数
    */
    dal.execAjax = function (setting, setting1) {
        var cs = new Class(setting, setting1);
        if (cs.checkForm()) {
            var config = cs.config;
            $.ajax(config.ajax);
        }
    };

    /*
    直接ajax 执行数据处理操作
    @param url ajax方法的url参数
    @param data ajax方法的data参数 {键：值}
    @param func 执行请求成功后，要执行的方法，参数为ajax返回的数据
    @param setting 要修改的默认配置参数
    */
    dal.execMyAjax = function (url, data, func, setting) {
        dal.execAjax({
            ajaxSuccType: -1, ajaxSuccTrue: func,
            ajax: {
                url: url, data: data
            }
        }, setting);
    };

    /*
    执行ajax操作，使用json方式传递参数
    @param myform表单对象
    @param bt 点击对象
    @param url post方法的url参数
    @param data 要传人的参数，json数组{ id: id, operLst: operLst };
    */
    dal.execByJsonData = function (myform, bt, url, data, setting) {
        dal.execAjax({
            myform: myform,
            bt: bt,
            ajaxSuccType: -1,
            ajax: {
                url: url,
                contentType: "application/json; charset=UTF-8",
                data: JSON.stringify(data)
            }
        }, setting);
    };

    /*
    执行一个操作成功后，跳转到另一个页面
    @param returnUrl 操作完成后，要跳转的url地址
    @param setting 要修改的默认配置参数
    */
    dal.execToUrl = function (url, returnUrl, setting) {
        dal.execAjax({
            returnUrl: returnUrl,
            ajaxSuccType: 1,
            ajax: { url: url }
        }, setting);
    };

    /*
    执行方法后，提示，操作成功，关闭弹窗层，同时刷新父窗体
    @param myform表单对象
    @param bt 点击对象
    @param url post方法的url参数
    @param setting 要修改的默认配置参数
    */
    dal.execReloadParent = function (myform, bt, url, setting) {
        dal.execAjax({ myform: myform, bt: bt, ajaxSuccType: 2, ajax: { url: url } }, setting);
    };

    /*
    执行方法后，提示，操作成功，之后执行指定的func 方法
    @param myform表单对象
    @param bt 点击对象
    @param url post方法的url参数
    @param func 提示操作成功后，要执行的方法名,(如果不要此参数，只提示操作成功)
    @param setting 要修改的默认配置参数
    */
    dal.execSuccFunc = function (myform, bt, url, func, setting) {
        dal.execAjax({
            myform: myform, bt: bt, ajaxSuccType: 1,
            unBindBtClick: false,
            ajaxSuccTrue: func,
            ajax: { url: url }
        }, setting);
    };

    /*
    直接ajax 执行数据处理操作,提示操作成功！或者操作失败！
    @param url ajax方法的url参数
    @param data ajax方法的data参数
    @param func 提示操作成功后，要执行的方法名,(如果不要此参数，只提示操作成功)
    @param setting 要修改的默认配置参数
    */
    dal.execNoConfirm = function (url, data, func, setting) {
        dal.execAjax({
            ajaxSuccType: 3, ajaxSuccTrue: func,
            ajax: {
                url: url, data: data
            }
        }, setting);
    };

    /*
    弹出confirm提示对话框，确定后ajax 执行数据处理操作,提示操作成功！或者操作失败！
    @param url ajax方法的url参数
    @param data 需要操作的数据参数
    @param func 提示操作成功后，要执行的方法名
    @param msg  弹出msg对话框，确定后执行，否则不执行
    @param setting 要修改的默认配置参数
    */
    dal.execConfirm = function (url, data, func, msg, setting) {
        //if (confirm(msg)) {
        //    dal.execNoConfirm(url, data, func, setting);
        //}
        layer.confirm(msg, function (index) {
            dal.execNoConfirm(url, data, func, setting);
        });
    };

    // RequireJS && SeaJS
    if (typeof define === 'function' && define.amd) {
        // AMD模式
        define(["jquery"], function () {
            return dal;
        });
    }
})(jQuery);