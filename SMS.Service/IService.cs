using SMS.Core;
using System.Data;
using System.Threading.Tasks;

namespace SMS.Service
{
    /// <summary>
    /// Defines the contract for a generic service that provides asynchronous CRUD operations for entities of type T.
    /// </summary>
    /// <remarks>Implementations of this interface typically interact with a data store to perform create,
    /// read, update, and delete operations. All methods return a DBResponse object that encapsulates the result and any
    /// relevant status information.</remarks>
    /// <typeparam name="T">The type of the entity managed by the service.</typeparam>
    internal interface IService<T>
    {
        /// <summary>
        /// Asynchronously adds the specified entity to the data store.
        /// </summary>
        /// <param name="entity">The entity to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DBResponse with the identifier
        /// of the added entity.</returns>
        Task<DBResponse<int>> AddAsync(T entity);

        /// <summary>
        /// Asynchronously retrieves an entity by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve. Must be a non-negative integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{T}"/>
        /// with the entity if found; otherwise, an appropriate response indicating the result of the lookup.</returns>
        Task<DBResponse<T>> FindAsync(int id);

        /// <summary>
        /// Checks if the record with the given id exists in the datastore or not.
        /// </summary>
        /// <param name="id">Record id wanted to check.</param>
        /// <returns>Returns true if a record with given id exists in the datastore otherwise return false.</returns>
        Task<DBResponse<bool>> ExistsAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all entities from the data store.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{DataTable}"/>
        /// with all entities retrieved from the data store.</returns>
        Task<DBResponse<DataTable>> GetAllAsync();

        /// <summary>
        /// Asynchronously updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{bool}"/>
        /// indicating whether the update was successful.</returns>
        Task<DBResponse<bool>> UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously deletes the specified entity from the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete. Must be a non-negative integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{bool}"/>
        /// indicating whether the deletion was successful.</returns>
        Task<DBResponse<bool>> DeleteAsync(int id);
    }
}       
