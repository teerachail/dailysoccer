namespace ApiApp.Repositories
{
    /// <summary>
    /// Demo Repository Interface
    /// </summary>
    public interface IDemoRepository
    {
        /// <summary>
        /// Get an Email (DEMO)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        string GetEmail(string userId);
    }
}