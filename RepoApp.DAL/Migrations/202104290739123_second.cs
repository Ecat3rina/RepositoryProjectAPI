namespace RepoApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Departments", newName: "DMDepartments");
            RenameTable(name: "dbo.Projects", newName: "DMProjects");
            RenameTable(name: "dbo.Repositories", newName: "DMRepositories");
            RenameTable(name: "dbo.RepositoryTypes", newName: "DMRepositoryTypes");
            RenameTable(name: "dbo.Users", newName: "DMUsers");
            RenameTable(name: "dbo.UserRoles", newName: "DMUserRoles");
            RenameTable(name: "dbo.Roles", newName: "DMRoles");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DMRoles", newName: "Roles");
            RenameTable(name: "dbo.DMUserRoles", newName: "UserRoles");
            RenameTable(name: "dbo.DMUsers", newName: "Users");
            RenameTable(name: "dbo.DMRepositoryTypes", newName: "RepositoryTypes");
            RenameTable(name: "dbo.DMRepositories", newName: "Repositories");
            RenameTable(name: "dbo.DMProjects", newName: "Projects");
            RenameTable(name: "dbo.DMDepartments", newName: "Departments");
        }
    }
}
