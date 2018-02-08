(function ($, window) {

    
    var applicationPath = "http://www.welfulloutdoors.com";
    var imgPath = "http://img.welfulloutdoors.com";
    //window.applicationPath === "" ? "" : window.applicationPath || "../..";
    function SuiJiNum() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    function initWebUpload(item, options) {
        if (!WebUploader.Uploader.support()) {
            var error = "上传控件不支持您的浏览器！请尝试升级flash版本或者使用Chrome引擎的浏览器。<a target='_blank' href='http://se.360.cn'>下载页面</a>";
            if (window.console) {
                window.console.log(error);
            }
            $(item).text(error);
            return;
        }
        //创建默认参数
        var defaults = {
            auto: true,
            hiddenInputId: "uploadifyHiddenInputId", // input hidden id
            onAllComplete: function (event) { }, // 当所有file都上传后执行的回调函数
            onComplete: function (event) { },// 每上传一个file的回调函数
            innerOptions: {},
            fileNumLimit: 1,//验证文件总数量, 超出则不允许加入队列
            fileSizeLimit: undefined,//验证文件总大小是否超出限制, 超出则不允许加入队列。
            fileSingleSizeLimit: 5,//验证单个文件大小是否超出限制, 超出则不允许加入队列
            PostbackHold: false,
            formData: {
                uptype: "product",
                token: "token"
            },
            accept: undefined,
        };
        var opts = $.extend(defaults, options);
        var hdFileData = $("#" + opts.hiddenInputId);
        var target = $(item);//容器
        var pickerid = "";
        if (typeof guidGenerator36 != 'undefined')//给一个唯一ID
            pickerid = guidGenerator36();
        else
            pickerid = (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
        var uploaderStrdiv = '<div class="webuploader">'
        // debugger
        if (opts.auto) {
            uploaderStrdiv =
            '<div id="Uploadthelist" class="uploader-list"></div>' +
            '<div class="btns">' +
            '<div id="' + pickerid + '">Select files</div>' +
            '<p>Formats supported: '+opts.accept.extensions+', less than '+opts.fileSingleSizeLimit+'M</p></div>'
        } else {
            uploaderStrdiv =
            '<div  class="uploader-list"></div>' +
            '<div class="btns">' +
            '<div id="' + pickerid + '">Select files</div>' +
            '<button class="webuploadbtn">Start upload</button>' +
             '<p>Formats supported: '+opts.accept.extensions+', less than '+opts.fileSingleSizeLimit+'M,Max'+opts.fileNumLimit+' files</p></div>'
        }
        uploaderStrdiv += '<div style="display:none" class="UploadhiddenInput" >\
                         </div>'
        uploaderStrdiv += '</div>';
        target.append(uploaderStrdiv);

        var $list = target.find('.uploader-list'),
             $btn = target.find('.webuploadbtn'),//手动上传按钮备用
             state = 'pending',
             $hiddenInput = target.find('.UploadhiddenInput'),
             uploader;
        var jsonData = {
            fileList: []
        };

        var webuploaderoptions = $.extend({
            // swf文件路径
            swf: imgPath + '/img/webuploader/dist/Uploader.swf',
            // 文件接收服务端。
            server: imgPath + '/api/upload/UploadFile',
            deleteServer: imgPath + '/api/upload/DelFile',
            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: '#' + pickerid,
            //不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
            resize: false,
            fileNumLimit: opts.fileNumLimit,
            fileSizeLimit: opts.fileSizeLimit * 1024 * 1024, //单位M
            fileSingleSizeLimit: opts.fileSingleSizeLimit * 1024 * 1024,//单位M
            formData: opts.formData,
            accept: opts.accept,
        },
        opts.innerOptions);
        var uploader = WebUploader.create(webuploaderoptions);

        //回发时还原hiddenfiled的保持数据
        var fileDataStr = hdFileData.val();
        if (fileDataStr && opts.PostbackHold) {
            jsonData = JSON.parse(fileDataStr);
            $.each(jsonData.fileList, function (index, fileData) {
                var newid = SuiJiNum();
                fileData.queueId = newid;
                $list.append('<div id="' + newid + '" class="item">' +
                '<div class="info">' + fileData.name + '</div>' +
                '<div class="state">uploaded</div>' +
                '<div class="del"></div>' +
                '</div>');
            });
            hdFileData.val(JSON.stringify(jsonData));
        }



        uploader.on('fileQueued', function (file) {
            //   debugger;
            var $item = $('<div id="' + $(item)[0].id + file.id + '" class="item">' +
                 '<span class="webuploadinfo">' + file.name + '</span>' +
                 '<span class="webuploadstate">wait upload...</span>' +
                 '<div class="webuploadDelbtn">delete</div><br />' +
             '</div>').appendTo($list);

            if (opts.auto) {
                uploader.upload();
            }
        });

        uploader.on('error', function (code) {
            var res="An unknown error occurred";
            switch(code)
            {
                case "Q_EXCEED_NUM_LIMIT":
                    res="File number exceeds limit";
                    break;
                case "Q_EXCEED_SIZE_LIMIT":
                    res="File size exceeds limit";
                    break
                case "Q_TYPE_DENIED":
                    res="File type error";
                    break;
            }
           alert( 'Error: ' + res );
        });



        uploader.on('uploadProgress', function (file, percentage) {//进度条事件
            var $li = target.find('#' + $(item)[0].id + file.id),
                $percent = $li.find('.progress .bar');

            // 避免重复创建
            if (!$percent.length) {
                $percent = $('<span class="progress">' +
                    '<span  class="percentage"><span class="text"></span>' +
                  '<span class="bar" role="progressbar" style="width: 0%">' +
                  '</span></span>' +
                '</span>').appendTo($li).find('.bar');
            }

            $li.find('span.webuploadstate').html('uploading');
            $li.find(".text").text(Math.round(percentage * 100) + '%');
            $percent.css('width', percentage * 100 + '%');
        });

        uploader.on('uploadSuccess', function (file, response) {//上传成功事件
        
            if (response.code === 0) {
                target.find('#' + $(item)[0].id + file.id).find('span.webuploadstate').html(response.msg);
            } else {
                target.find('#' + $(item)[0].id + file.id).find('span.webuploadstate').html('success');
                $hiddenInput.append('<input type="text" id="hiddenInput' + $(item)[0].id + file.id + '" name="fileUrl" class="hiddenInput" value="' + response.url + '" /><input type="text" id="hiddenInputName' + $(item)[0].id + file.id + '" name="fileName" class="hiddenInput" value="' + response.name + '" />')
                if(opts.onComplete!==undefined)
                    opts.onComplete(file,response);
            }
        });

        uploader.on('uploadError', function (file) {
            target.find('#' + $(item)[0].id + file.id).find('span.webuploadstate').html('failed');
        });

        uploader.on('uploadComplete', function (file) {//全部完成事件
            target.find('#' + $(item)[0].id + file.id).find('.progress').fadeOut();

        });

        uploader.on('all', function (type) {
            if (type === 'startUpload') {
                state = 'uploading';
            } else if (type === 'stopUpload') {
                state = 'paused';
            } else if (type === 'uploadFinished') {
                state = 'done';
            }

            if (state === 'uploading') {
                $btn.text('paused');
            } else {
                $btn.text('start');
            }
        });

        //删除时执行的方法
        uploader.on('fileDequeued', function (file) {
            //  debugger
            var fullName = $("#hiddenInput" + $(item)[0].id + file.id).val();
            $("#" + $(item)[0].id + file.id).remove();
            $("#hiddenInput" + $(item)[0].id + file.id).remove();

        })

        //多文件点击上传的方法
        $btn.on('click', function () {
            if (state === 'uploading') {
                uploader.stop();
            } else {
                uploader.upload();
            }
        });

        //删除
        $list.on("click", ".webuploadDelbtn", function () {
            //debugger
            var $ele = $(this);
            var id = $ele.parent().attr("id");
            var id = id.replace($(item)[0].id, "");

            var file = uploader.getFile(id);
            uploader.removeFile(file);
        });

    }

    $.fn.powerWebUpload = function (options) {
        var ele = this;
         var casspath = imgPath + "/img/webuploader/dist/webuploader.css";
            $("<link>").attr({ rel: "stylesheet", type: "text/css", href: casspath }).appendTo("head");
       if (typeof WebUploader == 'undefined') {
            var casspath = imgPath + "/img/webuploader/dist/webuploader.css";
            $("<link>").attr({ rel: "stylesheet", type: "text/css", href: casspath }).appendTo("head");
            var jspath = imgPath + "/img/webuploader/dist/webuploader.min.js";
            $.getScript(jspath).done(function () {

                initWebUpload(ele, options);
            })
            .fail(function () {
                alert("请检查webuploader的路径是否正确!")
            });

        }
        else {
            initWebUpload(ele, options);
        }
    }
})(jQuery, window);