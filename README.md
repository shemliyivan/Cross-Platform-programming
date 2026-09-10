# CrossApp
Наскрізний проєкт з крос-платформного програмування.
Предметна область: Бібліотека. Сутності: Book, BookCopy, Reader, Loan.
Призначення: облік видач примірників книг читачам.
## Запуск
dotnet build
dotnet run --project src/Cli
## Структура Solution
CrossApp/
└──src/
   ├── Cli/
       ├── Cli.csproj
       └── Program.cs
   └── Core/
       ├── Core.csproj
       └── EvironmentInfo.cs
## таблиця «RID – режим – розмір – чи потрібен встановлений runtime»
RID           Режим           Розмір publish Потрібен runtime
linux-x64 self-contained      ~80 МБ         ні
linux-x64 framework-dependent ~144 КБ        так (.NET 10)