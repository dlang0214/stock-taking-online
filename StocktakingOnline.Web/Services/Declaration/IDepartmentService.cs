using System;
using System.Threading.Tasks;
using StocktakingOnline.Web.Models.Domain;
using System.Collections.Generic;
namespace StocktakingOnline.Web.Services.Declaration
{
	public interface IDepartmentService
	{
		Task<List<Department>> GetDepartments();
		Task<Department> GetDepartmentById(int departmentId);
	}
}
