using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace frm.Models;

public partial class FrmContext : DbContext
{
    public FrmContext(DbContextOptions<FrmContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
