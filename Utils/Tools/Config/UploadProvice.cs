using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace FancyFix.Tools.Config
{
    public class UploadProvice
    {
        //独立配置文件路径
        static string path = @"E:\workspace\Fancy-fix\源代码\OA\FancyFix.OA\Utils\Tools\Config\UploadConfig.config";

        private static UploadConfig uploadConfig;
        static UploadProvice()
        {
            uploadConfig = ConfigurationManager.GetSection("UploadConfig") as UploadConfig;

            //独立配置文件形式
            //ExeConfigurationFileMap map = new ExeConfigurationFileMap() { ExeConfigFilename = path };
            //var configManager = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            //uploadConfig = configManager.GetSection("UploadConfig") as UploadConfig;
        }

        public static UploadConfig Instance()
        {
            return uploadConfig;
        }
    }
}
