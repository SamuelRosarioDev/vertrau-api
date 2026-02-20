using FluentMigrator;

namespace ApiVertrau.Infrastructure.Migrations;

[Migration(001)]
public class CreateUsersTable : Migration
{
    public override void Up()
    {
        Create
            .Table("user")
            .WithColumn("id")
            .AsInt64()
            .PrimaryKey()
            .Identity()
            .WithColumn("nome")
            .AsString()
            .NotNullable()
            .WithColumn("sobrenome")
            .AsString()
            .NotNullable()
            .WithColumn("email")
            .AsString()
            .NotNullable()
            .Unique()
            .WithColumn("genero")
            .AsInt32()
            .NotNullable()
            .WithColumn("datanascimento")
            .AsString()
            .Nullable()
            .WithColumn("createdat")
            .AsDateTime()
            .NotNullable();
    }

    public override void Down()
    {
        Delete.Table("user");
    }
}
