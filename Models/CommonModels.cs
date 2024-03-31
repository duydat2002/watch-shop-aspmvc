namespace WatchShop2.Models;

public class PagedListMetaDataModel
{
  public int PageCount { get; protected set; }

  public int TotalItemCount { get; protected set; }

  public int PageNumber { get; protected set; }

  public int PageSize { get; protected set; }

  public bool HasPreviousPage { get; protected set; }

  public bool HasNextPage { get; protected set; }

  public bool IsFirstPage { get; protected set; }

  public bool IsLastPage { get; protected set; }

  public int FirstItemOnPage { get; protected set; }

  public int LastItemOnPage { get; protected set; }
}