using SMS.Core;
using System.Data;
using System.Threading.Tasks;

namespace SMS.Repository
{
    internal interface IRepository<T>
    {
        /// <summary>
        /// Adds a new entity to the data store and returns the ID of the newly created record.
        /// </summary>
        /// <param name="entity">The entity to add. Cannot be null.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{int}"/>
        /// with the ID of the newly created entity.</returns>
        Task<DBResponse<int>> AddAsync(T entity);

        /// <summary>
        /// Retrieves an entity from the database by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve. Must be a non-negative integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{T}"/>
        /// with the entity if found; otherwise, an appropriate response indicating the result of the lookup.</returns>
        Task<DBResponse<T>> GetAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all records from the data source.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DBResponse{DataTable}"/> with the retrieved records. If no records are found, the <see
        /// cref="DataTable"/> will be empty.</returns>
        Task<DBResponse<DataTable>> GetAllAsync();

        /// <summary>
        /// Asynchronously retrieves a page of records as a DataTable, using keyset pagination.
        /// </summary>
        /// <remarks>Keyset pagination is used to efficiently retrieve subsequent pages. Use the
        /// identifier of the last record from the previous result as lastId to fetch the next page.</remarks>
        /// <param name="pageSize">The maximum number of records to include in the returned page. Must be greater than zero.</param>
        /// <param name="lastId">The identifier of the last record from the previous page, or null to retrieve the first page.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DBResponse with a DataTable
        /// holding up to pageSize records. If no records are found, the DataTable will be empty.</returns>
        Task<DBResponse<DataTable>> GetPagedAsync(int pageSize, int? lastId);

        /// <summary>
        /// Asynchronously updates the specified entity in the data store.
        /// </summary>
        /// <param name="entity">The entity to update. Cannot be null. The entity must have a valid identifier corresponding to an existing
        /// record.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see
        /// cref="DBResponse{Boolean}"/> indicating whether the update was successful.</returns>
        Task<DBResponse<bool>> UpdateAsync(T entity);

        /// <summary>
        /// Asynchronously deletes the entity with the specified ID from the data store.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to delete. Must be a non-negative integer.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="DBResponse{Boolean}"/>
        /// indicating whether the deletion was successful.</returns>
        Task<DBResponse<bool>> DeleteAsync(int id);
    }
}
