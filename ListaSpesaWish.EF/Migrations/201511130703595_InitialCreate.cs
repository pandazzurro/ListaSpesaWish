namespace ListaSpesaWish.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ListaSpesa",
                c => new
                    {
                        IdListaSpesa = c.Long(nullable: false, identity: true),
                        Nome = c.String(),
                    })
                .PrimaryKey(t => t.IdListaSpesa);
            
            CreateTable(
                "dbo.UtentiListaSpesa",
                c => new
                    {
                        IdUtentiListaSpesa = c.Long(nullable: false, identity: true),
                        Utente_IdUtente = c.Long(nullable: false),
                        ListaSpesa_IdListaSpesa = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.IdUtentiListaSpesa)
                .ForeignKey("dbo.Utente", t => t.Utente_IdUtente, cascadeDelete: true)
                .ForeignKey("dbo.ListaSpesa", t => t.ListaSpesa_IdListaSpesa, cascadeDelete: true)
                .Index(t => t.Utente_IdUtente)
                .Index(t => t.ListaSpesa_IdListaSpesa);
            
            CreateTable(
                "dbo.Utente",
                c => new
                    {
                        IdUtente = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.IdUtente);
            
            CreateTable(
                "dbo.VoceListaSpesa",
                c => new
                    {
                        IdVoceListaSpesa = c.Long(nullable: false, identity: true),
                        Comprata = c.Boolean(nullable: false),
                        Voce_IdVoce = c.Long(nullable: false),
                        ListaSpesa_IdListaSpesa = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.IdVoceListaSpesa)
                .ForeignKey("dbo.Voce", t => t.Voce_IdVoce, cascadeDelete: true)
                .ForeignKey("dbo.ListaSpesa", t => t.ListaSpesa_IdListaSpesa, cascadeDelete: true)
                .Index(t => t.Voce_IdVoce)
                .Index(t => t.ListaSpesa_IdListaSpesa);
            
            CreateTable(
                "dbo.Voce",
                c => new
                    {
                        IdVoce = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.IdVoce);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VoceListaSpesa", "ListaSpesa_IdListaSpesa", "dbo.ListaSpesa");
            DropForeignKey("dbo.VoceListaSpesa", "Voce_IdVoce", "dbo.Voce");
            DropForeignKey("dbo.UtentiListaSpesa", "ListaSpesa_IdListaSpesa", "dbo.ListaSpesa");
            DropForeignKey("dbo.UtentiListaSpesa", "Utente_IdUtente", "dbo.Utente");
            DropIndex("dbo.VoceListaSpesa", new[] { "ListaSpesa_IdListaSpesa" });
            DropIndex("dbo.VoceListaSpesa", new[] { "Voce_IdVoce" });
            DropIndex("dbo.UtentiListaSpesa", new[] { "ListaSpesa_IdListaSpesa" });
            DropIndex("dbo.UtentiListaSpesa", new[] { "Utente_IdUtente" });
            DropTable("dbo.Voce");
            DropTable("dbo.VoceListaSpesa");
            DropTable("dbo.Utente");
            DropTable("dbo.UtentiListaSpesa");
            DropTable("dbo.ListaSpesa");
        }
    }
}
