
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
        /// <param name="condition">Gets a function to filter</param>
        /// <returns>conditional bone</returns>
        public T Get(Func<T?, bool> condition);

        /// <summary>
        /// The function returns a collection of all objects that meet a condition if received
        /// </summary>
        /// <param name="condition">Condition for filtering if necessary</param>
        /// <returns>A collection of all members</returns>
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
