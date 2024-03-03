using System.Text.Json;

namespace WatchesShop.Helpers;

public static class StringHelper
{
  public static string ToSlug(string input)
  {
    if (string.IsNullOrEmpty(input))
    {
      return "";
    }

    string slug = new string(input.ToLower().Where(c => Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c)).ToArray());
    slug = slug.Trim().Replace(' ', '-');
    return slug;
  }
}