using FarmKeeper.Models;

namespace FarmKeeper.Repositories
{
    public interface IAssignmentRepository
    {
        Task<List<Assignment>> GetAllAsync();
        Task<Assignment?> GetByIdAsync(Guid id);
        Task<Assignment> CreateAsync(Assignment assignment);
        Task<Assignment?> UpdateAsync(Guid id, Assignment assignment);
        Task<Assignment?> DeleteAsync(Guid id);
        Task<List<Assignment>> GetNotStartedAssignmentsAsync(Guid farmId);
        Task<List<Assignment>> GetByIdsAsync(List<Guid> assignmentIds);
    }
}
