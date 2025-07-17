namespace NotifyHub.Application.Interfaces;

// Seeder для базы данных
public interface IDbSeeder
{
    /// <summary>
    /// Seed данных
    /// </summary>
    /// <param name="cancellationToken">cancellationToken</param>
    /// <returns></returns>
    public Task SeedAsync(CancellationToken cancellationToken = default);
}