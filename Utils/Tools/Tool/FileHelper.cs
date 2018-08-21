using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using FancyFix.Tools.Config;
using FancyFix.Tools.Tool;

namespace Tools.Tool
{
    public class FileHelper
    {
        #region 构造函数
        private bool _alreadyDispose = false;
        public FileHelper()
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }
        ~FileHelper()
        {
            Dispose(); ;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            //if (isDisposing)
            //{
            //    if (xml != null)
            //    {
            //        xml = null;
            //    }
            //}
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region 取得文件后缀名
        /****************************************
         * 函数名称：GetPostfixStr
         * 功能说明：取得文件后缀名
         * 参    数：filename:文件名称
         * 调用示列：
         *           string filename = "aaa.aspx";        
         *           string s = EC.FileObj.GetPostfixStr(filename);         
        *****************************************/
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf("."); //从0 开始数
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }
        #endregion

        #region 取得文件名
        /****************************************
         * 函数名称：GetPostfixStr
         * 功能说明：取得文件后缀名
         * 参    数：filename:文件名称
         * 调用示列：
         *           string filename = "aaa.aspx";        
         *           string s = EC.FileObj.GetPostfixStr(filename);         
        *****************************************/
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPicName(string filename)
        {
            int start = filename.LastIndexOf("\\") + 1;
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start); // Substring start和length
            return postfix;
        }
        #endregion

        #region 写文件
        /****************************************
         * 函数名称：WriteFile
         * 功能说明：当文件不存时，则创建文件，并追加文件
         * 参    数：Path:文件路径,Strings:文本内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string Strings = "这是我写的内容啊";
         *           EC.FileObj.WriteFile(Path,Strings);
        *****************************************/
        /// <summary>
        /// 写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public static void WriteFile(string Path, string Strings)
        {

            if (!System.IO.File.Exists(Path))
            {
                //Directory.CreateDirectory(Path);
                System.IO.FileStream f = System.IO.File.Create(Path);
                f.Close();
                f.Dispose();
            }
            System.IO.StreamWriter f2 = new System.IO.StreamWriter(Path, true, System.Text.Encoding.GetEncoding("gb2312"));
            f2.Write(Strings);
            f2.Close();
            f2.Dispose();
        }
        #endregion

        #region 读文件
        /****************************************
         * 函数名称：ReadFile
         * 功能说明：读取文本内容
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");       
         *           string s = EC.FileObj.ReadFile(Path);
        *****************************************/
        /// <summary>
        /// 读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));

                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        #endregion

        #region 追加文件
        /****************************************
         * 函数名称：FileAdd
         * 功能说明：追加文件内容
         * 参    数：Path:文件路径,strings:内容
         * 调用示列：
         *           string Path = Server.MapPath("Default2.aspx");     
         *           string Strings = "新追加内容";
         *           EC.FileObj.FileAdd(Path, Strings);
        *****************************************/
        /// <summary>
        /// 追加文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="strings">内容</param>
        public static void FileAdd(string Path, string strings)
        {
            StreamWriter sw = File.AppendText(Path);
            sw.Write(strings);
            sw.Flush();
            sw.Close();
            sw.Dispose();
        }
        #endregion

        #region 拷贝文件
        /****************************************
         * 函数名称：FileCoppy
         * 功能说明：拷贝文件
         * 参    数：OrignFile:原始文件,NewFile:新文件路径
         * 调用示列：
         *           string OrignFile = Server.MapPath("Default2.aspx");     
         *           string NewFile = Server.MapPath("Default3.aspx");
         *           EC.FileObj.FileCoppy(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="OrignFile">原始文件</param>
        /// <param name="NewFile">新文件路径</param>
        public static void FileCoppy(string OrignFile, string NewFile)
        {
            File.Copy(OrignFile, NewFile, true);
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="OrignFile">原始文件</param>
        /// <param name="NewFile">新文件路径</param>
        public static bool FileCoppy2(string OrignFile, string NewFile)
        {
            try
            {
                File.Copy(OrignFile, NewFile, true);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 删除文件
        /****************************************
         * 函数名称：FileDel
         * 功能说明：删除文件
         * 参    数：Path:文件路径
         * 调用示列：
         *           string Path = Server.MapPath("Default3.aspx");    
         *           EC.FileObj.FileDel(Path);
        *****************************************/
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="Path">路径</param>
        public static void FileDel(string Path)
        {
            File.Delete(Path);
        }
        #endregion

        #region 移动文件
        /****************************************
         * 函数名称：FileMove
         * 功能说明：移动文件
         * 参    数：OrignFile:原始路径,NewFile:新文件路径
         * 调用示列：
         *            string OrignFile = Server.MapPath("../说明.txt");    
         *            string NewFile = Server.MapPath("../../说明.txt");
         *            EC.FileObj.FileMove(OrignFile, NewFile);
        *****************************************/
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="OrignFile">原始路径</param>
        /// <param name="NewFile">新路径</param>
        public static void FileMove(string OrignFile, string NewFile)
        {
            File.Move(OrignFile, NewFile);
        }
        #endregion

        #region 在当前目录下创建目录
        /****************************************
         * 函数名称：FolderCreate
         * 功能说明：在当前目录下创建目录
         * 参    数：OrignFolder:当前目录,NewFloder:新目录
         * 调用示列：
         *           string OrignFolder = Server.MapPath("test/");    
         *           string NewFloder = "new";
         *           EC.FileObj.FolderCreate(OrignFolder, NewFloder); 
        *****************************************/
        /// <summary>
        /// 在当前目录下创建目录
        /// </summary>
        /// <param name="OrignFolder">当前目录</param>
        /// <param name="NewFloder">新目录</param>
        public static void FolderCreate(string OrignFolder, string NewFloder)
        {
            Directory.SetCurrentDirectory(OrignFolder);
            Directory.CreateDirectory(NewFloder);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="Path"></param>
        public static void FolderCreate(string Path)
        {
            // 判断目标目录是否存在如果不存在则新建之
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);
        }

        #endregion

        #region 创建目录
        public static void FileCreate(string Path)
        {
            FileInfo CreateFile = new FileInfo(Path); //创建文件 
            if (!CreateFile.Exists)
            {
                FileStream FS = CreateFile.Create();
                FS.Close();
            }
        }
        #endregion

        #region 递归删除文件夹目录及文件
        /****************************************
         * 函数名称：DeleteFolder
         * 功能说明：递归删除文件夹目录及文件
         * 参    数：dir:文件夹路径
         * 调用示列：
         *           string dir = Server.MapPath("test/");  
         *           EC.FileObj.DeleteFolder(dir);       
        *****************************************/
        /// <summary>
        /// 递归删除文件夹目录及文件
        /// </summary>
        /// <param name="dir"></param>  
        /// <returns></returns>
        public static void DeleteFolder(string dir)
        {
            if (Directory.Exists(dir)) //如果存在这个文件夹删除之 
            {
                foreach (string d in Directory.GetFileSystemEntries(dir))
                {
                    if (File.Exists(d))
                        File.Delete(d); //直接删除其中的文件                        
                    else
                        DeleteFolder(d); //递归删除子文件夹 
                }
                Directory.Delete(dir, true); //删除已空文件夹                 
            }
        }
        #endregion

        #region 将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
        /****************************************
         * 函数名称：CopyDir
         * 功能说明：将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
         * 参    数：srcPath:原始路径,aimPath:目标文件夹
         * 调用示列：
         *           string srcPath = Server.MapPath("test/");  
         *           string aimPath = Server.MapPath("test1/");
         *           EC.FileObj.CopyDir(srcPath,aimPath);   
        *****************************************/
        /// <summary>
        /// 指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
        #endregion

        #region 获取指定文件夹下所有子目录及文件(树形)
        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(树形)
         * 参    数：Path:详细路径
         * 调用示列：
         *           string strDirlist = Server.MapPath("templates");       
         *           this.Literal1.Text = EC.FileObj.GetFoldAll(strDirlist);  
        *****************************************/
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件
        /// </summary>
        /// <param name="Path">详细路径</param>
        public static string GetFoldAll(string Path)
        {
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str);
            return str;
        }

        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="Rn">用于迭加的传入值,一般为空</param>
        /// <returns></returns>
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn)//递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录
            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                if (nLevel == 0)
                {
                    Rn += "├";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│&nbsp;";
                    }
                    Rn += _s + "├";
                }
                Rn += "<b>" + dirinfo.Name.ToString() + "</b><br />";
                FileInfo[] fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (FileInfo fInfo in fileInfo)
                {
                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│&nbsp;";
                        }
                        Rn += _f + "│&nbsp;├";
                    }
                    Rn += fInfo.Name.ToString() + " <br />";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn);


            }
            return Rn;
        }



        /****************************************
         * 函数名称：GetFoldAll(string Path)
         * 功能说明：获取指定文件夹下所有子目录及文件(下拉框形)
         * 参    数：Path:详细路径
         * 调用示列：
         *            string strDirlist = Server.MapPath("templates");      
         *            this.Literal2.Text = EC.FileObj.GetFoldAll(strDirlist,"tpl","");
        *****************************************/
        /// <summary>
        /// 获取指定文件夹下所有子目录及文件(下拉框形)
        /// </summary>
        /// <param name="Path">详细路径</param>
        ///<param name="DropName">下拉列表名称</param>
        ///<param name="tplPath">默认选择模板名称</param>
        public static string GetFoldAll(string Path, string DropName, string tplPath)
        {
            string strDrop = "<select name=\"" + DropName + "\" id=\"" + DropName + "\"><option value=\"\">--请选择详细模板--</option>";
            string str = "";
            DirectoryInfo thisOne = new DirectoryInfo(Path);
            str = ListTreeShow(thisOne, 0, str, tplPath);
            return strDrop + str + "</select>";

        }

        /// <summary>
        /// 获取指定文件夹下所有子目录及文件函数
        /// </summary>
        /// <param name="theDir">指定目录</param>
        /// <param name="nLevel">默认起始值,调用时,一般为0</param>
        /// <param name="Rn">用于迭加的传入值,一般为空</param>
        /// <param name="tplPath">默认选择模板名称</param>
        /// <returns></returns>
        public static string ListTreeShow(DirectoryInfo theDir, int nLevel, string Rn, string tplPath)//递归目录 文件
        {
            DirectoryInfo[] subDirectories = theDir.GetDirectories();//获得目录

            foreach (DirectoryInfo dirinfo in subDirectories)
            {

                Rn += "<option value=\"" + dirinfo.Name.ToString() + "\"";
                if (tplPath.ToLower() == dirinfo.Name.ToString().ToLower())
                {
                    Rn += " selected ";
                }
                Rn += ">";

                if (nLevel == 0)
                {
                    Rn += "┣";
                }
                else
                {
                    string _s = "";
                    for (int i = 1; i <= nLevel; i++)
                    {
                        _s += "│&nbsp;";
                    }
                    Rn += _s + "┣";
                }
                Rn += "" + dirinfo.Name.ToString() + "</option>";


                FileInfo[] fileInfo = dirinfo.GetFiles();   //目录下的文件
                foreach (FileInfo fInfo in fileInfo)
                {
                    Rn += "<option value=\"" + dirinfo.Name.ToString() + "/" + fInfo.Name.ToString() + "\"";
                    if (tplPath.ToLower() == fInfo.Name.ToString().ToLower())
                    {
                        Rn += " selected ";
                    }
                    Rn += ">";

                    if (nLevel == 0)
                    {
                        Rn += "│&nbsp;├";
                    }
                    else
                    {
                        string _f = "";
                        for (int i = 1; i <= nLevel; i++)
                        {
                            _f += "│&nbsp;";
                        }
                        Rn += _f + "│&nbsp;├";
                    }
                    Rn += fInfo.Name.ToString() + "</option>";
                }
                Rn = ListTreeShow(dirinfo, nLevel + 1, Rn, tplPath);


            }
            return Rn;
        }
        #endregion

        #region 获取文件夹大小
        /****************************************
         * 函数名称：GetDirectoryLength(string dirPath)
         * 功能说明：获取文件夹大小
         * 参    数：dirPath:文件夹详细路径
         * 调用示列：
         *           string Path = Server.MapPath("templates"); 
         *           Response.Write(EC.FileObj.GetDirectoryLength(Path));       
        *****************************************/
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        #endregion

        #region 获取指定文件详细属性
        /****************************************
         * 函数名称：GetFileAttibe(string filePath)
         * 功能说明：获取指定文件详细属性
         * 参    数：filePath:文件详细路径
         * 调用示列：
         *           string file = Server.MapPath("robots.txt");  
         *            Response.Write(EC.FileObj.GetFileAttibe(file));         
        *****************************************/
        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            System.IO.FileInfo objFI = new System.IO.FileInfo(filePath);
            str += "详细路径:" + objFI.FullName + "<br>文件名称:" + objFI.Name + "<br>文件长度:" + objFI.Length.ToString() + "字节<br>创建时间" + objFI.CreationTime.ToString() + "<br>最后访问时间:" + objFI.LastAccessTime.ToString() + "<br>修改时间:" + objFI.LastWriteTime.ToString() + "<br>所在目录:" + objFI.DirectoryName + "<br>扩展名:" + objFI.Extension;
            return str;
        }
        #endregion

        #region CompareSingleIp
        public static bool CompareSingleIp(string ip, string comip)
        {
            string[] ipsort = ip.Split('.');
            string[] comipsort = comip.Split('.');

            for (int i = 0; i < 4; i++)
            {
                int ipys = Convert.ToInt32(ipsort[i].ToString());
                int comipys = Convert.ToInt32(comipsort[i].ToString());

                if (ipys < comipys)
                {
                    return true;

                }
                if (ipys > comipys)
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region  CompareDouIp
        public static string CompareDouIp(string ip, string dbip)
        {
            string[] ipcsz = ip.Split('-');
            string[] dbipcsz = dbip.Split('-');
            string allip = string.Empty;
            if (CompareSingleIp(ipcsz[1], dbipcsz[0]))
            {
                allip = "next";
            }
            else if (CompareSingleIp(dbipcsz[1], ipcsz[0]))
            {
                allip = "pre";
            }
            else if (!CompareSingleIp(dbipcsz[1], ipcsz[1]) && !CompareSingleIp(ipcsz[1], dbipcsz[0]) && CompareSingleIp(ipcsz[0], dbipcsz[0]))
            {
                allip = ipcsz[0] + "-" + dbipcsz[1];
            }
            else if (!CompareSingleIp(dbipcsz[1], ipcsz[1]) && !CompareSingleIp(ipcsz[1], dbipcsz[0]) && !CompareSingleIp(ipcsz[0], dbipcsz[0]))
            {
                allip = dbipcsz[0] + "-" + dbipcsz[1];

            }
            else if (CompareSingleIp(dbipcsz[1], ipcsz[1]) && !CompareSingleIp(dbipcsz[1], ipcsz[0]) && CompareSingleIp(ipcsz[0], dbipcsz[0]))
            {
                allip = ipcsz[0] + "-" + ipcsz[1];
            }
            else if (CompareSingleIp(dbipcsz[1], ipcsz[1]) && !CompareSingleIp(dbipcsz[1], ipcsz[0]) && !CompareSingleIp(ipcsz[0], dbipcsz[0]))
            {
                allip = dbipcsz[0] + "-" + ipcsz[1];
            }

            return allip;
        }
        #endregion

        #region  CompareBunchIp
        public static string CompareBunchIp(string ip, string dbip)
        {
            string[] ipjg = ip.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string temp = string.Empty;
            string iptemp = string.Empty;
            string qjip = dbip;
            foreach (string ipv in ipjg)
            {
                string jg = CompareDouIp(ipv, qjip);
                if (jg == "next")
                {
                    temp += ipv + "|";
                }
                else if (jg == "pre")
                {
                    iptemp = qjip;
                    qjip = ipv;
                    temp += iptemp + "|";
                }
                else
                {
                    qjip = jg;
                }
            }
            temp += qjip + "|";
            return temp;
        }
        #endregion

        #region CompareSingleIpNew
        public static bool CompareSingleIpNew(string ip, string comip)
        {
            long ipnum = IpAddress(ip);
            long comipnum = IpAddress(comip);
            if (comipnum > ipnum + 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region  CompareDouIpNew
        public static string CompareDouIpNew(string ip, string dbip)
        {
            string[] ipcsz = ip.Split('-');
            string[] dbipcsz = dbip.Split('-');
            string allip = string.Empty;
            if (CompareSingleIpNew(ipcsz[1], dbipcsz[0]))
            {
                allip = "next";
            }
            else if (CompareSingleIpNew(dbipcsz[1], ipcsz[0]))
            {
                allip = "pre";
            }
            else if (!CompareSingleIpNew(dbipcsz[1], ipcsz[0]) && !CompareSingleIpNew(ipcsz[1], dbipcsz[0]))
            {
                string ip1 = IpAddress(dbipcsz[0]) <= IpAddress(ipcsz[0]) ? dbipcsz[0] : ipcsz[0];
                string ip2 = IpAddress(dbipcsz[1]) >= IpAddress(ipcsz[1]) ? dbipcsz[1] : ipcsz[1];
                allip = ip1 + "-" + ip2;
            }
            return allip;
        }
        #endregion

        #region  CompareBunchIpNew
        public static string CompareBunchIpNew(string ip, string dbip)
        {
            string[] ipjg = ip.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            string temp = string.Empty;
            string iptemp = string.Empty;
            string qjip = dbip;
            foreach (string ipv in ipjg)
            {
                string jg = CompareDouIpNew(ipv, qjip);
                if (jg == "next")
                {
                    temp += ipv + "|";
                }
                else if (jg == "pre")
                {
                    iptemp = qjip;
                    qjip = ipv;
                    temp += iptemp + "|";
                }
                else
                {
                    qjip = jg;
                }
            }
            temp += qjip + "|";
            return temp;
        }
        #endregion

        #region  IpAddress
        private static long IpAddress(string IP)
        {
            long ipcode = 0;
            if (IP != "")
            {
                string[] ipn = IP.Split('.');
                for (int i = 0; i < ipn.Length; i++)
                {
                    ipcode += int.Parse(ipn[i]);
                    if (i < ipn.Length - 1)
                    {
                        ipcode <<= 8;
                    }
                }
            }
            return ipcode;
        }
        #endregion

        #region ipToLong
        private static long ipToLong(String strIP)
        {
            //int j = 0;
            //int i = 0;
            long[] ip = new long[4];
            int position1 = strIP.IndexOf(".");
            int position2 = strIP.IndexOf(".", position1 + 1);
            int position3 = strIP.IndexOf(".", position2 + 1);
            ip[0] = long.Parse(strIP.Substring(0, position1));
            ip[1] = long.Parse(strIP.Substring(position1 + 1, position2 - position1 - 1));
            ip[2] = long.Parse(strIP.Substring(position2 + 1, position3 - position2 - 1));
            ip[3] = long.Parse(strIP.Substring(position3 + 1));
            return (ip[0] << 24) + (ip[1] << 16) + (ip[2] << 8) + ip[3];
        }
        #endregion

        #region IsCompanyIpOne
        public static bool IsCompanyIpOne(string ip, string ipback)
        {
            long ipnum = IpAddress(ip);
            long ipbacknum = IpAddress(ipback);
            long re = ipbacknum > ipnum ? (ipbacknum - ipnum) : (ipnum - ipbacknum);
            if (re == 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        #endregion

        #region 验证并保存上传的文件到指定文件夹
        /// <summary>
        /// 验证并保存上传的文件到指定文件夹
        /// </summary>
        /// <param name="file"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ValicationAndSaveFileToPath(HttpPostedFileBase file, string filePath)
        {
            if (file == null)
                return "上传文件为空";

            try
            {
                var size = file.ContentLength;
                int maxFileSize = UploadProvice.Instance().Settings["file"].MaxFileSize;
                //判断文件大小和格式
                if (size > maxFileSize)
                    return "上传的文件太大";

                if (!CheckFilesRealFormat.ValidationFile(file))
                    return "上传的文件格式不正确";

                file.SaveAs(filePath);
                return "0";
            }
            catch (Exception ex)
            {
                return "上传错误！";
            }


        }


        #endregion
    }
}
