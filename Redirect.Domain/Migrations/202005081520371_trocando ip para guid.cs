namespace Redirect.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trocandoipparaguid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Registro", "Guid", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Registro", "Ip");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Registro", "Ip", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.Registro", "Guid");
        }
    }
}
