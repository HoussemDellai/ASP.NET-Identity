namespace AspNetIdentity.WebApi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedUserToDosAndToDo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Todoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Priority = c.Int(nullable: false),
                        Status = c.String(),
                        Category = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdatedAt = c.DateTime(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Todoes", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Todoes", new[] { "User_Id" });
            DropTable("dbo.Todoes");
        }
    }
}
