using Core.Dto;
using Core.Import;
using Core.Domain;

string path = args.Length > 0 ? args[0] : Path.Combine("data", "sample.csv");

if (!File.Exists(path))
{
    Console.WriteLine($"Файл не знайдено: {Path.GetFullPath(path)}");
}
else
{
    ImportResult<ProductDto> importResult = ProductCsvImporter.Load(path);
    
    var (entities, errors) = DomainImportMapper.MapToDomain(importResult);

    Console.WriteLine($"Створено валідних примірників (BookCopy): {entities.Count}");
    foreach (var copy in entities.Take(5))
    {
        Console.WriteLine($" {copy}");
    }

    if (errors.Count > 0)
    {
        Console.WriteLine($"Пропущено через помилки (синтаксис + бізнес-правила): {errors.Count}");
        foreach (var e in errors)
        {
            Console.WriteLine($" ! {e}");
        }
    }
}

// 1 сценарій - успіх
BookCopy book = BookCopy.Create("C-001", "978-0131103627");
Console.WriteLine(book); 

Loan loan = Loan.Open("L-100", book, "Reader-42", new DateTime(2023, 10, 01));
Console.WriteLine(loan);
Console.WriteLine(book); 

loan.Close(new DateTime(2023, 10, 15));
Console.WriteLine(loan);
Console.WriteLine(book);

// 2 сценарій - порушення інваріантів
TryDo("Спроба створити з порожнім ISBN", () => BookCopy.Create("C-002", "   "));

TryDo("Видача вже виданого примірника", () => 
{
    var busyCopy = BookCopy.Create("C-003", "12345");
    busyCopy.Issue(); 
    busyCopy.Issue(); 
});

TryDo("Повернення раніше дати видачі", () => 
{
    var c = BookCopy.Create("C-004", "11111");
    var l = Loan.Open("L-101", c, "R-1", new DateTime(2023, 10, 10));
    l.Close(new DateTime(2023, 10, 01)); // Дата закриття менша за 10 жовтня
});

return 0;

static void TryDo(string title, Action action)
{
    try
    {
        action();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($" [FAIL] {title}: виняток НЕ спрацював — інваріант відсутній!");
        Console.ResetColor();
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($" [OK] {title}: {ex.GetType().Name} — {ex.Message}");
        Console.ResetColor();
    }
}