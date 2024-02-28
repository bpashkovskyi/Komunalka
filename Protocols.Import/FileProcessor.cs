namespace Protocols.Import;

public class FileProcessor
{
    private static string folderPath = @"D:\\kodlo";

    public static void RenameFiles()
    {
        var files = Directory.GetFiles(folderPath, "*.pdf", SearchOption.TopDirectoryOnly);

        foreach (var filePath in files)
        {
            var protocol = FileParser.GetProtocolNumberAndDate(filePath);

            if (protocol == null)
            {
                continue;
            }

            var sourcePath = filePath;

            var oldFileName = Path.GetFileNameWithoutExtension(filePath);
            var newFileName = $"Протокол №{protocol.Number} від {protocol.Date} року";

            var destinationPath = sourcePath.Replace(oldFileName, newFileName);

            File.Move(sourcePath, destinationPath);
        }
    }
}