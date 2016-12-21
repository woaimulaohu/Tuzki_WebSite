namespace MyWebProject.Models.Entity
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	public partial class Entity : DbContext
	{
		public Entity()
			: base("name=Entity")
		{
		}

		public virtual DbSet<COMMENTS> COMMENTS { get; set; }
		public virtual DbSet<POST_CONTENT> POST_CONTENT { get; set; }
		public virtual DbSet<POST_INFO> POST_INFO { get; set; }
		public virtual DbSet<TAG_INFO> TAG_INFO { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<COMMENTS>()
				.Property(e => e.EMAIL)
				.IsUnicode(false);

			modelBuilder.Entity<COMMENTS>()
				.HasMany(e => e.COMMENTS1)
				.WithRequired(e => e.COMMENTS2)
				.HasForeignKey(e => e.POST_ID);

			modelBuilder.Entity<POST_CONTENT>()
				.Property(e => e.POST_CONTENT1)
				.IsUnicode(false);
		}
	}
}