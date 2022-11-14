﻿using Microsoft.AspNetCore.Authorization;

namespace ASP12_RazorPage_EntityFramework.Security.Requirements;

public class GenZRequirement : IAuthorizationRequirement
{
    public GenZRequirement(int fromYear=1997, int toYear=2012)
    {
        FromYear = fromYear;
        ToYear = toYear;
    }

    public int FromYear { get; set; }
    public int ToYear { get; set; }
}