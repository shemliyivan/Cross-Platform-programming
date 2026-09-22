using Core.Dto;
namespace Core.Domain;

public sealed class BookCopy
{
    public string Id { get; }
    public string Isbn { get; }
    
    public bool IsIssued { get; private set; }

    private BookCopy(string id, string isbn, bool isIssued)
    {
        Id = id;
        Isbn = isbn;
        IsIssued = isIssued;
    }

    // Фабричний метод із перевіркою інваріантів
    public static BookCopy Create(string id, string isbn, bool isIssued = false)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор примірника обов'язковий", nameof(id));
        
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN не може бути порожнім", nameof(isbn));

        return new BookCopy(id.Trim(), isbn.Trim(), isIssued);
    }

    // Бізнес-метод зміни стану
    public void Issue()
    {
        if (IsIssued)
            throw new InvalidOperationException($"Примірник {Id} вже виданий, повторна видача неможлива");
        
        IsIssued = true;
    }

    public void Return()
    {
        if (!IsIssued)
            throw new InvalidOperationException($"Примірник {Id} не був виданий, тому його не можна повернути");
        
        IsIssued = false;
    }

    public ProductDto ToDto() => new(Id, Isbn, "Назва з каталогу", 0);
    
    public static BookCopy FromDto(ProductDto dto) => 
        Create(dto.Id, dto.Isbn, false);

    public override string ToString() => 
        $"Примірник {Id} [ISBN: {Isbn}] — {(IsIssued ? "Виданий" : "Доступний")}";
}