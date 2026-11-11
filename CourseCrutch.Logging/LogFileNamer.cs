namespace CourseCrutch.Logging;

internal static class LogFileNamer
{
    public static FileStream CreateNext(string directory, string prefix, DateTime date)
    {
        Directory.CreateDirectory(directory);

        var baseName = $"{prefix}-{date:yyyy-MM-dd}";

        for (var index = 1; ; index++)
        {
            var fileName = index == 1 ? $"{baseName}.log" : $"{baseName}-{index}.log";
            var path = Path.Combine(directory, fileName);

            try
            {
                return new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite);
            }
            catch (IOException) when (File.Exists(path))
            {
                // Taken by an earlier run today, so try the next index.
            }
        }
    }
}
