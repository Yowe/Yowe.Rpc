namespace Vice.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Base_Article",
                c => new
                    {
                        ArticleId = c.String(nullable: false, maxLength: 128),
                        ArticleTitle = c.String(),
                        ArticleAbstract = c.String(),
                        ArticleContent = c.String(),
                        ArticleCategoryId = c.String(),
                        ArticleStatistical = c.Int(nullable: false),
                        ArticleImages = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        Author = c.String(),
                    })
                .PrimaryKey(t => t.ArticleId);
            
            CreateTable(
                "dbo.Base_Category",
                c => new
                    {
                        CategoryId = c.String(nullable: false, maxLength: 128),
                        CategoryName = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Base_Schedule",
                c => new
                    {
                        DutyId = c.String(nullable: false, maxLength: 128),
                        WeekDay = c.String(),
                        UserName = c.String(),
                        Memo = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.DutyId);
            
            CreateTable(
                "dbo.Base_ImageLinks",
                c => new
                    {
                        ImageId = c.String(nullable: false, maxLength: 128),
                        ImageUrl = c.String(),
                        Title = c.String(),
                        Abstract = c.String(),
                        Author = c.String(),
                        Content = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.ImageId);
            
            CreateTable(
                "dbo.Base_Notify",
                c => new
                    {
                        Notify_Id = c.String(nullable: false, maxLength: 128),
                        Notify_Content = c.String(),
                        Notify_ShortDate = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Notify_Id);
            
            CreateTable(
                "dbo.Base_ScrollImage",
                c => new
                    {
                        SrollImageId = c.String(nullable: false, maxLength: 128),
                        ScrollTitle = c.String(),
                        ScrollImageUrl = c.String(),
                        ScrollAbstract = c.String(),
                        ScrollContent = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                        ScrollStatistical = c.Int(nullable: false),
                        Author = c.String(),
                    })
                .PrimaryKey(t => t.SrollImageId);
            
            CreateTable(
                "dbo.Base_User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        AccountName = c.String(),
                        UserIcon = c.String(),
                        UserPass = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.Base_SuperStar",
                c => new
                    {
                        StarId = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(),
                        Photo_Url = c.String(),
                        SelectDate = c.String(),
                        CurrentDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StarId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Base_SuperStar");
            DropTable("dbo.Base_User");
            DropTable("dbo.Base_ScrollImage");
            DropTable("dbo.Base_Notify");
            DropTable("dbo.Base_ImageLinks");
            DropTable("dbo.Base_Schedule");
            DropTable("dbo.Base_Category");
            DropTable("dbo.Base_Article");
        }
    }
}
