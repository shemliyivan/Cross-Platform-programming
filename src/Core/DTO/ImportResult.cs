namespace Core.Dto;
public record ProductDto(
string Id,
string Isbn,
string Title,
int Year);
public sealed record ImportResult<T>(IReadOnlyList<T> Items, IReadOnlyList<string> Errors);