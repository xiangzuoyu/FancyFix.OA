
///
///webuploaderext 调用说明
///by:willian date:2016-10-27
///

官网：http://fex.baidu.com/webuploader/getting-started.htm
js引用 
/dist/webuploader.min.js
/dist/webuploaderext.min.js

正常配置
$("#myUploader").powerWebUpload({
            auto: true,  fileNumLimit: 5, formData: {
                uptype: "product",//上传类型
                token: "token"//验证令牌 功能待完善
            },
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png', //文件类型
               // mimeTypes: 'image/*'
            },
            fileSingleSizeLimit: 5,//单个文件大小限制（单位M，默认5）
  		fileNumLimit: 1,//文件总个数限制（默认为1）
        });

requirejs 配置


define('upload',['jquery','webuploader'],function($,WebUploader){
     
     window['WebUploader']=WebUploader ;
   require(['webuploaderext'],function(){
	   
	    $("#myUploader").powerWebUpload({
            auto: true, fileNumLimit: 5, formData: {
               uptype: "product",//上传类型
                token: "token"//验证令牌 功能待完善
            },
            accept: {
                title: 'Images',
                extensions: 'gif,jpg,jpeg,bmp,png',//文件类型
                // mimeTypes: 'image/*'
            },  
            fileSingleSizeLimit: 5,//单个文件大小限制（单位M，默认5）
  		fileNumLimit: 1,//文件总个数限制（默认为1）
        });
   });
备注：
上传成功隐藏输入框name名称分别为 
fileUrl url地址
fileName 本地上传文件名

后台获取参数名时需特别注意

