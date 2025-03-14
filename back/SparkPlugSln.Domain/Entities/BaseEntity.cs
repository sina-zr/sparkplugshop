using System.ComponentModel.DataAnnotations;

namespace SparkPlugSln.Domain.Entities;

public class BaseEntity<T>
{
    [Key]
    public T Id { get; set; }
    public bool IsDelete { get; set; } = false;
    public DateTime CreateDate { get; set; } = DateTime.UtcNow;
}