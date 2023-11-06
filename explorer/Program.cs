using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using System.Linq;

class Program
{
    
    static void Main()
    {
        FileExplorer.Start();
    }
}
public static class FileExplorer
{
    public static DirectoryInfo currentDirectory;

    public static void Start()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        Console.WriteLine("Выберите диск:");

        int driveIndex = MenuSelector.Run(allDrives);

        currentDirectory = allDrives[driveIndex].RootDirectory;
        DriveInfo drive = allDrives[driveIndex];
        Explore();
    }

    public static void Explore()
    {
        FileSystemInfo[] items = currentDirectory.GetFileSystemInfos();

        Console.WriteLine($"Текущее расположение: {currentDirectory.FullName}");

        int fileIndex = MenuSelector.Run(items);

        if (items[fileIndex] is DirectoryInfo)
        {
            currentDirectory = (DirectoryInfo)items[fileIndex];
            Explore();
        }
        else
        {
            Process.Start(new ProcessStartInfo(items[fileIndex].FullName) { UseShellExecute = true });
        }
    }
}
public static class MenuSelector
{
    public static int Run(DriveInfo[] drives)
    {
        int currentIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Выберите диск:");
            for (int i = 0; i < drives.Length; i++)
            {
                if (i == currentIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{i+1}: {drives[i].Name} {drives[i].AvailableFreeSpace/1024/1024/1024}/{drives[i].TotalSize/1024/1024/1024}gb");

                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                currentIndex = Math.Max(currentIndex - 1, 0);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                currentIndex = Math.Min(currentIndex + 1, drives.Length - 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                return currentIndex;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                Environment.Exit(0);
            }
            Console.Clear();
        }
    }

    public static int Run(FileSystemInfo[] fileSysInfo)
    {
        int currentIndex = 0;

        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Текущее расположение: {FileExplorer.currentDirectory}");
            for (int i = 0; i < fileSysInfo.Length; i++)
            {
                if (i == currentIndex)
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (fileSysInfo[i] is DirectoryInfo)
                {
                    Console.WriteLine($"{fileSysInfo[i].Name}   Дата создания: {fileSysInfo[i].CreationTime}");
                }
                else
                {
                    Console.WriteLine($"{fileSysInfo[i].Name}   Дата создания: {fileSysInfo[i].CreationTime}   Расширение: {fileSysInfo[i].Extension}");
                }

                Console.ResetColor();
            }

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow)
            {
                currentIndex = Math.Max(currentIndex - 1, 0);
            }
            else if (key.Key == ConsoleKey.DownArrow)
            {
                currentIndex = Math.Min(currentIndex + 1, fileSysInfo.Length - 1);
            }
            else if (key.Key == ConsoleKey.Enter)
            {
                return currentIndex;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                FileExplorer.Start();
            }
        }
    }
}