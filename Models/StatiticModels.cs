namespace WatchShop2.Models;

public class DashBoardInfoModel
{
  public int NewCustomers { get; set; }

  public int Orders { get; set; }

  public double Sales { get; set; }
}

public class SalesStatiticModel
{
  public int OrderDay { get; set; }

  public double CompletedSales { get; set; }

  public double PendingSales { get; set; }
}

public class DashBoardOrderStatusModel
{
  public int CancelledOrders { get; set; }

  public int CompletedOrders { get; set; }

  public int PendingOrders { get; set; }
}