using Dallas.Core.Data;
using Dallas.Core.Data.Collections;
using Dallas.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Dallas.Core.Services
{
    public class EmployeeService
    {
        private readonly ILogger<EmployeeService> _logger;

        private readonly MainDbContext _dbContext;

        public EmployeeService(
            ILogger<EmployeeService> logger,
            MainDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task AddAsync(string name, string jobTitle)
        {
            name = name.Trim();
            if(String.IsNullOrEmpty(name))
                throw new ArgumentException(name);

            jobTitle = jobTitle.Trim();
            if (String.IsNullOrEmpty(jobTitle))
                throw new ArgumentException(jobTitle);

            Employee employee = new Employee
            {
                Name = name,
                JobTitle = jobTitle
            };

            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id) 
        {
            return await _dbContext.Employees.SingleOrDefaultAsync(x => x.EmployeeID == id);
        }

        public async Task<PagedList<Employee>> GetAllAsync(string? name = null, string? jobTitle = null, string? orderBy = null, int pageIndex = 0, int pageSize = 20)
        {
            var query = _dbContext.Employees.AsQueryable();
            
            name = name?.Trim();
            if (!String.IsNullOrWhiteSpace(name))
                query = query.Where(x => x.Name.Contains(name));

            jobTitle = jobTitle?.Trim();
            if (!String.IsNullOrWhiteSpace(jobTitle))
                query = query.Where(x => x.Name.Contains(jobTitle));

            int totalCount = await query.CountAsync();

            if(!String.IsNullOrWhiteSpace(orderBy))
            {
                query = query.OrderBy("ASC", orderBy);
            }
            else
            {
                query = query.OrderBy(x => x.EmployeeID);
            }

            Employee[] employees = await query
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToArrayAsync();

            return new PagedList<Employee>(employees, pageIndex, pageSize, totalCount);
        }

        public async Task UpdateAsync(int id, string? name = null, string? jobTitle = null)
        {
            // To use 'disconnected' entities check this out
            // https://learn.microsoft.com/en-us/ef/core/saving/disconnected-entities

            Employee? employee = await _dbContext.Employees.SingleOrDefaultAsync(x => x.EmployeeID == id);
            if (employee != null)
            {
                name = name?.Trim();
                if (!String.IsNullOrWhiteSpace(name))
                    employee.Name = name;

                jobTitle = jobTitle?.Trim();
                if (!String.IsNullOrWhiteSpace(jobTitle))
                    employee.JobTitle = jobTitle;

                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            // To use 'disconnected' entities check this out
            // https://learn.microsoft.com/en-us/ef/core/saving/disconnected-entities

            Employee? employee = await _dbContext.Employees.SingleOrDefaultAsync(x => x.EmployeeID == id);
            if(employee != null)
            {
                _dbContext.Employees.Remove(employee);
                await _dbContext.SaveChangesAsync();
            }           
        }
    }
}
