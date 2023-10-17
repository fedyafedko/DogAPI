using Common.Enums;

namespace Common;

public class GetDogsRequest
{
    public string? Attribute { get; set; }

    public Order Order { get; set; }

    public PaginationModel? Pagination { get; set; }
}

public record PaginationModel(int PageNumber, int PageSize);