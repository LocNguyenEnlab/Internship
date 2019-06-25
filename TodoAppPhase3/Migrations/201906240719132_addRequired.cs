namespace TodoAppPhase3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRequired : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Task", "Author_Id", "dbo.Author");
            DropIndex("dbo.Task", new[] { "Author_Id" });
            AlterColumn("dbo.Task", "AuthorName", c => c.String(nullable: false));
            AlterColumn("dbo.Task", "Author_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Task", "Author_Id");
            AddForeignKey("dbo.Task", "Author_Id", "dbo.Author", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Task", "Author_Id", "dbo.Author");
            DropIndex("dbo.Task", new[] { "Author_Id" });
            AlterColumn("dbo.Task", "Author_Id", c => c.Int());
            AlterColumn("dbo.Task", "AuthorName", c => c.String());
            CreateIndex("dbo.Task", "Author_Id");
            AddForeignKey("dbo.Task", "Author_Id", "dbo.Author", "Id");
        }
    }
}
