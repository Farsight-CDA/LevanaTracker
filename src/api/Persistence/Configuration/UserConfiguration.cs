﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LevanaTracker.Api.Models;

namespace LevanaTracker.Api.Persistence.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        _ = builder.Property(x => x.Id);
        _ = builder.HasKey(x => x.Id);

        _ = builder.ToTable("Users");
    }
}
