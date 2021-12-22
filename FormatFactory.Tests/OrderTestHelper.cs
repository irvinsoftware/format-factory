using System;
using Company.Entities;

namespace TestProject
{
    internal static class OrderTestHelper
    {
        internal static Order GetOrder(int i = 1)
        {
            Order subject = new Order();
            subject.OrderNumber = String.Format("ABCD{0}", i);
            subject.OrderTotal = 35m + i;
            subject.Ordered = new DateTime(2007, 12, 30).AddDays(i);
            subject.ShippingName = String.Format("Tom Hardy {0}", i);
            subject.IsActive = true;
            return subject;
        }

        internal static Order2 GetOrder2(int i = 1)
        {
            Order2 subject = new Order2();
            subject.OrderNumber = String.Format("ABCD{0}", i);
            subject.OrderTotal = 35m + i;
            subject.Ordered = new DateTime(2007, 12, 30).AddDays(i);
            subject.ShippingName = String.Format("Tom Hardy {0}", i);
            subject.IsActive = true;
            return subject;
        }

        internal static Order3 GetOrder3(int i = 1)
        {
            Order3 subject = new Order3();
            subject.OrderNumber = String.Format("ABCD{0}", i);
            subject.OrderTotal = 35m + i;
            subject.Ordered = new DateTime(2007, 12, 30).AddDays(i);
            subject.ShippingName = String.Format("Tom Hardy {0}", i);
            subject.IsActive = true;
            return subject;
        }

        internal static OrderB GetOrderB(int i = 1)
        {
            OrderB subject = new OrderB();
            subject.OrderNumber = String.Format("ABCD{0}", i);
            subject.OrderTotal = 35m + i;
            subject.Ordered = new DateTime(2007, 12, 30).AddDays(i);
            subject.ShippingName = String.Format("Tom Hardy {0}", i);
            subject.IsActive = true;
            return subject;
        }

        internal static OrderC GetOrderC(int i = 1)
        {
            OrderC subject = new OrderC();
            subject.OrderNumber = String.Format("ABCD{0}", i);
            subject.OrderTotal = 35m + i;
            subject.Ordered = new DateTime(2007, 12, 30).AddDays(i);
            subject.ShippingName = String.Format("Tom Hardy {0}", i);
            subject.IsActive = true;
            return subject;
        }

        internal static OrderQ GetOrderQ(int i = 1)
        {
            OrderQ subject = new OrderQ();
            subject.OrderNumber = String.Format("ABCD{0}", i);
            subject.OrderTotal = 35m + i;
            subject.Ordered = new DateTime(2007, 12, 30).AddDays(i);
            subject.ShippingName = String.Format("Tom Hardy {0}", i);
            return subject;
        }
    }
}