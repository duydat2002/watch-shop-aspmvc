namespace WatchShop2.Models;

public class CategoryWithProductCountModel
{
  public int CategoryId { get; set; }

  public string CategoryName { get; set; } = null!;

  public bool Active { get; set; }

  public int ProductCount { get; set; }
}