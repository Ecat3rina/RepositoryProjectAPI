using RepositoryApplication.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryApplication.DAL.Context
{
    public class FirstContext : DbContext
    {
        public FirstContext() : base("Connection")
        {
            Database.SetInitializer(new MyDbInitializer());
        }


        public  DbSet<DMDepartment> Departments { get; set; }
        public  DbSet<DMProject> Projects { get; set; }
        public  DbSet<DMRepository> Repositories { get; set; }
        public  DbSet<DMRepositoryType> RepositoryTypes { get; set; }
        public  DbSet<DMRole> Roles { get; set; }
        public  DbSet<DMUser> Users { get; set; }
        public  DbSet<DMUserRole> UserRoles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region Department
            modelBuilder.Entity<DMDepartment>()
                .HasMany(x => x.Projects)
                .WithRequired(x => x.Department)
                .HasForeignKey(x => x.DepartmentId)
                .WillCascadeOnDelete(false);
            #endregion Department

            #region Project
            modelBuilder.Entity<DMProject>()
                .HasMany(x => x.Repositories)
                .WithRequired(x => x.Project)
                .HasForeignKey(x => x.ProjectId)
                .WillCascadeOnDelete(false);
            #endregion Project

            #region RepositoryType
            modelBuilder.Entity<DMRepositoryType>()
                .HasMany(x => x.Repositories)
                .WithRequired(x => x.Type)
                .HasForeignKey(x => x.TypeId)
                .WillCascadeOnDelete(false);
            #endregion RepositoryType

            #region Repository

            #endregion Repository

            #region Role
            modelBuilder.Entity<DMRole>()
                .HasMany(x => x.UserRoles)
                .WithRequired(x => x.Role)
                .HasForeignKey(x => x.RoleId)
                .WillCascadeOnDelete(false);
            #endregion Role

            #region User
            modelBuilder.Entity<DMUser>()
                .HasMany(x => x.Repositories)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DMUser>()
                .HasMany(x => x.Projects)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DMUser>()
                .HasMany(x => x.UserRoles)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId)
                .WillCascadeOnDelete(false);
            #endregion User


            base.OnModelCreating(modelBuilder);
        }
        class MyDbInitializer : CreateDatabaseIfNotExists<FirstContext>
        {
            protected override void Seed(FirstContext context)
            {

                context.Users.Add(new DMUser { Id = Guid.Parse("{C41722D4-482E-42D2-B24B-50CBBE387996}"), UserName = "user", FullName = "Catea", Email = "catea@mail", Password = "cat" });
                context.Roles.Add(new DMRole { Id=Guid.Parse("{5AE6D943-6DD5-451A-9C64-4CAF213559F6}"), Name="Admin"});
                context.UserRoles.Add(new DMUserRole { UserId= Guid.Parse("{C41722D4-482E-42D2-B24B-50CBBE387996}"), RoleId= Guid.Parse("{5AE6D943-6DD5-451A-9C64-4CAF213559F6}") });

                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}
