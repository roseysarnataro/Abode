﻿using System;
using System.Collections.Generic;

namespace Abode.Models;

public partial class OldTenant
{
    public int PersonId { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }
}