
namespace DalApi
{
    public interface ICrud<T> where T : struct
    {
        /// <summary>
        /// An operation receives a data entity and adds to the entity pool
        /// </summary>
        /// <param name="entity">Data entity to be added</param>
        public int Add(T entity);

        /// <summary>
        /// An operation accepts an entity ID number and will return the entity
        /// </summary>
        /// <param name="id">Entity ID number to return the entity</param>
        /// <returns>Requested data entity</returns>
        public T Get(/*Func<T?, bool>? condition,*/ int id);

        /// <summary>
        /// An operation that returns all existing entity objects
        /// </summary>
        /// <returns>Every existing entity object</returns>
        public IEnumerable<T?> GetAll(Func<T?, bool>? condition=null);

        /// <summary>
        /// The operation receives an entity ID number and deletes the requested entity
        /// </summary>
        /// <param name="id">Entity ID number</param>
        public void Delete(int id);

        /// <summary>
        /// An action receives an object to update and updates it as requested
        /// </summary>
        /// <param name="updateEntity"></param>
        public void Update(T updateEntity);

    }
}
