namespace WatchShop2.Models;

public class CartModel
{
  public int CartId { get; set; }

  public int ProductId { get; set; }

  public string ProductName { get; set; } = null!;

  public string ProductSlug { get; set; } = null!;

  public string ProductDesc { get; set; } = null!;

  public double Price { get; set; }

  public double TotalPrice { get { return CartQuantity * PriceSale; } }

  public int CartQuantity { get; set; }

  public int ProductQuantity { get; set; }

  public int Discount { get; set; }

  public string ProductImages { get; set; } = null!;

  public string ColorName { get; set; } = null!;

  public string ColorValue { get; set; } = null!;

  public string SizeName { get; set; } = null!;

  public double PriceSale { get; set; }
}

public class AddOrderModel
{
  public int UserId { get; set; }

  public string Carts { get; set; } = null!;
}