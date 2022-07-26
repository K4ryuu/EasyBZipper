using System.Diagnostics;
using ICSharpCode.SharpZipLib.BZip2;

public class BZipperConsole
{
    public static Stopwatch CompressTimer = new Stopwatch();
    public static void Main()
    {
        Console.WriteLine("Compressing Files... | Made by Karyuu\r\n ");

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        ProcessDirectory(Environment.CurrentDirectory);

        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(" \r\nCompression finished under {0:00}:{1:00}.{2}\r\nPress any key to close this window...", ts.Minutes, ts.Seconds, ts.Milliseconds);
        Console.Read();
    }

    public static void ProcessDirectory(string targetDirectory)
    {
        string[] fileEntries = Directory.GetFiles(targetDirectory);
        foreach (string fileName in fileEntries)
            ProcessFile(fileName);

        string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
        foreach (string subdirectory in subdirectoryEntries)
            ProcessDirectory(subdirectory);
    }

    public static void ProcessFile(string targetFile)
    {
        FileInfo fileToBeZipped = new FileInfo(targetFile);

        if (fileToBeZipped.Extension == ".bz2" || fileToBeZipped.Name == String.Concat(AppDomain.CurrentDomain.FriendlyName, ".exe"))
            return;

        FileInfo zipFileName = new FileInfo(string.Concat(fileToBeZipped.FullName, ".bz2"));

        if (File.Exists(zipFileName.FullName))
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Skipped: {0}", zipFileName.FullName);
            return;
        }

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Compressing: {0}", targetFile);

        using (FileStream fileToBeZippedAsStream = fileToBeZipped.OpenRead())
        {
            using (FileStream zipTargetAsStream = zipFileName.Create())
            {
                try
                {
                    CompressTimer.Start();
                    BZip2.Compress(fileToBeZippedAsStream, zipTargetAsStream, true, 4096);
                    CompressTimer.Stop();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Finished: {0} [{1} ms]", zipFileName.FullName, CompressTimer.ElapsedMilliseconds);
                    CompressTimer.Reset();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}