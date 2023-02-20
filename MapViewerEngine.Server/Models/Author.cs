﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MapViewerEngine.Server.Models;

public class Author
{
    public int Id { get; private set; }

    [StringLength(255)]
    public required string Name { get; set; }

    [JsonIgnore] public ICollection<OfficialBlock> OfficialBlocks { get; set; } = Array.Empty<OfficialBlock>();
    [JsonIgnore] public ICollection<OfficialItemMesh> OfficialItemMeshes { get; set; } = Array.Empty<OfficialItemMesh>();
}
