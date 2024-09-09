using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NGitLab.Mock;

[DebuggerDisplay("{FullPath}")]
public sealed class TemporaryDirectory : IDisposable
{
    private const string LockFileName = "lock";
    private const string DirectoryName = "a";

    private readonly string _path;
    private readonly Stream _lockFile;
    private static readonly HashSet<string> TempDirectories = new(StringComparer.OrdinalIgnoreCase);

    public string FullPath { get; }

    public bool KeepDirectory { get; set; }

    private TemporaryDirectory(string path, string innerPath, Stream lockFile)
    {
        _path = path;
        FullPath = innerPath;
        _lockFile = lockFile;
    }

    public static TemporaryDirectory Create()
    {
        return CreateUniqueDirectory(Path.Combine(Path.GetTempPath(), "TD", DateTime.UtcNow.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture)));
    }

    public string GetFullPath(string relativePath)
    {
        return Path.Combine(FullPath, relativePath);
    }

    public void Dispose()
    {
        if (!KeepDirectory)
        {
            var di = new DirectoryInfo(FullPath);
            DeleteFileSystemEntry(di);
        }

        _lockFile.Dispose();

        if (!KeepDirectory)
        {
            var di = new DirectoryInfo(_path);
            DeleteFileSystemEntry(di);
        }
    }

    public void MakeReadOnly()
    {
        MakeReadOnly(new DirectoryInfo(FullPath));
    }

    private static void MakeReadOnly(FileSystemInfo fileSystemInfo)
    {
        if (fileSystemInfo is DirectoryInfo directoryInfo)
        {
            foreach (var childInfo in directoryInfo.GetFileSystemInfos())
            {
                MakeReadOnly(childInfo);
            }
        }

        fileSystemInfo.Attributes |= FileAttributes.ReadOnly;
    }

    private static TemporaryDirectory CreateUniqueDirectory(string folderPath)
    {
        /*
         * Structure
         * - temp/<folder>/lock => allows to detect concurrency
         * - temp/<folder>/<returned_value>/
         */
        var count = 1;
        while (true)
        {
            Stream lockFileStream = null;
            try
            {
                var tempPath = folderPath + "_";

                lock (TempDirectories)
                {
                    while (TempDirectories.Contains(folderPath) || Directory.Exists(folderPath))
                    {
                        folderPath = tempPath + count.ToString(CultureInfo.InvariantCulture);
                        if (count == int.MaxValue)
                            throw new InvalidOperationException("Cannot create a temporary directory");

                        count++;
                    }

                    Directory.CreateDirectory(folderPath);
                }

                var lockFilePath = Path.Combine(folderPath, LockFileName);
                lockFileStream = new FileStream(lockFilePath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None);
                var innerFolderPath = Path.Combine(folderPath, DirectoryName);
                if (Directory.Exists(innerFolderPath))
                {
                    lockFileStream.Dispose();
                    continue;
                }

                Directory.CreateDirectory(innerFolderPath);

                bool tempDirRegistered;
                lock (TempDirectories)
                {
                    tempDirRegistered = TempDirectories.Add(folderPath);
                }

                // Assert folder is empty
                if (!tempDirRegistered || Directory.EnumerateFileSystemEntries(innerFolderPath).Any())
                {
                    lockFileStream.Dispose();
                    continue;
                }

                return new TemporaryDirectory(folderPath, innerFolderPath, lockFileStream);
            }
            catch (IOException)
            {
                // The folder may already in use
            }
            catch
            {
                lockFileStream?.Dispose();
                throw;
            }

            lockFileStream?.Dispose();
        }
    }

    public static void DeleteDirectory(string path)
    {
        DeleteFileSystemEntry(new DirectoryInfo(path));
    }

    public static void DeleteFile(string path)
    {
        DeleteFileSystemEntry(new FileInfo(path));
    }

    private static void DeleteFileSystemEntry(FileSystemInfo fileSystemInfo)
    {
        var exceptions = new List<Exception>();

        DeleteFileSystemEntry(fileSystemInfo, exceptions);

        if (exceptions.Count > 0)
        {
            throw new AggregateException($"Could not delete {fileSystemInfo.FullName}", exceptions);
        }
    }

    private static void DeleteFileSystemEntry(FileSystemInfo fileSystemInfo, List<Exception> ioExceptions)
    {
        if (!fileSystemInfo.Exists)
            return;

        var exceptionBefore = ioExceptions.Count;

        if (fileSystemInfo is DirectoryInfo directoryInfo)
        {
            DeleteDirectoryContent(directoryInfo, ioExceptions);
        }

        if (exceptionBefore < ioExceptions.Count)
        {
            return;
        }

        try
        {
            fileSystemInfo.Attributes = FileAttributes.Normal;
            if (fileSystemInfo is DirectoryInfo directory)
            {
                // The content should be empty, but it is not always true for some unknown reasons.
                // Let's .NET try to do its job.
                directory.Delete(recursive: true);
            }
            else
            {
                fileSystemInfo.Delete();
            }
        }
        catch (FileNotFoundException)
        {
        }
        catch (DirectoryNotFoundException)
        {
        }
        catch (IOException)
        {
        }
    }

    private static void DeleteDirectoryContent(DirectoryInfo directoryInfo, List<Exception> ioExceptions)
    {
        foreach (var childInfo in directoryInfo.GetFileSystemInfos())
        {
            if (IsSymbolicLink(childInfo))
            {
                try
                {
                    childInfo.Delete();
                }
                catch (FileNotFoundException)
                {
                }
                catch (DirectoryNotFoundException)
                {
                }
            }
            else
            {
                try
                {
                    DeleteFileSystemEntry(childInfo, ioExceptions);
                }
                catch (FileNotFoundException)
                {
                }
                catch (DirectoryNotFoundException)
                {
                }
                catch (Exception exception)
                {
                    ioExceptions.Add(exception);
                }
            }
        }
    }

    private static bool IsSymbolicLink(FileSystemInfo fileSystemInfo)
    {
        if (fileSystemInfo.Attributes.HasFlag(FileAttributes.ReparsePoint))
            return true;

        return false;
    }

    public void CopyFolder(string sourcePath)
    {
        foreach (var dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, FullPath));
            CopyFolder(dirPath.Replace(sourcePath, FullPath));
        }

        foreach (var newPath in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
        {
            System.IO.File.Copy(newPath, newPath.Replace(sourcePath, FullPath), overwrite: true);
        }
    }
}
