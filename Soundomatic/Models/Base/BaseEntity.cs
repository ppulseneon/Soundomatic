using System;
using System.ComponentModel.DataAnnotations;

namespace Soundomatic.Models.Base;

/// <summary>
/// Базвоая сущности для записи базы данных
/// </summary>
public class BaseEntity
{
    /// <summary>
    /// Уникальный идентификатор записи
    /// </summary>
    [Key]
    public long Id { get; set; }
    
    /// <summary>
    /// Дата создания записи
    /// </summary>
    public DateTime CreatedAt { get; set; }
}