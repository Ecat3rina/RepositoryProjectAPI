namespace RepoApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class usernameAddedToDMProject : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DMProjects", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DMProjects", "UserName");
        }
    }
}
