using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Survey.Core.Entities;

namespace Survey.Infrastructure.Database
{
	public class ContextDb : IdentityDbContext<UserIdentity>
	{
		public ContextDb()
			: base("name=Survey")
		{

		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
