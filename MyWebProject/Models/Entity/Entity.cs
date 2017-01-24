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
		public virtual DbSet<CONFIG> CONFIG { get; set; }
		public virtual DbSet<POST_CONTENT> POST_CONTENT { get; set; }
		public virtual DbSet<POST_INFO> POST_INFO { get; set; }
		public virtual DbSet<STATISTICS> STATISTICS { get; set; }
		public virtual DbSet<TAG_INFO> TAG_INFO { get; set; }
		public virtual DbSet<USER_INFO> USER_INFO { get; set; }
		public virtual DbSet<MENU> MENU { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<COMMENTS>()
				.Property(e => e.TEXT)
				.IsUnicode(false);

			modelBuilder.Entity<COMMENTS>()
				.Property(e => e.EMAIL)
				.IsUnicode(false);

			modelBuilder.Entity<COMMENTS>()
				.Property(e => e.AVATAR_URL)
				.IsUnicode(false);

			modelBuilder.Entity<POST_CONTENT>()
				.Property(e => e.POST_CONTENT1)
				.IsUnicode(false);

			modelBuilder.Entity<POST_INFO>()
				.Property(e => e.TAG_ID)
				.IsUnicode(false);

			modelBuilder.Entity<POST_INFO>()
				.Property(e => e.SECOND_TITLE)
				.IsFixedLength();
		}
	}
}
