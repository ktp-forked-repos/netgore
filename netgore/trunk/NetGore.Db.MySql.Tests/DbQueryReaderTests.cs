﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using NUnit.Framework;

namespace NetGore.Db.MySql.Tests
{
    struct MyReaderValues
    {
        public int A;
        public int B;
        public int C;

        public MyReaderValues(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }
    }

    class MyReader : DbQueryReader<QueryTestValues>
    {
        const string _commandText = "SELECT @a + @b + @c";
 
        public MyReader(DbConnectionPool connectionPool)
            : base(connectionPool, _commandText)
        {
        }

        protected override void SetParameters(DbParameterValues p, QueryTestValues item)
        {
            p["@a"] = item.A;
            p["@b"] = item.B;
            p["@c"] = item.C;
        }

        protected override IEnumerable<DbParameter> InitializeParameters()
        {
            return CreateParameters("@a", "@b", "@c");
        }
    }

    [TestFixture]
    public class DbQueryReaderTests
    {
        static MyReader CreateReader() { return new MyReader(TestSettings.CreateConnectionPool()); }

        [Test]
        public void SelectTest()
        {
            using (var reader = CreateReader())
            {
                var cp = reader.ConnectionPool;

                for (int i = 0; i < 100; i++)
                {
                    Assert.AreEqual(0, cp.Count);
                    using (var r = reader.ExecuteReader(new QueryTestValues(5, 10, 15)))
                    {
                        Assert.AreEqual(1, cp.Count);
                        Assert.IsTrue(r.Read());
                        Assert.AreEqual(5 + 10 + 15, r[0]);
                    }
                }
            }
        }

        static void SelectTestRecurse(IDbQueryReader<QueryTestValues> reader, int depth, int initialDepth)
        {
            var cp = reader.ConnectionPool;
            var v = new QueryTestValues(depth * 2, depth - 2, depth + 57);
            int expectedPoolSize = initialDepth - depth;

            Assert.AreEqual(expectedPoolSize, cp.Count);
            using (var r = reader.ExecuteReader(v))
            {
                expectedPoolSize++;

                Assert.AreEqual(expectedPoolSize, cp.Count);
                Assert.IsTrue(r.Read());
                Assert.AreEqual(v.A + v.B + v.C, r[0]);
                if (depth > 0)
                    SelectTestRecurse(reader, depth - 1, initialDepth);
                Assert.AreEqual(expectedPoolSize, cp.Count);

                expectedPoolSize--;
            }
            Assert.AreEqual(expectedPoolSize, cp.Count);
        }

        static void SelectTestRecurse(IDbQueryReader<QueryTestValues> reader, int depth)
        {
            SelectTestRecurse(reader, depth, depth);
        }

        [Test]
        public void ConcurrentSelectTest()
        {
            // Tests 3 concurrently open selects 100 times total

            using (var reader = CreateReader())
            {
                for (int i = 0; i < 100; i++)
                {
                    SelectTestRecurse(reader, 3);
                }
            }
        }

        [Test]
        public void DisposeTest()
        {
            MyReader reader = CreateReader();

            using (reader)
            {
                SelectTestRecurse(reader, 1);
            }

            Assert.IsTrue(reader.IsDisposed);
        }

        [Test]
        public void SuperConcurrentSelectTest()
        {
            // Recurses 50 levels deep, resulting in 50 concurrent selects being open

            using (var reader = CreateReader())
            {
                SelectTestRecurse(reader, 50);
            }
        }
    }
}
