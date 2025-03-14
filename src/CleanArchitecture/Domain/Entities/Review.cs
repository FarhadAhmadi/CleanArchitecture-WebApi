﻿using CleanArchitecture.Shared.Models.Base;

namespace CleanArchitecture.Domain.Entities;

public class Review : BaseEntity
{
    public string ReviewerName { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public int Rating { get; set; } // e.g., 1-5 stars

    // Foreign Key
    public string BookId { get; set; }
    public Book Book { get; set; } = null!;
}
