﻿using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Domain.Entities;

public class Publisher : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    // Navigation Property
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
