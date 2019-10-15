namespace WebFileSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "web_file_system.File",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Filename = c.String(nullable: false),
                        LocalFilePath = c.String(nullable: false),
                        Extension = c.String(),
                        Length = c.Long(nullable: false),
                        ContentType = c.String(),
                        AccessSymbolic = c.String(nullable: false),
                        Owner = c.String(),
                        GroupId = c.Int(),
                        Description = c.String(),
                        CreationTime = c.DateTime(nullable: false),
                        LastUpdationTime = c.DateTime(),
                        LastVisitTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("web_file_system.Group", t => t.GroupId)
                .Index(t => t.Filename)
                .Index(t => t.LocalFilePath)
                .Index(t => t.Extension)
                .Index(t => t.AccessSymbolic)
                .Index(t => t.GroupId)
                .Index(t => t.CreationTime);
            
            CreateTable(
                "web_file_system.Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreationTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CreationTime);
            
        }
        
        public override void Down()
        {
            DropForeignKey("web_file_system.File", "GroupId", "web_file_system.Group");
            DropIndex("web_file_system.Group", new[] { "CreationTime" });
            DropIndex("web_file_system.File", new[] { "CreationTime" });
            DropIndex("web_file_system.File", new[] { "GroupId" });
            DropIndex("web_file_system.File", new[] { "AccessSymbolic" });
            DropIndex("web_file_system.File", new[] { "Extension" });
            DropIndex("web_file_system.File", new[] { "LocalFilePath" });
            DropIndex("web_file_system.File", new[] { "Filename" });
            DropTable("web_file_system.Group");
            DropTable("web_file_system.File");
        }
    }
}
