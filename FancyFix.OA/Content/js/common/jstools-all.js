/**

    jsTools(js工具集) 当前库依赖第三方库：
    1).jQuery（v1.7.x）。详细了解：http://jquery.com/
    2).json2.js库。如果浏览器支持JSON.stringify和JSON.parse接口就不需要此库，详细了解：http://www.json.org/

    当前库实现的基本功能：
    0).jsTools: 基础库；
    1).jsTools.form: 表单操作；

    @Author:  jack
    @Date: 2014-12-18
    说明：基于Chihpeng Tsai(470597142@qq.com) git: https://github.com/iiTsai/iTsai-Webtools  改写

*/
; !(function (window, $, undefined) {
    "use strict";//设立"严格模式"

    if (!window.jsTools) {
        window.jsTools = {
            /**
            * jsTools库版本
            * @type {String}
            * @property version
            */
            version: '1.0.0',
            //是否是IE6
            ie6: !!window.ActiveXObject && !window.XMLHttpRequest,
            /**
             * 生成唯一CID编号:时间+4位随机数
             *
             * @method random
             * @return {String} 随机数
             */
            random: function () {
                return new Date().getTime() + '' + Math.round(Math.random() * 10000);
            },
            //跳转到指定url
            toUrl: function (url) {
                window.location.href = url;
            },
            /**
             * 判断是否含有'.'号
             *
             * @method hasDot
             * @param {String} str 输入字符串
             * @return {Boolean}
             */
            hasDot: function (str) {
                if (typeof str != 'string') {
                    return false;
                }
                if (str.indexOf('.') != -1) {
                    return true;
                }
                return false;
            },
            /**
             * 判断对象是否为纯整形数字或整形数字字符串 011=9(011 表示8进制)
             *
             * @method isInteger
             * @param {Number/String} obj 输入数字或字符串
             * @return {Boolean}
             */
            isInteger: function (obj) {
                if (obj != parseInt(obj, 10)) {
                    return false;
                }
                return true;
            },
            /**
             * 将"undefined"和null转换为空串
             *
             * @method obj2Empty
             * @param {Object} obj 输入对象
             * @return {Object}
             */
            obj2Empty: function (obj) {
                if (typeof obj == "undefined" || obj == null) {
                    return '';
                }
                return obj;
            },
            /**
             * 检测插件是否存在,如:'Quicktime'/'Quicktime.Quicktime'<br>
             * IE浏览器控件的名称通道和其它浏览器插件名称不一致
             *
             * @method checkPlugin
             * @param {String} name 插件名称
             * @param {String} nameIE [optional,default=name] IE浏览器ActiveX插件名称
             * @return {Boolean} true-插件已经安装;false-未安装
             */
            checkPlugin: function (name, nameIE) {
                var ie = '';
                if (typeof nameIE === 'undefined') {
                    ie = name;
                } else {
                    ie = nameIE;
                }
                return this.hasPluginIE(ie) || this.hasPlugin(name);
            },
            /**
             * 检测非IE浏览器插件是否存在
             *
             * @method hasPlugin
             * @param {String} name 插件名称
             * @return {Boolean} true-插件已经安装;false-未安装
             */
            hasPlugin: function (name) {
                if (!name)
                    return false;
                name = name.toLowerCase();
                var plugins = window.navigator.plugins;
                for (var i = 0; i < plugins.length; i++) {
                    if (plugins[i] && plugins[i].name.toLowerCase().indexOf(name) > -1) {
                        return true;
                    }
                }
                return false;
            },
            /**
             * 检测IE浏览器插件是否存在
             *
             * @method hasPluginIE
             * @param {String} name IE浏览器ActiveX插件名称
             * @return {Boolean} true-插件已经安装;false-未安装
             */
            hasPluginIE: function (name) {
                if (!name)
                    return false;
                try {
                    new ActiveXObject(name);
                    return true;
                } catch (ex) {
                    return false;
                }
            },
            /**
             * 颜色取反，如将白色'#ffffff'转换为黑色'#000000'
             *
             * @method colorInverse
             * @param {String} color 颜色16进制字符表示形式，如：'#ff0000'，表示红色。
             * @return {String} 取反后的颜色
             */
            colorInverse: function (color) {
                color = !color ? '' : color;
                color = parseInt(color.replace('#', '0x'));
                var r = color >> 16, g = color >> 8 & 0x0000ff, b = color & 0x0000ff, _r = 255 - r, _g = 255 - g, _b = 255 - b, clr = '#'
                    + (_r << 16 | _g << 8 | _b).toString(16);
                return clr == '#0' ? '#000000' : clr;
            },
            /**
             * 获取浏览器语言代码,如:'zh-CN'
             *
             * @method getLang
             * @return {String} 语言代码
             */
            getLang: function () {
                var nav = window.navigator;
                return (nav.language || nav.userLanguage);
            },
            /**
             * 取消事件冒泡
             *
             * @method stopBubble
             * @param {Object} e 事件对象
             */
            stopBubble: function (e) {
                if (e && e.stopPropagation) {
                    e.stopPropagation();
                } else {
                    // ie
                    window.event.cancelBubble = true;
                }
            },
            /**
             * 阻止浏览器默认行为
             *
             * @method stopDefault
             * @param {Object} e 事件对象
             * @return {Boolean}
             */
            preventDefault: function (e) {
                if (e && e.preventDefault) {
                    e.preventDefault();
                } else {
                    // ie
                    window.event.returnValue = false;
                }
                return false;
            }
        };
    }

    // RequireJS && SeaJS
    if (typeof define === 'function' && define.amd) {
        // AMD模式
        define(["jquery"], function () {
            return jsTools;
        });
    }

    /*

    序列化表单值,结果以key/value形式返回key为表单对象名称(name||id),value为其值。input type 为'button','reset','submit','image'会被过虑掉。
    @param {Object} doms jQuery表单对象  或者表单数组
    @ignoreNames 要忽略的标签Name集合  ['ID','Age']
    @returnArray 是否返回数组对象
    @return {Object} json对象

    使用：  var uspLst = $("#divContact dl[class='on']").toJson(["ID"]);
    */
    $.fn.toJson = function (ignoreNames, returnArray) {
        return jsTools.form.toJson($(this), ignoreNames, returnArray);
    };
})(window, jQuery);

//通过js操作 相关按键操作
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"

    jsTools.bindKey = {
        /**
             * 常用键盘码对象。
             *
             * @type {Object}
             * @namespace iTsai.form
             * @class keycode
             */
        keycode: {
            /**
             * 全屏F11(122)
             *
             * @type {Number}
             * @property F11
             */
            F11: 122,
            /**
             * 退出Esc(27)
             *
             * @type {Number}
             * @property ESC
             */
            ESC: 27,
            /**
             * 回车Enter(13)
             *
             * @type {Number}
             * @property ENTER
             */
            ENTER: 13,
            /**
             * 上一页Page Up(33)
             *
             * @type {Number}
             * @property PAGEUP
             */
            PAGEUP: 33,
            /**
             * 下一页Page Down(34)
             *
             * @type {Number}
             * @property PAGEDOWN
             */
            PAGEDOWN: 34,
            /**
             * 页尾end(35)
             *
             * @type {Number}
             * @property END
             */
            END: 35,
            /**
             * 页首home(36)
             *
             * @type {Number}
             * @property HOME
             */
            HOME: 36,
            /**
             * 左箭头left(37)
             *
             * @type {Number}
             * @property LEFT
             */
            LEFT: 37,
            /**
             * 向上箭头up(38)
             *
             * @type {Number}
             * @property UP
             */
            UP: 38,
            /**
             * 右前头(39)
             *
             * @type {Number}
             * @property RIGHT
             */
            RIGHT: 39,
            /**
             * 向下箭头down(40)
             *
             * @type {Number}
             * @property DOWN
             */
            DOWN: 40
        },
        /**
         * 绑定键盘事件到元素，当焦点在元素上并触发键盘事件时响应该函数。
         *
         * @method _bindKey
         * @param {Number} jsTools.form.keycode 键盘码
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         */
        _bindKey: function (keycode, element, callback) {
            if (!(element instanceof jQuery)) {
                element = $(element);
            }
            element.keydown(function (e) {
                if (e.keyCode == keycode) {
                    if (typeof callback == 'function')
                        callback(element, e);
                }
            });
        },
        /**
         * 在element区域内响应Enter键盘事件。<br>
         * 实际处理中应该将提交按键(type="submit")放在element区域外,避免重复提交。
         *
         * @method bindEnterKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindEnterKey: function (element, callback) {
            this._bindKey(this.keycode.ENTER, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Esc键盘事件。
         *
         * @method bindEscKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindEscKey: function (element, callback) {
            this._bindKey(this.keycode.ESC, element, callback);
            return this;
        },
        /**
         * 在element区域内响应F11键盘事件。
         *
         * @method bindF11Key
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindF11Key: function (element, callback) {
            this._bindKey(this.keycode.F11, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Page Down键盘事件。
         *
         * @method bindPageDownKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindPageDownKey: function (element, callback) {
            this._bindKey(this.keycode.PAGEDOWN, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Page Up键盘事件。
         *
         * @method bindPageUpKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindPageUpKey: function (element, callback) {
            this._bindKey(this.keycode.PAGEUP, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Left键盘事件。
         *
         * @method bindLeftKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindLeftKey: function (element, callback) {
            this._bindKey(this.keycode.LEFT, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Right键盘事件。
         *
         * @method bindRightKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindRightKey: function (element, callback) {
            this._bindKey(this.keycode.RIGHT, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Up键盘事件。
         *
         * @method bindUpKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindUpKey: function (element, callback) {
            this._bindKey(this.keycode.UP, element, callback);
            return this;
        },
        /**
         * 在element区域内响应Down键盘事件。
         *
         * @method bindDownKey
         * @param {Object} element 被绑定元素的jQuery对象 或者 ("#form1")
         * @param {Function} callback 回调函数，参数为绑定的元素对象element和事件e
         * @return {Object} iTsai.form
         */
        bindDownKey: function (element, callback) {
            this._bindKey(this.keycode.DOWN, element, callback);
            return this;
        }
    };
})(jsTools, jQuery);

/**
 扩展  表单处理工具
@Date 2014-12-19
*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.form = {
        /**
         * 禁用/启用输入控件。
         *
         * @method formDisable
         * @param {Object} frmObj iQuery表单对象（或其它任何包装容器，如：div）
         * @param {Boolean} disabled true-禁用;false-启用
         * @return {Object} iTsai.form
         */
        formDisable: function (frmObj, disabled) {
            frmObj.find('input,select,textarea').attr('disabled', disabled);
            return this;
        },
        /**
         * 将输入控件集合序列化成对象， 名称或编号作为键，value属性作为值。
         *
         * @method _serializeInputs
         * @param {Array} inputs input/select/textarea的对象集合
         * @param {Array} ignoreNames 要忽略的标签Name集合  格式：['ID','Age']
         * @return {Object} json 对象 {key:value,...}
         */
        _serializeInputs: function (inputs, ignoreNames) {
            var json = {};
            if (!inputs) {
                return json;
            }
            for (var i = inputs.length - 1; i >= 0; i--) {
                var input = $(inputs[i]), type = input.attr('type');
                if (type) {
                    type = type.toLowerCase();
                }
                var tagName = input.get(0).tagName, id = input.attr('id'), name = input
                    .attr('name'), value = null;

                //过滤不可用
                if (input.attr("disabled")) {
                    continue;
                }

                //inArray 函数 返回第一个参数在数组中的位置，默认下标0开始计数(如果没有找到则返回 -1）
                //要忽略的input标签的name数组
                if (typeof ignoreNames != 'undefined' && $.inArray(name, ignoreNames || []) >= 0) {
                    continue;
                }

                // 判断输入框是否已经序列化过
                if (input.hasClass('_isSerialized') || (typeof id == 'undefined' && typeof name == 'undefined')) {
                    continue;
                }

                // input输入标签
                if (tagName == 'INPUT' && type) {
                    switch (type) {
                        case 'checkbox':
                            {
                                //value = input.is(':checked');
                                //console.log(value);
                                if (input.is(':checked')) {
                                    value = 1;
                                } else {
                                    value = 0;
                                }
                            }
                            break;
                        case 'radio':
                            {
                                if (input.is(':checked')) {
                                    value = input.attr('value');
                                } else {
                                    continue;
                                }
                            }
                            break;
                        default:
                            {
                                value = input.val();
                            }
                    }
                } else {
                    // 非input输入标签，如：select,textarea
                    value = input.val();
                }

                json[name || id] = value;
                // 清除序列化标记
                input.removeClass('_isSerialized');
            }
            return json;
        },
        /**
         * 将值填充到输入标签里面。
         *
         * @method _deserializeInputs
         * @param {Array} inputs 输入标签集合
         * @param {String/Number} value 值
         * @return {Object} iTsai.form
         */
        _deserializeInputs: function (inputs, value) {
            if (!inputs && value == null) {
                return this;
            }

            for (var i = inputs.length - 1; i >= 0; i--) {
                var input = $(inputs[i]);
                // 判断输入框是否已经序列化过
                if (input.hasClass('_isSerialized')) {
                    continue;
                }
                var type = input.attr('type');
                if (type) {
                    type = type.toLowerCase();
                }
                if (type) {
                    switch (type) {
                        case 'checkbox':
                            {
                                input.attr('checked', value);
                            }
                            break;

                        case 'radio':
                            {
                                input.each(function (i) {
                                    var thiz = $(this);
                                    if (thiz.attr('value') == value) {
                                        thiz.attr('checked', true);
                                    }
                                });
                            }
                            break;
                        default:
                            {
                                input.val(value);
                            }
                    }
                } else {
                    input.val(value);
                }
                input.addClass('_isSerialized');
            }
            return this;
        },
        /**
         * 在分组中查找 fieldset (如：fieldset="user")开头的数据域。
         *
         * @method _serializeGroups
         * @param {Array} groups 输入框分组容器集合
         * @return {Object} json 对象 {key:value,...}
         */
        _serializeGroups: function (groups) {
            var json = {};
            if (!groups) {
                return json;
            }
            for (var i = groups.length - 1; i >= 0; i--) {
                var group = $(groups[i]), key = group.attr('fieldset');
                if (!key) {
                    continue;
                }
                var inputs = group
                    .find('input[type!=button][type!=reset][type!=submit],select,textarea');
                json[key] = this._serializeInputs(inputs);
                // 添加序列化标记
                inputs.addClass('_isSerialized');
            }
            return json;
        },
        /**
         * 序列化表单值,结果以key/value形式返回key为表单对象名称(name||id),value为其值。<br>
         * HTML格式：<br>
         * 1).表单容器：通常是一个form表单（如果不存在就以body为父容器），里面包含输入标签和子容器;<br>
         * 2).子容器（也可以没有）：必须包括属性fieldset="XXXX" div标签，里面包含输入标签和子容器。<br>
         * 序列化后将生成以XXX为主键的json对象.如果子容器存在嵌套则以fieldset为主键生成不同分组的json对象。<br>
         * 3).输入标签：输入标签为input类型标签（包括：'checkbox','color','date','datetime','datetime-local',<br>
         * 'email','file','hidden','month','number','password','radio','range
         * ','reset','search','submit',<br>
         * 'tel','text','time ','url','week'）。
         * 而'button','reset','submit','image'会被过虑掉。
         *
         * @method serialize
         * @param {Object} frm jQuery表单对象
         * @return {Object} json对象，最多包含两层结构
         */
        serialize: function (frm) {
            var json = {};
            frm = frm || $('body');
            if (!frm) {
                return json;
            }
            var groups = frm.find('div[fieldset]'), jsonGroup = this
                ._serializeGroups(groups), inputs = frm
                .find('input[type!=button][type!=reset][type!=submit][type!=image],select,textarea'), json = this
                ._serializeInputs(inputs);

            for (var key in jsonGroup) {
                json[key] = jsonGroup[key];
            }
            return json;
        },
        /**
         * 填充表单内容：将json数据形式数据填充到表单内，只解析单层json结构。
         *
         * @method deserializeSimple
         * @param {Object} frm jQuery表单对象（或其它容器标签对象，如：div）
         * @param {Object} json 序列化好的json数据对象，最多只包含两层嵌套
         * @return {Object} iTsai.form
         */
        deserializeSimple: function (frm, json) {
            frm = frm || $('body');
            if (!frm || !json) {
                return this;
            }

            var _deserializeInputs = this._deserializeInputs;
            for (var key in json) {
                var value = json[key];
                _deserializeInputs(frm, key, value);
            }
            return this;
        },
        /**
         * 获取合法的输入标签。
         *
         * @method _filterInputs
         * @param {Object} container jQuery对象，标签容器
         * @return {Array} inputs jQuery对象数组
         */
        _filterInputs: function (container) {
            return $(container
                .find('input[type!=button][type!=reset][type!=submit][type!=image][type!=file],select,textarea'));
        },
        /**
         * 查找符合条件的输入标签。
         *
         * @method _findInputs
         * @param {Array} inputs jQueery输入标签数组
         * @param {String} key 查询关键字
         * @return {Array} inputs jQuery对象数组
         */
        _findInputs: function (inputs, key) {
            return $(inputs.filter('input[name=' + key + '],input[id=' + key
                + '],textarea[name=' + key + '],textarea[id=' + key
                + '],select[name=' + key + '],select[id=' + key + ']'));
        },
        /**
         * 填充表单内容：将json数据形式数据填充到表单内，最多解析两层json结构。
         *
         * @method deserialize
         * @param {Object} frm jQuery表单对象（或其它容器标签对象，如：div）
         * @param {Object} json 序列化好的json数据对象，最多只包含两层嵌套
         * @return {Object} iTsai.form
         */
        deserialize: function (frm, json) {
            frm = frm || $('body');
            if (!frm || !json) {
                return this;
            }

            // objects缓存json第一层数据对象;
            // groups缓存json嵌套层数据（第二层），将首先被赋值，以避免覆盖
            var objects = {}, groups = {};

            // 数据分组
            for (var key in json) {
                var value = json[key];
                if ($.isPlainObject(value)) {
                    groups[key] = value;
                } else {
                    objects[key] = value;
                }
            }

            var _deserializeInputs = this._deserializeInputs, _filterInputs = this._filterInputs, _findInputs = this._findInputs;

            // 填充嵌套层数据
            for (var key in groups) {
                var json = groups[key], div = frm.find('div[fieldset="' + key
                    + '"]');
                if (!div.length) {
                    continue;
                }
                var inputs = _filterInputs(div);
                if (!inputs.length) {
                    continue;
                }
                for (var k in json) {
                    var val = json[k], input = _findInputs(inputs, k);
                    _deserializeInputs(input, val);
                }
            }

            // 填充第一层数据
            var inputs = _filterInputs(frm);
            for (var key in objects) {
                var value = objects[key], input = _findInputs(inputs, key);
                _deserializeInputs(input, value);
            }

            inputs.filter('._isSerialized').removeClass('_isSerialized');
            return this;
        },

        /*

        序列化表单值,结果以key/value形式返回key为表单对象名称(name||id),value为其值。input type 为'button','reset','submit','image'会被过虑掉。
        @param {Object} doms jQuery表单对象  或者表单数组
        @ignoreNames 要忽略的标签Name集合  ['ID','Age']
        @returnArray 是否返回数组对象
        @return {Object} json对象

        使用：  var uspLst = toJson($("#divContact dl[class='on']"),["ID"]);
        */
        toJson: function (doms, ignoreNames, returnArray) {
            doms = doms || $('body');
            if (returnArray) {
                var jsons = [];
                doms.each(function (i) {
                    jsons.push(toJsonOne($(doms[i])));
                });
                return jsons;
            } else {
                return toJsonOne(doms);
            }
            function toJsonOne(domId) {
                var json = {};
                if (!domId) {
                    return json;
                }
                var groups = domId.find('div[fieldset]'),
                    jsonGroup = jsTools.form._serializeGroups(groups),
                    inputs = domId.find('input[type!=button][type!=reset][type!=submit][type!=image],select,textarea'),
                    json = jsTools.form._serializeInputs(inputs, ignoreNames);

                for (var key in jsonGroup) {
                    json[key] = jsonGroup[key];
                }
                return json;
            }
        }
    };
})(jsTools, jQuery);

/*
复选框相关操作
*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.form.checkbox = {
        /*
        点击多选，如果当前为选中，则全选中，否则全不选中
        全选，使用 allClick($("#tdata input[type='checkbox']"), this);
        选中奇数，使用 allClick($("#tdata input[type='checkbox']:even"), this);
        选中偶数，使用 allClick($("#tdata input[type='checkbox']:odd"), this);
        @param cbs 待操作的所有复选框
        @param dom 要操作的复选框按钮id，格式 "#divType"
        */
        allClick: function (cbs, dom) {
            if ($(dom).attr('checked')) {
                cbs.attr('checked', 'true');
            } else {
                cbs.removeAttr('checked');
            }
        },
        /*
        全选，使用 allSelect("#tdata");
        @param cbs 待操作的所有复选框 父dom  Id,  格式 "#divType"
        @param dom 当前点击操作按钮id，格式 "#allClick"
        @param  checkFunc    当选中执行方法
        @param  disCheckFunc 当取消选中执行方法
        */
        allSelect: function (cbs, dom, checkFunc, disCheckFunc) {
            var cblst = $(cbs).children().find("input[type='checkbox']:not([dischecked])");
            if ($(dom).length > 0) {
                if ($(dom).attr('checked')) {
                    cblst.attr('checked', 'true');
                    if ($.isFunction(checkFunc)) {
                        checkFunc();
                    }
                } else {
                    cblst.removeAttr('checked');
                    if ($.isFunction(disCheckFunc)) {
                        disCheckFunc();
                    }
                }
            } else {
                cblst.attr('checked', 'true');
            }
        },
        /*
        反选 （已过期，推荐使用reverseSelect）
        @cbs 待操作的所有复选框
        */
        reverseClick: function (cbs) {
            cbs.each(function () {
                jsTools.form.checkbox.setCheckdState(this);
            })
        },
        /*
       反选，使用 reverseSelect("#tdata");
       @cbs 待操作的所有复选框
       */
        reverseSelect: function (cbs) {
            var rc = $(cbs).children().find("input[type='checkbox']:not([dischecked])");
            rc.each(function () {
                jsTools.form.checkbox.setCheckdState(this);
            })
        },
        /*
        如果复选框 当前为选中状态，则设置为不选中，否则设置为选中
        @cbs 待操作的单个复选框
        */
        setCheckdState: function (cbs) {
            if ($(cbs).attr('checked')) {
                $(cbs).removeAttr('checked');
            } else {
                $(cbs).attr('checked', 'true');
            }
        },
        /*
        获取所有选中的复选框的值，使用,分割
        使用 getCheckedValue("#divType");
        @param domId  复选框父类标签id，格式 "#divType"
        */
        getCheckedValue: function (domId) {
            return this.getCheckedAttr(domId, "value");
        },
        /*
        获取所有选中的复选框的id，使用,分割
        使用 getCheckedId("#divType");
        @param domId  复选框父类标签id，格式 "#divType"
        */
        getCheckedId: function (domId) {
            return this.getCheckedAttr(domId, "id");
        },
        /*
        获取所有选中的复选框的name，使用,分割
        使用 getCheckedName("#divType");
        @param domId  复选框父类标签id，格式 "#divType"
        */
        getCheckedName: function (domId) {
            return this.getCheckedAttr(domId, "name");
        },
        /*
        获取所有选中的复选框的指定属性值，使用,分割
        使用 getCheckedAttr("#divType","name");
        @param domId  复选框父类标签id，格式 "#divType"
        */
        getCheckedAttr: function (domId, attrName) {
            var atts = '';
            var cbChecked = $(domId).children().find("input[type='checkbox']:checked");
            cbChecked.each(function (i) {
                var a = $(this).attr(attrName);
                if (a != undefined && a!='') {
                    atts += $(this).attr(attrName);
                    if (i < cbChecked.length - 1) {
                        atts += ",";
                    }
                }
            });
            return atts;
        },
        /*
        检验指定标签下，所有的复选框是否有选中的，如果有则选择，否则不选中。
        @param cbs 待操作的所有复选框 父dom  Id,  格式 "#divType"
        @param dom 当前点击操作按钮id，格式 "#allClick"
        */
        checkChecked: function (cbs, dom) {
            var cblst = $(cbs).children().find("input[type='checkbox']:not([dischecked])");
            if ($(dom).length > 0) {
                var b = false;
                cblst.each(function (i) {
                    if ($(this).attr('checked')) {
                        b = true;
                        return false;
                    }
                });
                if (b) {
                    $(dom).attr('checked', 'true');
                } else {
                    $(dom).removeAttr('checked');
                }
            }
        },
        /*
        获取所有选中的复选框的数量
        使用 getCheckedNumber("#divType");
        @param domId  复选框父类标签id，格式 "#divType"
        */
        getCheckedNumber: function (domId) {
            var cbChecked = $(domId).children().find("input[type='checkbox']:checked");
            return cbChecked.length;
        },
        /*
        如果复选框为选中状态，设置其值为true，否则为false
        @dom 为jquery对象字符串 "#isShow";
        */
        setValueAsBool: function (dom) {
            var input = $(dom);
            var check = input.attr('checked');
            if (check != undefined && check == "checked") {
                input.val(true);
            } else {
                input.val(false);
            }
        },
        /*
        如果复选框为选中状态，设置其值为1，否则为0
        @dom 为jquery对象字符串 "#isShow";
        */
        setValueAs01: function (dom) {
            var input = $(dom);
            var check = input.attr('checked');
            if (check != undefined && check == "checked") {
                input.val(1);
            } else {
                input.val(0);
            }
        },
        /*
        如果复选框值为1，则设置选中
        @dom 为jquery对象字符串 "#isShow";
        */
        setCheckedBy01: function (dom) {
            this.setCheckedByValue(dom, "1");
        },
        /*
        如果复选框值为指定值，则设置选中
        @dom       为jquery对象字符串 "#isShow";
        @checkVal  如果复选框值等于指定的值，则选中
        */
        setCheckedByValue: function (dom, checkVal) {
            this.setCheckedByAttr(dom, checkVal, "value");
            //var input = $(dom);
            //if ($.trim(input.val()) == $.trim(checkVal)) {
            //    input.attr('checked', true);
            //} else {
            //    input.attr('checked', false);
            //}
        },
        /*
        如果复选框值 指定属性 为指定值，则设置选中
        @dom       为jquery对象字符串 "#isShow";
        @checkVal  如果复选框 指定属性 等于指定的值，则选中
        @atrrName  指定属性名称
        */
        setCheckedByAttr: function (dom, checkVal, atrrName) {
            var input = $(dom);
            if ($.trim(input.attr(atrrName)) == $.trim(checkVal)) {
                input.attr('checked', true);
            } else {
                input.attr('checked', false);
            }
        }
    };
})(jsTools, jQuery);

/*

单选框相关操作

*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.form.radio = {
        /**
            * 获取单选框值,如果有表单就在表单内查询,否则在全文查询
            *
            * @method getRadioValue
            * @param {String}name radio名称
            * @param {Object} frm [optional,default=document] jQuery表单（或其它容器对象）对象
            * @return {Object} radio jQuery对象
            */
        getRadioValue: function (name, frm) {
            if (frm && frm.find)
                return frm.find('input[name="' + name + '"]:checked').val();
            return $('input[name="' + name + '"]:checked').val();
        },
        /*
        如果单选框值为指定值，则设置选中
        @dom       为jquery对象字符串 "#isShow";
        @checkVal  如果单选框值等于指定的值，则选中
        */
        setCheckedByValue: function (dom, checkVal) {
            var divId = $(dom);
            if (divId && divId.find) {
                return divId.find('input[type="radio"][value="' + checkVal + '"]').attr('checked', true);
            }
        },

        /*
        获取选中的单选框的值
        使用 getCheckedValue("#divType");
        @param domId  单选框父类标签id，格式 "#divType"
        */
        getCheckedValue: function (domId) {
            return this.getCheckedAttr(domId, "value");
        },
        /*
        获取选中的单选框的id
        使用 getCheckedId("#divType");
        @param domId  单选框父类标签id，格式 "#divType"
        */
        getCheckedId: function (domId) {
            return this.getCheckedAttr(domId, "id");
        },
        /*
        获取选中的单选框的name
        使用 getCheckedId("#divType");
        @param domId  单选框父类标签id，格式 "#divType"
        */
        getCheckedName: function (domId) {
            return this.getCheckedAttr(domId, "name");
        },
        /*
        获取选中的单选框的name
        使用 getCheckedId("#divType","name");
        @param domId  单选框父类标签id，格式 "#divType"
        */
        getCheckedAttr: function (domId, att) {
            var cbChecked = $(domId).children().find("input[type='radio']:checked");
            return cbChecked.attr(att);
        }
    };
})(jsTools, jQuery);

/*

下拉框相关操作

*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.form.select = {
        /**
         * 设置select下拉框的值
         *
         * @method setRadioValue
         * @param {String} selectId 下拉框id号
         * @param {String/Number} value select表单value值
         * @param {Object} frm [optional,default=document] jQuery表单（或其它容器对象）对象
         * @return {Object} select jQuery对象
         */
        setSelectValue: function (selectId, value, frm) {
            if (frm && frm.find)
                return frm.find('#' + selectId + ' option[value="' + value + '"]')
                    .attr('selected', true);
            return $('#' + selectId + ' option[value="' + value + '"]').attr(
                'selected', true);
        },
        /**
        * 将object转换为select的列表模式，key为option的value，值为option的文本。
        *
        * @method object2Options
        * @param {Object}objects key-map对象
        * @return {String} html
        */
        object2Options: function (objects) {
            if (!$.isPlainObject(objects)) {
                return '';
            }
            var html = [];
            for (var i in objects) {
                html.push('<option value="' + i + '">' + objects[i] + '</option>');
            }
            return html.join('');
        }
    };
})(jsTools, jQuery);

/*

js 与日期 相关的操作

*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.date = {
        /*
        将时间 转化为指定格式的字符串
        例子：
        jsTools.date.dateToString(new Date(),"yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
        */
        dateToString: function (date, fmt) {
            var dtTmp = jsTools.date.toDate(date);
            var o = {
                "M+": dtTmp.getMonth() + 1, //月份
                "d+": dtTmp.getDate(), //日
                "h+": dtTmp.getHours() % 12 == 0 ? 12 : dtTmp.getHours() % 12, //小时(12小时制)
                "H+": dtTmp.getHours(), //小时（24小时制）
                "m+": dtTmp.getMinutes(), //分
                "s+": dtTmp.getSeconds(), //秒
                "q+": Math.floor((dtTmp.getMonth() + 3) / 3), //季度
                "S": dtTmp.getMilliseconds() //毫秒
            };
            if (/(y+)/.test(fmt)) {
                fmt = fmt.replace(RegExp.$1, (dtTmp.getFullYear() + "").substr(4 - RegExp.$1.length));
            }
            for (var k in o) {
                if (new RegExp("(" + k + ")").test(fmt)) {
                    fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
                }
            }
            return fmt;
        },
        /*
        将当前时间 转化为指定格式的字符串
        例子：
        jsTools.date.nowToString("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423
        */
        nowToString: function (fmt) {
            var date = new Date();
            return jsTools.date.dateToString(date, fmt);
        },

        //当前时间转换为UNIX时间戳
        nowToTimestamp: function () {
            return this.toTimestamp(new Date());
        },
        //转换为UNIX时间戳
        toTimestamp: function (date) {
            var datum = this.toDate(date);
            return (datum.getTime());
        },
        //UNIX时间戳转换为日期
        timestampToDate: function (timestamp) {
            var unixTimestamp = new Date(timestamp * 1000);
            var d = new Date(timestamp * 1000);
            return d;
        },

        /*
        将日期转换为  星期X
        */
        dateToWeek: function (date) {
            var dtTmp = jsTools.date.toDate(date);
            var week = " 星期" + "日一二三四五六 ".charAt(dtTmp.getDay());
            return week;
        },
        /*
        将 非日期格式  转换为  日期
        */
        toDate: function (dateStr) {
            if (dateStr instanceof Date) {
                return dateStr;
            }
            var date = new Date(Date.parse(dateStr.replace(/-/g, "/")));
            return date;
        },
        /*
        根据生日字符串  计算年龄
        */
        getAge: function (birth) {
            var bDate = jsTools.date.toDate(birth),
                bMonth = bDate.getMonth() + 1,
                bDay = bDate.getDate();
            var now = new Date(),
                month = now.getMonth() + 1,
                day = now.getDate();
            var age = now.getFullYear() - bDate.getFullYear() - 1;
            if (bMonth < month || (bMonth == month && bDay <= day)) {
                age++;
            }
            return age;
        },
        /*
        日期 增加或者减少
        @date 原日期
        @cType  要修改项 y年、M月、d日、h时、m分、s秒、w周、q季
        */
        dateAdd: function (date, cType, num) {
            var dtTmp = jsTools.date.toDate(date);
            switch (cType) {
                case 'y':
                    var year = dtTmp.getFullYear(),
                        month = dtTmp.getMonth(),
                        day = dtTmp.getDate(),
                        hour = dtTmp.getHours(),
                        minutes = dtTmp.getMinutes(),
                        seconds = dtTmp.getSeconds();
                    return new Date(year + num, month, day, hour, minutes, seconds);
                case 'q':
                    var year = dtTmp.getFullYear(),
                        month = dtTmp.getMonth(),
                        day = dtTmp.getDate(),
                        hour = dtTmp.getHours(),
                        minutes = dtTmp.getMinutes(),
                        seconds = dtTmp.getSeconds();
                    return new Date(year, month + num * 3, day, hour, minutes, seconds);
                case 'M':
                    var year = dtTmp.getFullYear(),
                        month = dtTmp.getMonth(),
                        day = dtTmp.getDate(),
                        hour = dtTmp.getHours(),
                        minutes = dtTmp.getMinutes(),
                        seconds = dtTmp.getSeconds();
                    return new Date(year, month + num, day, hour, minutes, seconds);
                case 'w': return new Date(Date.parse(dtTmp) + (1000 * 60 * 60 * 24 * 7 * num));
                case 'd': return new Date(Date.parse(dtTmp) + (1000 * 60 * 60 * 24 * num));
                case 'h': return new Date(Date.parse(dtTmp) + (1000 * 60 * 60 * num));
                case 'm': return new Date(Date.parse(dtTmp) + (1000 * 60 * num));
                case 's': return new Date(Date.parse(dtTmp) + (1000 * num));
                case 'S': return new Date(Date.parse(dtTmp) + num);
            }
        },
        /*
        计算日期时间差 (startDate - endDate)，如rType 为空或者null  则默认返回天
        @rType 返回类型  y年、M月、d日、h时、m分、s秒、w周、q季
        */
        getDateDiff: function (startDate, endDate, rType) {
            startDate = jsTools.date.toDate(startDate);
            endDate = jsTools.date.toDate(endDate);
            var startTime = startDate.getTime();
            var endTime = endDate.getTime();
            var num = 1000 * 60 * 60 * 24;//把相差的毫秒数转换为天
            if (rType != null && rType != undefined) {
                switch (rType) {
                    case 'y':
                        var syear = startDate.getFullYear();
                        var eyear = endDate.getFullYear();
                        return Math.abs((syear - eday));
                    case 'M':
                        var smonth = startDate.getMonth();
                        var emonth = endDate.getMonth();
                        return Math.abs((smonth - emonth));
                    case 'w': num = 1000 * 60 * 60 * 24 * 7; break;
                    case 'd': num = 1000 * 60 * 60 * 24; break;
                    case 'h': num = 1000 * 60 * 60; break;
                    case 'm': num = 1000 * 60; break;
                    case 's': num = 1000; break;
                    case 'S': num = 1; break;
                }
            }
            //Math.abs((startTime - endTime)) 得到的是 时间差的毫秒数  绝对值
            var dates = (startTime - endTime) / num;
            return dates;
        }
    };
})(jsTools, jQuery);

/*
js 针对html5的相关操作
*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.html5 = {
        //校验浏览器是否支持html localStorage
        supportsLocalStorage: function () {
            try {
                return 'localStorage' in window && window['localStorage'] !== null;
            } catch (e) {
                return false;
            }
        },
        //如果浏览器支持localStorage 则返回localStorage值，否则返回null
        getLocalStorage: function (key) {
            if (this.supportsLocalStorage()) {
                var temp = localStorage.getItem(key);
                return temp;
            }
            return null;
        },
        //如果浏览器支持localStorage 则设置localStorage值，返回true ，否则返回false
        setLocalStorage: function (key, data) {
            if (this.supportsLocalStorage()) {
                localStorage.setItem(key, data);
                return true;
            }
            return false;
        },
        //如果浏览器支持localStorage 则移除localStorage值，返回true ，否则返回false
        removeLocalStorage: function (key) {
            if (this.supportsLocalStorage()) {
                localStorage.removeItem(key);
                return true;
            }
            return false;
        },
        //校验浏览器是否支持html sessionStorage
        supportsSessionStorage: function () {
            try {
                return 'sessionStorage' in window && window['sessionStorage'] !== null;
            } catch (e) {
                return false;
            }
        }
    };
})(jsTools, jQuery);

/*
js 相关的校验操作
*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.valite = {
        // 联系电话(手机/电话皆可)验证
        isPhone: function (value) {
            var mobile = /^(((13[0-9]{1})|(15[0-9]{1})|(147)|(177)|(18[0-9]{1}))+\d{8})$/;
            var tel = /^(\d{3}-(\d{8})|(\d{7}))$|^(\d+)$|^(\d{4}-(\d{7})|(\d{8}))$|^(\d{7,8})$/;
            return (tel.test(value) || mobile.test(value));
        },
        //IP 地址验证
        ip: function (value) {
            var reg = /^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$/;
            return (reg.test(value) && (RegExp.$1 < 256 && RegExp.$2 < 256 && RegExp.$3 < 256 && RegExp.$4 < 256));
        },
        //电话号码验证
        isTell: function (value) {
            var reg = /^(\d{3}-(\d{8})|(\d{7}))$|^(\d+)$|^(\d{4}-(\d{7})|(\d{8}))$|^(\d{7,8})$/;
            return reg.test(value);
        },
        //手机号码验证
        isMobile: function (value) {
            var reg = /^(((13[0-9]{1})|(15[0-9]{1})|(147)|(177)|(18[0-9]{1}))+\d{8})$/;
            return reg.test(value);
        },
        //护照编号验证
        isPassport: function (value) {
            var reg = /(P\d{7})|(G\d{8})/;
            return reg.test(value);
        },
        //只能输入中文
        isChinese: function (value) {
            var reg = /^[\u4e00-\u9fa5]+$/;
            return reg.test(value);
        },
        //只能包括中文字、英文字母、数字和下划线
        stringCheck: function (value) {
            var reg = /^[\u0391-\uFFE5\w]+$/
            return reg.test(value);
        },
        //只能为小数
        isFloat: function (value) {
            var reg = /^\d+\.?\d*$/g;
            return reg.test(value);
        },
        //只能为数字或字母
        chrnum: function (value) {
            var reg = /^([a-zA-Z0-9]+)$/;
            return reg.test(value);
        },
        //只能为正整数或者小数
        isNumOrFloat: function (value) {
            var reg = /^\d+(\.\d){0,1}\d*$/g;
            return reg.test(value);
        },
        //只能输入整数
        isNum: function (value) {
            var reg = /^\d+$/;
            return reg.test(value);
        },
        //身份证号码验证
        isIdCardNo: function (value) {
            return this.idCard.checkIdCardNo(value);
        },
        /**
        Luhm校验规则：16位银行卡号（19位通用）:
         1.将未带校验位的 15（或18）位卡号从右依次编号 1 到 15（18），位于奇数位号上的数字乘以 2。
         2.将奇位乘积的个十位全部相加，再加上所有偶数位上的数字。
         3.将加法和加上校验位能被 10 整除。
        @bankno为银行卡号 banknoInfo为显示提示信息的DIV或其他控件
        **/
        isBankCardNo: function (bankno) {
            if ($.trim(bankno).length > 0) {
                var lastNum = bankno.substr(bankno.length - 1, 1);//取出最后一位（与luhm进行比较）

                var first15Num = bankno.substr(0, bankno.length - 1);//前15或18位
                var newArr = new Array();
                for (var i = first15Num.length - 1; i > -1; i--) {    //前15或18位倒序存进数组
                    newArr.push(first15Num.substr(i, 1));
                }
                var arrJiShu = new Array();  //奇数位*2的积 <9
                var arrJiShu2 = new Array(); //奇数位*2的积 >9

                var arrOuShu = new Array();  //偶数位数组
                for (var j = 0; j < newArr.length; j++) {
                    if ((j + 1) % 2 == 1) {//奇数位
                        if (parseInt(newArr[j]) * 2 < 9)
                            arrJiShu.push(parseInt(newArr[j]) * 2);
                        else
                            arrJiShu2.push(parseInt(newArr[j]) * 2);
                    }
                    else //偶数位
                        arrOuShu.push(newArr[j]);
                }

                var jishu_child1 = new Array();//奇数位*2 >9 的分割之后的数组个位数
                var jishu_child2 = new Array();//奇数位*2 >9 的分割之后的数组十位数
                for (var h = 0; h < arrJiShu2.length; h++) {
                    jishu_child1.push(parseInt(arrJiShu2[h]) % 10);
                    jishu_child2.push(parseInt(arrJiShu2[h]) / 10);
                }

                var sumJiShu = 0; //奇数位*2 < 9 的数组之和
                var sumOuShu = 0; //偶数位数组之和
                var sumJiShuChild1 = 0; //奇数位*2 >9 的分割之后的数组个位数之和
                var sumJiShuChild2 = 0; //奇数位*2 >9 的分割之后的数组十位数之和
                var sumTotal = 0;
                for (var m = 0; m < arrJiShu.length; m++) {
                    sumJiShu = sumJiShu + parseInt(arrJiShu[m]);
                }

                for (var n = 0; n < arrOuShu.length; n++) {
                    sumOuShu = sumOuShu + parseInt(arrOuShu[n]);
                }

                for (var p = 0; p < jishu_child1.length; p++) {
                    sumJiShuChild1 = sumJiShuChild1 + parseInt(jishu_child1[p]);
                    sumJiShuChild2 = sumJiShuChild2 + parseInt(jishu_child2[p]);
                }
                //计算总和
                sumTotal = parseInt(sumJiShu) + parseInt(sumOuShu) + parseInt(sumJiShuChild1) + parseInt(sumJiShuChild2);

                //计算Luhm值
                var k = parseInt(sumTotal) % 10 == 0 ? 10 : parseInt(sumTotal) % 10;
                var luhm = 10 - k;

                if (lastNum == luhm) {
                    //$("#banknoInfo").html("Luhm验证通过");
                    return true;
                }
                else {
                    //$("#banknoInfo").html("银行卡号必须符合Luhm校验");
                    return false;
                }
            }
            return false;
        }
    };
})(jsTools, jQuery);

/*
js 身份证相关处理
*/
; !(function (jsTools, $, undefined) {
    "use strict";//设立"严格模式"
    jsTools.valite.idCard = {
        provinceAndCitys: {
            11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江",
            31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东",
            45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏",
            65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外"
        },

        powers: ["7", "9", "10", "5", "8", "4", "2", "1", "6", "3", "7", "9", "10", "5", "8", "4", "2"],

        parityBit: ["1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2"],

        //性别配置
        genders: { male: "男", female: "女" },
        //校验地址码
        checkAddressCode: function (addressCode) {
            var check = /^[1-9]\d{5}$/.test(addressCode);
            if (!check) return false;
            if (this.provinceAndCitys[parseInt(addressCode.substring(0, 2))]) {
                return true;
            } else {
                return false;
            }
        },
        //校验生日码
        checkBirthDayCode: function (birDayCode) {
            var check = /^[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))$/.test(birDayCode);
            if (!check) return false;
            var yyyy = parseInt(birDayCode.substring(0, 4), 10);
            var mm = parseInt(birDayCode.substring(4, 6), 10);
            var dd = parseInt(birDayCode.substring(6), 10);
            var xdata = new Date(yyyy, mm - 1, dd);
            if (xdata > new Date()) {
                return false;//生日不能大于当前日期
            } else if ((xdata.getFullYear() == yyyy) && (xdata.getMonth() == mm - 1) && (xdata.getDate() == dd)) {
                return true;
            } else {
                return false;
            }
        },
        //获取18位身份证号校验码
        getParityBit: function (idCardNo) {
            var id17 = idCardNo.substring(0, 17);
            var power = 0;
            for (var i = 0; i < 17; i++) {
                power += parseInt(id17.charAt(i), 10) * parseInt(this.powers[i]);
            }
            var mod = power % 11;
            return this.parityBit[mod];
        },
        //验证校检码
        checkParityBit: function (idCardNo) {
            var parityBit = idCardNo.charAt(17).toUpperCase();
            if (this.getParityBit(idCardNo) == parityBit) {
                return true;
            } else {
                return false;
            }
        },
        //校验身份证号
        checkIdCardNo: function (idCardNo) {
            //15位和18位身份证号码的基本校验
            var check = /^\d{15}|(\d{17}(\d|x|X))$/.test(idCardNo);
            if (!check) return false;
            //判断长度为15位或18位
            if (idCardNo.length == 15) {
                return this.check15IdCardNo(idCardNo);
            } else if (idCardNo.length == 18) {
                return this.check18IdCardNo(idCardNo);
            } else {
                return false;
            }
        },
        //校验15位的身份证号码
        check15IdCardNo: function (idCardNo) {
            //15位身份证号码的基本校验
            var check = /^[1-9]\d{7}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}$/.test(idCardNo);
            if (!check) return false;
            //校验地址码
            var addressCode = idCardNo.substring(0, 6);
            check = this.checkAddressCode(addressCode);
            if (!check) return false;
            //校验日期码
            var birDayCode = '19' + idCardNo.substring(6, 12);
            return this.checkBirthDayCode(birDayCode);
        },
        //校验18位的身份证号码
        check18IdCardNo: function (idCardNo) {
            //18位身份证号码的基本格式校验
            var check = /^[1-9]\d{5}[1-9]\d{3}((0[1-9])|(1[0-2]))((0[1-9])|([1-2][0-9])|(3[0-1]))\d{3}(\d|x|X)$/.test(idCardNo);
            if (!check) return false;
            //校验地址码
            var addressCode = idCardNo.substring(0, 6);
            check = this.checkAddressCode(addressCode);
            if (!check) return false;
            //校验日期码
            var birDayCode = idCardNo.substring(6, 14);
            check = this.checkBirthDayCode(birDayCode);
            if (!check) return false;
            //验证校检码
            return this.checkParityBit(idCardNo);
        },
        //将身份证号转化为15位
        changeId15: function (idCardNo) {
            if (idCardNo.length == 15) {
                return idCardNo;
            } else if (idCardNo.length == 18) {
                return idCardNo.substring(0, 6) + idCardNo.substring(8, 17);
            } else {
                return null;
            }
        },
        //将身份证号转化为18位
        changeId18: function (idCardNo) {
            if (idCardNo.length == 15) {
                var id17 = idCardNo.substring(0, 6) + '19' + idCardNo.substring(6);
                var parityBit = this.getParityBit(id17);
                return id17 + parityBit;
            } else if (idCardNo.length == 18) {
                return idCardNo;
            } else {
                return null;
            }
        },
        //转化为日期格式 yyyy-MM-dd
        formateDateCN: function (day) {
            var yyyy = day.substring(0, 4);
            var mm = day.substring(4, 6);
            var dd = day.substring(6);
            return yyyy + '-' + mm + '-' + dd;
        },
        /**
        获取性别(男、女)，出生日期（yyyy-MM-dd）,年龄
        //使用例子
        var icval = $(this).val();
        if ($.trim(icval).length > 0) {
            if (jsTools.valite.idCard.checkIdCardNo(icval)) {
                //配置1男，2女
                jsTools.valite.idCard.genders = { male: "1", female: "2" };
                var user = jsTools.valite.idCard.getIdCardInfo(icval);
                console.log(user.gender);
                $("#BirthDay").val(user.birthday);
                $("#rabLstSex input[type='radio'][value='" + user.gender + "']").attr("checked", "checked");
                $("#Age").val(user.age);
            }
        }
        **/
        getIdCardInfo: function (idCardNo) {
            var idCardInfo = {
                gender: "", //性别
                birthday: "", // 出生日期(yyyy-mm-dd)
                age: ""
            };
            if (idCardNo.length == 15) {
                //获取出生日期
                var aday = '19' + idCardNo.substring(6, 12);
                idCardInfo.birthday = this.formateDateCN(aday);
                //获取性别
                if (parseInt(idCardNo.charAt(14)) % 2 == 0) {
                    idCardInfo.gender = this.genders.female;
                } else {
                    idCardInfo.gender = this.genders.male;
                }
                //获取年龄
                var myDate = new Date();
                var month = myDate.getMonth() + 1;
                var day = myDate.getDate();
                var age = myDate.getFullYear() - idCardNo.substring(6, 8) - 1;
                var icMonth = idCardNo.substring(8, 10);
                if (icMonth < month || icMonth == month && idCardNo.substring(10, 12) <= day) {
                    age++;
                }
                idCardInfo.age = age;
            } else if (idCardNo.length == 18) {
                //获取出生日期
                var aday = idCardNo.substring(6, 14);
                idCardInfo.birthday = this.formateDateCN(aday);
                //获取性别
                if (parseInt(idCardNo.charAt(16)) % 2 == 0) {
                    idCardInfo.gender = this.genders.female;
                } else {
                    idCardInfo.gender = this.genders.male;
                }
                //获取年龄
                var myDate = new Date();
                var month = myDate.getMonth() + 1;
                var day = myDate.getDate();
                var age = myDate.getFullYear() - idCardNo.substring(6, 10) - 1;
                var icMonth = idCardNo.substring(10, 12);
                if (icMonth < month || icMonth == month && idCardNo.substring(12, 14) <= day) {
                    age++;
                }
                idCardInfo.age = age;
            }
            return idCardInfo;
        }
    };
})(jsTools, jQuery);