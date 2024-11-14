using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Ukid.Domain.Common;

namespace Infrastructure.Common;

public class SnakeNamingConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(IConventionModelBuilder builder, IConventionContext<IConventionModelBuilder> context)
    {
        foreach (var entity in builder.Metadata.GetEntityTypes())
        {
            if (!typeof(BaseEntity).IsAssignableFrom(entity.ClrType))
            {
                continue;
            }

            var dbEntity = builder.Entity(entity.Name);
            if (dbEntity is null) continue;
            
            // get the name of DbSet<TEntity> instead of TEntity
            var currentTableName = dbEntity.Metadata.GetTableName();
            if (!string.IsNullOrEmpty(currentTableName) && !entity.IsAbstract())
            {
                dbEntity.ToTable(GetSnakeName(currentTableName));
            }
            
            foreach (var property in entity.GetDeclaredProperties())
            {
                property.Builder.HasColumnName(GetSnakeName(property.Name));
            }
        }
    }
    
    private string GetSnakeName(string name)
    {
        return string.Concat(
            name.Select((x, i) => i > 0 && char.IsUpper(x)
                ? $"_{x}"
                : x.ToString())).ToLower();
    }
}
