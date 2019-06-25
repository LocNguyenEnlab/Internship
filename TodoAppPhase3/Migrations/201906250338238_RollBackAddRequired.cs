namespace TodoAppPhase3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RollBackAddRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Author", "AuthorName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Author", "AuthorName", c => c.String());
        }
    }
}
