namespace SeniorLearnApi.Interfaces;

public interface IShowBulletinService<T>
{
    Task<T> GetBulletinDetails(string Id);
}
