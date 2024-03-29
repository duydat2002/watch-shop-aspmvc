namespace WatchShop2.Models;

public class AddProductModel
{
  public int ProductId { get; set; }

  public int ColorId { get; set; }

  public int SizeId { get; set; }

  public string ProductName { get; set; } = null!;

  public string ProductSlug { get; set; } = null!;

  public string ProductDesc { get; set; } = null!;

  public double Price { get; set; }

  public int Quantity { get; set; }

  public int Discount { get; set; }

  public string ProductImages { get; set; } = null!;

  public string Categories { get; set; } = null!;
}