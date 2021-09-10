using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using APGLogs.Domain.Models;

namespace APGLogs.Infra.Data.Mappings
{    
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Id)
                .HasColumnName("Id");

            builder.Property(c => c.Name)
                .HasColumnType("varchar(150)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();   
        }
    }
}
