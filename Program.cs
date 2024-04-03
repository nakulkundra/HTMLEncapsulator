using System;
using System.IO;
using System.Linq;

class ModifyHtmlFiles
{
    static void Main(string[] args)
    {
        //if (args.Length != 1)
        //{
        //    Console.WriteLine("Usage: ModifyHtmlFiles <directoryPath>");
        //    return;
        //}

        //string directoryPath = args[0];

        ProcessDirectory(@"D:\New folder\src");
    }

    static void ProcessDirectory(string path)
    {
        string[] htmlFiles = Directory.GetFiles(path, "*.html");

        foreach (string file in htmlFiles)
        {
            ModifyHtmlFile(file);
        }

        string[] subdirectories = Directory.GetDirectories(path);

        foreach (string subdirectory in subdirectories)
        {
            ProcessDirectory(subdirectory);
        }
    }

    static void ModifyHtmlFile(string filePath)
    {
        if (filePath.Contains("\\") && filePath.ToLower().Contains("component"))
        {
            string[] splitResult = filePath.Split('\\');
            string componentName = splitResult.Last().ToLower();
            componentName = componentName.Replace(".html", "");
            componentName = componentName.Replace(".component", "");

            string DivStart = "<div id=\"" + componentName + "\">\n";
            string DivEnd = "\n </div>";

            string content = File.ReadAllText(filePath);

            try
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"Error removing read-only attribute from {filePath}: Insufficient permissions. :: "+ex.Message);
            }

            if (content.StartsWith(DivStart))
            {
                Console.WriteLine($"File Already has the required headers : {filePath}");
                return;
            }

            string modifiedContent = DivStart + content + DivEnd;
            File.WriteAllText(filePath, modifiedContent);
            Console.WriteLine($"Modified file: {filePath}.");
        }
    }
}