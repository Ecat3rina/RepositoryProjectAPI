namespace RepoApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class third : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DMUserRoles", "UserId", "dbo.DMUsers");
            AddForeignKey("dbo.DMUserRoles", "UserId", "dbo.DMUsers", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DMUserRoles", "UserId", "dbo.DMUsers");
            AddForeignKey("dbo.DMUserRoles", "UserId", "dbo.DMUsers", "Id");
        }
    }
}
