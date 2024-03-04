
namespace WatchShop2.Helpers;

public static class UploadHelper
{
  public static async Task<string> UploadOne(IWebHostEnvironment environment, IFormFile file, string? fileName, string subDirectory = "")
  {
    if (file != null && file.Length > 0)
    {
      if (fileName == null)
        fileName = Path.GetFileName(file.FileName);
      else
        fileName += Path.GetExtension(Path.GetFileName(file.FileName));

      string path = Path.Combine(environment.ContentRootPath, "wwwroot", "image", subDirectory);

      if (!Directory.Exists(path))
        Directory.CreateDirectory(path);

      path = Path.Combine(path, fileName);

      using (var fileStream = new FileStream(path, FileMode.Create))
      {
        await file.CopyToAsync(fileStream);
        return fileName;
      }
    }
    return "";
  }

  public static async Task<List<string>> UploadMulti(IWebHostEnvironment environment, IFormFile[] files, string? fileName, string subDirectory = "")
  {
    List<string> names = new List<string>();

    for (int i = 0; i < files.Length; i++)
    {
      IFormFile file = files[i];
      string _fileName = fileName;

      if (file != null && file.Length > 0)
      {
        if (_fileName == null)
          _fileName = Path.GetFileName(file.FileName);
        else
          _fileName += i + Path.GetExtension(Path.GetFileName(file.FileName));

        string path = Path.Combine(environment.ContentRootPath, "wwwroot", "image", subDirectory);

        if (!Directory.Exists(path))
          Directory.CreateDirectory(path);

        path = Path.Combine(path, _fileName);

        using (var fileStream = new FileStream(path, FileMode.Create))
        {
          await file.CopyToAsync(fileStream);
          names.Add(_fileName);
        }
      }
    }
    return names;
  }
}