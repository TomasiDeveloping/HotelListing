using HotelListing.Core.Models;

namespace HotelListing.Core.Contracts;

public interface IGenericRepository<T> where T : class
{
    Task<TResult> GetAsync<TResult>(int? id);
    Task<List<TResult>> GetAllAsync<TResult>();
    Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters);
    Task<TResult> AddAsync<TSource, TResult>(TSource source);
    Task DeleteAsync(int id);
    Task UpdaterAsync<TSource>(int id, TSource source);
    Task<bool> Exists(int id);
}