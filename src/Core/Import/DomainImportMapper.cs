using Core.Domain;
using Core.Dto;
using System.Collections.Generic;
using System;

namespace Core.Import;

public static class DomainImportMapper
{
    public static (IReadOnlyList<BookCopy> Entities, IReadOnlyList<string> Errors) MapToDomain(ImportResult<ProductDto> importResult)
    {
        var entities = new List<BookCopy>();
        var errors = new List<string>(importResult.Errors); 

        foreach (var dto in importResult.Items)
        {
            try
            {
                entities.Add(BookCopy.FromDto(dto));
            }
            catch (Exception ex)
            {
                errors.Add($"Помилка бізнес-логіки для {dto.Id}: {ex.Message}");
            }
        }

        return (entities, errors);
    }
}