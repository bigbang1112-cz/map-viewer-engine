﻿using System.ComponentModel.DataAnnotations;

namespace MapViewerEngine.Server.Models;

public class Author
{
    public int Id { get; set; }

    [StringLength(255)]
    public required string Name { get; set; }
}
