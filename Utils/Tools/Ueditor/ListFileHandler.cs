using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// FileManager 的摘要说明
/// </summary>
public class ListFileManager : Handler
{
    enum ResultState
    {
        Success,
        InvalidParam,
        AuthorizError,
        IOError,
        PathNotFound
    }

    private int Start;
    private int Size;
#pragma warning disable CS0649 // 从未对字段“ListFileManager.Total”赋值，字段将一直保持其默认值 0
    private int Total;
#pragma warning restore CS0649 // 从未对字段“ListFileManager.Total”赋值，字段将一直保持其默认值 0
    private ResultState State;
    private String PathToList;
    private String[] FileList;
    private String[] SearchExtensions;

    public ListFileManager(HttpContext context, string pathToList, string[] searchExtensions)
        : base(context)
    {
        this.SearchExtensions = searchExtensions.Select(x => x.ToLower()).ToArray();
        this.PathToList = pathToList;
    }

    public override void Process()
    {
        try
        {
            Start = String.IsNullOrEmpty(Request["start"]) ? 0 : Convert.ToInt32(Request["start"]);
           Size = String.IsNullOrEmpty(Request["size"]) ? Config.GetInt("imageManagerListSize") : Convert.ToInt32(Request["size"]);
         //   Size = Config.GetInt("imageManagerListSize") == 0 ? 20 : Config.GetInt("imageManagerListSize");
       
        }
        catch (FormatException)
        {
            State = ResultState.InvalidParam;
            WriteResult();
            return;
        }
        var buildingList = new List<String>();
        try
        {
            // var localPath = Server.MapPath(PathToList);

            buildingList.AddRange(Directory.GetFiles(PathToList, "*", SearchOption.AllDirectories)
                .Where(x => SearchExtensions.Contains(Path.GetExtension(x).ToLower()) && FancyFix.Tools.Usual.Utils.IsOriginPic(x))
                .Select(x => FancyFix.Tools.Usual.Utils.GetLastFolderName(PathToList) + "/" + x.Substring(PathToList.Length).Replace("\\", "/")));

            FileList = buildingList.OrderByDescending(x => new FileInfo(x).FullName).Skip(Start).Take(Size).ToArray();
        }
        catch (UnauthorizedAccessException)
        {
            State = ResultState.AuthorizError;
        }
        catch (DirectoryNotFoundException)
        {
            State = ResultState.PathNotFound;
        }
        catch (IOException)
        {
            State = ResultState.IOError;
        }
        finally
        {
            WriteResult();
        }
    }


    private void WriteResult()
    {
        WriteJson(new
        {
            state = GetStateString(),
            list = FileList == null ? null : FileList.Select(x => new { url = x }),
            start = Start,
            size = Size,
            total = Total
        });
    }

    private string GetStateString()
    {
        switch (State)
        {
            case ResultState.Success:
                return "SUCCESS";
            case ResultState.InvalidParam:
                return "参数不正确";
            case ResultState.PathNotFound:
                return "路径不存在";
            case ResultState.AuthorizError:
                return "文件系统权限不足";
            case ResultState.IOError:
                return "文件系统读取错误";
        }
        return "未知错误";
    }
}
