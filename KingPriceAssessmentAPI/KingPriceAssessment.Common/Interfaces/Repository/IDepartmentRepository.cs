using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KingPriceAssessment.Data.Models.Request.Add;
using KingPriceAssessment.Data.Models.Request.Update;
using KingPriceAssessment.Data.Tables;

namespace KingPriceAssessment.Common.Interfaces.Repository
{
    public interface IDepartmentRepository
    {
        public Task<IEnumerable<Department>> GetAllAsync();
        public Task<Department> GetByIdAsync(int id);
        public Task AddAsync(AddDepartmentRequest department);
        public Task UpdateAsync(UpdateDepartmentRequest department);
        public Task DeleteAsync(int id);
    }
}
