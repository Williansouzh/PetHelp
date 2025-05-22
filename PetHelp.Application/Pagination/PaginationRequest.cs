namespace PetHelp.Application.Pagination;

public class PaginationRequest
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string SortColumn { get; set; }
    public string SortOrder { get; set; } = "asc";
    public string Filter { get; set; }
}