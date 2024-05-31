using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using Company.Entities;
using Irvin.FormatFactory;
using Irvin.FormatFactory.Internal;
using Irvin.FormatFactory.Internal.Member;
using NUnit.Framework;

namespace TestProject;

[TestFixture]
public class TypeFactoryTests
{
    [Test]
    public void PropertyInfoWrapperEquals_ReturnsTrue_IfBasedOnTheSameProperty()
    {
        var a = new PropertyInfoWrapper(typeof(SimpleHeader).GetProperties().First());
        var b = new PropertyInfoWrapper(typeof(SimpleHeader).GetProperties().First());
        
        Assert.AreEqual(a,b);
        Assert.True(a.Equals(b));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }
    
    [Test]
    public void FieldInfoWrapperEquals_ReturnsTrue_IfBasedOnTheSameProperty()
    {
        var a = new FieldInfoWrapper(typeof(Stats).GetFields().First());
        var b = new FieldInfoWrapper(typeof(Stats).GetFields().First());
        
        Assert.AreEqual(a,b);
        Assert.True(a.Equals(b));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Test]
    public void ColumnWrapperEquals_ReturnsTrue_IfBasedOnSameColumn()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("ZORP", typeof(DateTime)));
        
        var a = new DataColumnWrapper(dataTable.Columns[0]);
        var b = new DataColumnWrapper(dataTable.Columns[0]);
        
        Assert.AreEqual(a,b);
        Assert.True(a.Equals(b));
        Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
    }
    
    [Test]
    [Timeout(60000)]
    public void Write_DoesNotExceedCachingLimits()
    {
        string probeContent = FormatWriter.Default.WriteSingle(SimpleHeaderTestFactory.Get());
        Assert.AreNotEqual(0, probeContent.Length);
        int expectedCacheCount = TypeFactory.MembersCached;

        Stopwatch st = new Stopwatch();
        
        for (int j = 0; j < 100; j++)
        {
            List<SimpleHeader> elements = new List<SimpleHeader>();
            for (int i = 0; i < 5000; i++)
            {
                elements.Add(SimpleHeaderTestFactory.Get());
            }
        
            st.Start();
            string content = FormatWriter.Default.Write(elements);
            st.Stop();
            Console.WriteLine(st.Elapsed.ToString());
            st.Reset();
            
            Assert.AreEqual(expectedCacheCount, TypeFactory.MembersCached);
            Assert.AreNotEqual(0, content.Length);
        }
    }
}