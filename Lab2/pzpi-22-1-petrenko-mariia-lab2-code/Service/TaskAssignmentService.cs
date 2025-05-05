using FarmKeeper.Data;
using FarmKeeper.Enums;
using FarmKeeper.Models;
using FarmKeeper.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FarmKeeper.Service
{
    public class TaskAssignmentService : ITaskAssignmentService
    {
        private readonly FarmKeeperDbContext dbContext;
        public TaskAssignmentService(FarmKeeperDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
        public async Task<List<UserTask>> AssignTasks(List<User> workers, List<Assignment> assignments)
        {
            var userTasks = new List<UserTask>();

            var sortedWorkers = workers
                .OrderBy(w => dbContext.UserTasks.Count(ut => ut.UserId == w.Id)) 
                .ThenBy(w => dbContext.UserTasks.Count(ut => ut.UserId == w.Id && ut.Assignment.Priority == Priority.High)) 
                .ToList();

            if (sortedWorkers.Count == 0)
            {
                throw new InvalidOperationException("No workers available to assign tasks.");
            }

            int workerIndex = 0;
            var notStartedAssignments = dbContext.UserTasks
                .Where(ut => ut.Assignment.Status == Status.NotStarted)
                .ToList();

            dbContext.UserTasks.RemoveRange(notStartedAssignments);
            dbContext.SaveChanges();
            
            foreach (var assignment in assignments)
            {
                var numberOfParticipants = assignment.NumberOfParticipants;

                var assignedWorkers = new List<User>();

                for (int i = 0; i < numberOfParticipants; i++)
                {
                    var worker = sortedWorkers[workerIndex];

                    
                    if (!userTasks.Any(ut => ut.UserId == worker.Id && ut.AssignmentId == assignment.Id))
                    {
                        assignedWorkers.Add(worker);
                        userTasks.Add(new UserTask
                        {
                            Id = Guid.NewGuid(),
                            UserId = worker.Id,
                            AssignmentId = assignment.Id
                   
                        });
                    }

                    workerIndex = (workerIndex + 1) % sortedWorkers.Count;
                }

                if (assignedWorkers.Count < numberOfParticipants)
                {
                    workerIndex = 0; 
                }
            }
            return userTasks;
        }
    }
}
