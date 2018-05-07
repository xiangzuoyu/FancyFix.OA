using FancyFix.OA.Base;
using FancyFix.OA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FancyFix.OA.api
{
    public class MenuController : BaseApiController
    {
        List<Mng_MenuClass> list = null;

        [HttpGet]
        public object GetMenuList()
        {
            Tree lJson = new Tree();
            //获取OA管理类
            list = Bll.BllSys_MenuClass.Instance().GetListByParentId(0, false).ToList();
            if (list != null && list.Count > 0)
            {
                var parent = list.Find(o => o.ParId == 0);
                if (parent != null)
                {
                    int id = parent.Id;
                    lJson.id = id;
                    lJson.text = parent.ClassName;
                    lJson.state = "open";
                    CreatTree(lJson, id);//子节点递归
                }
            }
            else
            {
                return FormatOutput(false, "无菜单列表");
            }
            return FormatOutput(true, lJson);
        }

        void CreatTree(Tree parentJson, int parentId)
        {
            var childList = list.Where(o => o.ParId == parentId).OrderBy(o => o.Sequence).AsEnumerable();
            if (childList != null && childList.Any())
            {
                foreach (var item in childList)
                {
                    Tree lJson = new Tree();
                    int id = item.Id;

                    //检测权限
                    if (CheckPermission(id))
                    {
                        lJson.id = id;
                        lJson.text = item.ClassName.ToString();
                        if (!string.IsNullOrEmpty(item.Url))
                        {
                            lJson.attributes.url = item.Url;
                        }
                        if (item.ChildNum.Value > 0)
                        {
                            lJson.children = new List<Tree>();
                            CreatTree(lJson, id);
                        }
                        else
                        {
                            lJson.state = "open";
                        }
                        parentJson.children.Add(lJson);
                    }
                }
            }
            else
            {
                parentJson.state = "open";
            }
        }

        class Tree
        {
            int _id;
            string _text = string.Empty;
            string _state = "closed";
            AttrList _attributes = new AttrList();
            List<Tree> _children = new List<Tree>();

            public int id
            {
                set { _id = value; }
                get { return _id; }
            }
            public string text
            {
                set { _text = value; }
                get { return _text; }
            }
            public string state
            {
                set { _state = value; }
                get { return _state; }
            }
            public AttrList attributes
            {
                set { _attributes = value; }
                get { return _attributes; }
            }
            public List<Tree> children
            {
                set { _children = value; }
                get { return _children; }
            }
        }

        class AttrList
        {
            string _url = string.Empty;

            public string url
            {
                set { _url = value.Replace("\"", "&quot;"); }
                get { return _url; }
            }
        }
    }
}
