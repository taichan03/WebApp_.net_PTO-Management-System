using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Constants;
using LeaveManagement.Web.Data;
using Microsoft.AspNetCore.Identity;
using WebApplication5.Data;

namespace LeaveManagement.Web.Repositories
{
    public class LeaveAllocationRepository : GenericRepostiory<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly UserManager<Employee> userManager;
        public LeaveAllocationRepository(ApplicationDbContext context, UserManager<Employee> userManager) : base(context)
        {
            this.userManager = userManager;
        }

        public async Task LeaveAllocation(int leaveTypeId)
        {
            var employees = await userManager.GetUsersInRoleAsync(Roles.User);

            throw new NotImplementedException();
        }
    }
}
