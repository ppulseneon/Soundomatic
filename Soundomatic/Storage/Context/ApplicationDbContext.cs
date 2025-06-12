using Microsoft.EntityFrameworkCore;
using Soundomatic.Enums;
using Soundomatic.Models;

namespace Soundomatic.Storage.Context;

/// <summary>
/// Контекст базы данных приложения
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Конструктор контекста базы данных
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    /// <summary>
    /// Группы звуков
    /// </summary>
    public DbSet<SoundPack> SoundPacks { get; set; }
    
    /// <summary>
    /// Звуки
    /// </summary>
    public DbSet<Sound> Sounds { get; set; }

    /// <summary>
    /// Привязки клавиш
    /// </summary>
    public DbSet<KeyBinding> KeyBindings { get; set; }
    
    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<SoundPack>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.PlaybackStrategyType).HasDefaultValue(PlaybackStrategyType.Sequential);
            entity.HasMany(e => e.Sounds)
                .WithOne(e => e.SoundPack)
                .HasForeignKey(e => e.SoundPackId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        modelBuilder.Entity<Sound>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.AudioFormat).IsRequired();
            entity.Property(e => e.FileSizeBytes).IsRequired();
            entity.Property(e => e.Volume).HasDefaultValue(100);
            
            entity.HasOne(e => e.SoundPack)
                .WithMany(e => e.Sounds)
                .HasForeignKey(e => e.SoundPackId)
                .OnDelete(DeleteBehavior.Cascade).IsRequired();
        });

        modelBuilder.Entity<KeyBinding>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Key).IsRequired();
            
            entity.HasOne(e => e.Pack)
                .WithMany()
                .HasForeignKey(e => e.PackId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);
        });
    }
}
