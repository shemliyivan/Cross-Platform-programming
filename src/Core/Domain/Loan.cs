namespace Core.Domain;

public enum LoanStatus { Active, Closed }

public sealed class Loan
{
    public string Id { get; }
    public BookCopy Copy { get; }
    public string ReaderId { get; }
    public DateTime IssuedOn { get; }
    public DateTime? ReturnedOn { get; private set; }
    
    // Це по-факту додаткове завдання(3)
    public LoanStatus Status { get; private set; }

    private Loan(string id, BookCopy copy, string readerId, DateTime issuedOn)
    {
        Id = id;
        Copy = copy;
        ReaderId = readerId;
        IssuedOn = issuedOn;
        Status = LoanStatus.Active;
    }

    // Відкриття видачі
    public static Loan Open(string id, BookCopy copy, string readerId, DateTime issuedOn)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new ArgumentException("Ідентифікатор видачі обов'язковий", nameof(id));
        if (copy == null)
            throw new ArgumentNullException(nameof(copy));
        if (string.IsNullOrWhiteSpace(readerId))
            throw new ArgumentException("Ідентифікатор читача обов'язковий", nameof(readerId));

        
        copy.Issue(); 

        return new Loan(id, copy, readerId, issuedOn);
    }

    // Закриття видачі
    public void Close(DateTime returnedOn)
    {
        Status = Status switch
        {
            LoanStatus.Active => LoanStatus.Closed,
            _ => throw new InvalidOperationException($"Видачу {Id} вже закрито, повторне закриття неможливе")
        };

        if (returnedOn < IssuedOn)
            throw new ArgumentOutOfRangeException(nameof(returnedOn), 
                $"Дата повернення ({returnedOn:d}) не може бути раніше дати видачі ({IssuedOn:d})");

        ReturnedOn = returnedOn;
        Copy.Return(); 
    }

    public override string ToString() => 
        $"Видача {Id} | Читач: {ReaderId} | Примірник: {Copy.Id} | Статус: {Status}";
}