using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FancyFix.ThirdPartyPlatform.Filter
{
    public class ModelValidFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///忽略字段，用逗号隔开
        /// </summary>
        public string IgnoreField { get; set; }

        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var modelSate = actionContext.Controller.ViewData.ModelState;
            RemoveField(modelSate);//移除忽略字段
            if (!modelSate.IsValid)
            {
                foreach (var item in modelSate.Values)
                {
                    if (item.Errors.Count > 0 && !string.IsNullOrEmpty(item.Errors[0].ErrorMessage))
                    {
                        actionContext.Result = LayerAlertErrorAndReturn(item.Errors[0].ErrorMessage);
                        return;
                    }
                }
            }
            base.OnActionExecuting(actionContext);
        }

        /// <summary>
        /// 移除忽略字段
        /// </summary>
        /// <param name="modelState"></param>
        private void RemoveField(ModelStateDictionary modelState)
        {
            if (!string.IsNullOrEmpty(IgnoreField))
            {
                List<string> fieldlist = IgnoreField.Split(',').ToList();
                foreach (var field in fieldlist)
                {
                    if (field != "") modelState.Remove(field);
                }
            }
        }

        /// <summary>
        /// 输出错误弹窗
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public ContentResult LayerAlertErrorAndReturn(string message)
        {
            var result = new ContentResult();
            result.Content = "<script type=\"text/javascript\">window.history.back(-1);parent.layer?parent.layer.alert('" + message.Replace("'", @"\'") + "', {icon: 5}):alert('" + message.Replace("'", @"\'") + "');</script>";
            result.ContentEncoding = Encoding.UTF8;
            result.ContentType = "text/html;charset=UTF-8";
            return result;
        }
    }
}
