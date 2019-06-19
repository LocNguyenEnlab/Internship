namespace TodoAppPhase3.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class createDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Task",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Description = c.String(),
                    TimeCreate = c.DateTime(nullable: false),
                    TypeList = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Task");
        }
    }
}
