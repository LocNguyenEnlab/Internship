namespace TodoAppPhase3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAuthor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Author",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Task", "AuthorName", c => c.String());
            AddColumn("dbo.Task", "Author_Id", c => c.Int());
            CreateIndex("dbo.Task", "Author_Id");
            AddForeignKey("dbo.Task", "Author_Id", "dbo.Author", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task", "Author_Id", "dbo.Author");
            DropIndex("dbo.Task", new[] { "Author_Id" });
            DropColumn("dbo.Task", "Author_Id");
            DropColumn("dbo.Task", "AuthorName");
            DropTable("dbo.Author");
        }
    }
}
