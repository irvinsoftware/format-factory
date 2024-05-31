using System;
using System.Collections.Generic;
using Company.Entities;

namespace TestProject;

internal static class SimpleHeaderTestFactory
{
    public static SimpleHeader Get()
    {
        SimpleHeader simpleHeader = new SimpleHeader
        {
            FamilyName = "Hardy",
            MarriageDate = new DateTime(1983, 2, 9),
            Parents = new List<SimpleFooter>
            {
                new SimpleFooter { FirstName = "Tom", LastName = "Hardy", Age = 35},
                new SimpleFooter { FirstName = "Sheryl", LastName = "Hardy", Age = 33},
            }
        };
			
        simpleHeader.Children.Add(new SimpleFooter { FirstName = "Laurel", LastName = "Hardy", Age = 4});
        simpleHeader.Children.Add(new SimpleFooter { FirstName = "Tom Jr.", LastName = "Hardy", Age = 8 });
			
        return simpleHeader;
    }
}