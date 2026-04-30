using SMS.Core.DTOs;
using System;
using System.IO;

namespace SMS.Core
{
    public static class Global
    {
        public static readonly string AppName = "SMS";
        public static readonly string AppDataDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMS");

        public static readonly string ImagesFolderPath = Path.Combine(AppDataDirectory, "Images");

        public static readonly string LoggingFolderPath = Path.Combine(AppDataDirectory, "Logs");
        public static readonly string LogFilePath = Path.Combine(LoggingFolderPath, "app.log");
        public static User CurrentUser { get; internal set; }
    }
}
