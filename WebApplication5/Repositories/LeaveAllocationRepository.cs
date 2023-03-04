using LeaveManagement.Web.Contracts;
using LeaveManagement.Web.Constants;
using LeaveManagement.Web.Data;
using Microsoft.AspNetCore.Identity;
using WebApplication5.Data;
using Microsoft.EntityFrameworkCore;
using LeaveManagement.Web.Models;
using AutoMapper;

namespace LeaveManagement.Web.Repositories
{
    public class LeaveAllocationRepository : GenericRepostiory<LeaveAllocation>, ILeaveAllocationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<Employee> userManager;
        private readonly ILeaveTypeRepository leaveTypeRepository;
        private readonly IMapper mapper;

        public LeaveAllocationRepository(ApplicationDbContext context, 
            UserManager<Employee> userManager, 
            ILeaveTypeRepository leaveTypeRepository,
            IMapper mapper) : base(context)
        {
            this.context = context;
            this.userManager = userManager;
            this.leaveTypeRepository = leaveTypeRepository;
            this.mapper = mapper;
        }

        public async Task<bool> AllocationExists(string employeeId, int leaveTypeId, int period)
        {
            return await context.LeaveAllocations.AnyAsync(q => q.EmployeeId == employeeId
                                                                && q.LeaveTypeId == leaveTypeId
                                                                && q.Period == period);
        }

        //select*
        //from leaveallocations as la
        //join leavetypes as lt on la.leavetypeid = lt.id
        //where la.employeeid = @employeeid
        public async Task<EmployeeAllocationVM> GetEmployeeAllocation(string employeeId)
        {
            List<LeaveAllocation> allocations = await context.LeaveAllocations
                .Include(q => q.LeaveType)
                .Where(q => q.EmployeeId == employeeId)
                .ToListAsync();
            Employee employee = await userManager.FindByIdAsync(employeeId);

            EmployeeAllocationVM employeeAllocationModel = mapper.Map<EmployeeAllocationVM>(employee);

            employeeAllocationModel.LeaveAllocations = mapper.Map<List<LeaveAllocationVM>>(allocations);

            return employeeAllocationModel;
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
