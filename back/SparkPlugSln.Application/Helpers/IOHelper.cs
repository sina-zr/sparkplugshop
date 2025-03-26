using Microsoft.AspNetCore.Http;

public static class IOHelper
{
    /// <summary>
    /// Stores a new file to the specified path and returns the file name.
    /// </summary>
    /// <param name="file">The file to store.</param>
    /// <param name="path">The directory path where the file will be stored.</param>
    /// <returns>The name of the stored file.</returns>
    public static string StoreNewFile(IFormFile file, string path, string? customName = null)
    {
        // Check if file is null or empty
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is invalid or empty.");

        // Ensure the directory exists
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        // Generate a unique file name (to avoid overwriting existing files)
        string uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
        if (!string.IsNullOrEmpty(customName))
        {
            uniqueFileName = customName + Path.GetExtension(file.FileName);
        }

        // Combine the path with the file name
        var filePath = Path.Combine(path, uniqueFileName);

        // Save the file to the specified path
        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"File copy failed: {ex.Message}");
            throw;
        }


        // Return the unique file name
        return uniqueFileName;
    }

    /// <summary>
    /// Deletes the specified file from the given path.
    /// </summary>
    /// <param name="path">The directory path where the file is stored.</param>
    /// <param name="fileName">The name of the file to delete.</param>
    public static void DeleteFile(string path, string fileName)
    {
        // Combine the path and file name to get the full file path
        var filePath = Path.Combine(path, fileName);

        // Check if the file exists
        if (File.Exists(filePath))
        {
            // Delete the file
            File.Delete(filePath);
        }
        // else
        // {
        //     throw new FileNotFoundException("The specified file does not exist.", fileName);
        // }
    }
}
