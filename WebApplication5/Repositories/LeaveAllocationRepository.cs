using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Constants;
using LeaveManagement.Web.Data;
using Microsoft.AspNetCore.Identity;
using WebApplication5.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Web.Models;

namespace LeaveManagement.Web.Repositories
{
    public class LeaveAllocationRepository : GenericRepostiory<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Employee> userManager;
        private readonly ILeaveTypeRepository leaveTypeRepository;

        public LeaveAllocationRepository(ApplicationDbContext context, 
            UserManager<Employee> userManager, ILeaveTypeRepository leaveTypeRepository) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
            this.leaveTypeRepository = leaveTypeRepository;
        }

        public async Task<bool> AllocationExists(string employeeId, int leaveTypeId, int period)
        {
            return await context.LeaveAllocations.AnyAsync(q => q.EmployeeId == employeeId
                                                                && q.LeaveTypeId == leaveTypeId
                                                                && q.Period == period);
        }

        public async Task<EmployeeAllocationVM> GetEmployeeAllocation(string employeeId)
        {
            var allocations = await context.LeaveAllocations.Where(q => q.EmployeeId.Equals(employeeId)).ToListAsync();

        }

        public async Task LeaveAllocation(int leaveTypeId)
        {
            var employees = await userManager.GetUsersInRoleAsync(Roles.User);
            var period = DateTime.Now.Year;
            var leaveType = await leaveTypeRepository.GetAsync(leaveTypeId);
            var allocations = new List<LeaveAllocation>();

            foreach(var employee in employees) 
            {
                if (await AllocationExists(employee.Id, leaveTypeId, period))
                    continue;

                allocations.Add(new LeaveAllocation
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId= leaveTypeId,
                    Period = period,
                    NumberOfDays = leaveType.DefaultDays
                });
            }

            await AddRangeAsync(allocations);
        }
    }
}
