﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetGore.Db.QueryBuilder;

namespace NetGore.Db.MySql.QueryBuilder
{
    /// <summary>
    /// An <see cref="IQueryBuilderFunctions"/> implementation for MySql.
    /// </summary>
    public class MySqlQueryBuilderFunctions : IQueryBuilderFunctions
    {
        static readonly MySqlQueryBuilderFunctions _instance;

        /// <summary>
        /// Initializes the <see cref="MySqlQueryBuilderFunctions"/> class.
        /// </summary>
        static MySqlQueryBuilderFunctions()
        {
            _instance = new MySqlQueryBuilderFunctions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MySqlQueryBuilderFunctions"/> class.
        /// </summary>
        MySqlQueryBuilderFunctions()
        {
        }

        /// <summary>
        /// Gets the <see cref="MySqlQueryBuilderFunctions"/> instance.
        /// </summary>
        public static MySqlQueryBuilderFunctions Instance
        {
            get { return _instance; }
        }

        #region IQueryBuilderFunctions Members

        /// <summary>
        /// Gets the absolute value of the <paramref name="expr"/>.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string Abs(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return "ABS(" + expr + ")";
        }

        /// <summary>
        /// Equality operator.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Equals(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " = " + right;
        }

        /// <summary>
        /// Inequality operator.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string NotEqual(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " != " + right;
        }

        /// <summary>
        /// Adds two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Add(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " + " + right;
        }

        /// <summary>
        /// Performs a logical AND.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string And(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " AND " + right;
        }

        /// <summary>
        /// Performs a bitwise AND.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string BitAnd(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " & " + right;
        }

        /// <summary>
        /// Performs a bitwise NOT (bit inversion).
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string BitNot(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return "~" + expr;
        }

        /// <summary>
        /// Performs a bitwise OR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string BitOr(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " | " + right;
        }

        /// <summary>
        /// Performs a bitwise XOR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string BitXor(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " ^ " + right;
        }

        /// <summary>
        /// Rounds a value up to the nearest integer.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string Ceiling(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return "CEILING(" + expr + ")";
        }

        /// <summary>
        /// Tests if an expression is not null.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string IsNotNull(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return expr + " IS NOT NULL";
        }

        /// <summary>
        /// Returns the first non-null value in a list.
        /// </summary>
        /// <param name="exprs">The SQL expressions, where each array element is an individual argument for the function.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exprs"/> is null or empty.</exception>
        public string Coalesce(IEnumerable<string> exprs)
        {
            if (exprs == null || exprs.Count() == 0)
                throw new ArgumentNullException("exprs");

            var sb = new StringBuilder();
            sb.Append("COALESCE(");
            foreach (var v in exprs)
            {
                sb.Append(v);
                sb.Append(",");
            }
            sb.Length--;
            sb.Append(")");

            return sb.ToString();
        }

        /// <summary>
        /// Returns the first non-null value in a list.
        /// </summary>
        /// <param name="exprs">The SQL expressions, where each array element is an individual argument for the function.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="exprs"/> is null or empty.</exception>
        public string Coalesce(params string[] exprs)
        {
            return Coalesce((IEnumerable<string>)exprs);
        }

        /// <summary>
        /// Counts the number of rows.
        /// </summary>
        /// <returns>The SQL string for the function.</returns>
        public string Count()
        {
            return "COUNT(*)";
        }

        /// <summary>
        /// Counts the number of non-null values in the <paramref name="expr"/>.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string Count(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return "COUNT(" + expr + ")";
        }

        /// <summary>
        /// Gets the default value for a column.
        /// </summary>
        /// <param name="expr">The SQL expression, typically the column name to get the default value for.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string Default(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return "DEFAULT(" + expr + ")";
        }

        /// <summary>
        /// Divides two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Divide(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " / " + right;
        }

        /// <summary>
        /// Rounds a value down to the nearest integer.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string Floor(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return "FLOOR(" + expr + ")";
        }

        /// <summary>
        /// Tests if an expression is null.
        /// </summary>
        /// <param name="expr">The SQL expression.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="expr"/> is null or empty.</exception>
        public string IsNull(string expr)
        {
            if (string.IsNullOrEmpty(expr))
                throw new ArgumentNullException("expr");

            return expr + " IS NULL";
        }

        /// <summary>
        /// Gets the remainder of a division.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Mod(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " % " + right;
        }

        /// <summary>
        /// Multiplies two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Multiply(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " * " + right;
        }

        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        /// <returns>The SQL string for the function.</returns>
        public string Now()
        {
            return "NOW()";
        }

        /// <summary>
        /// Performs a logical OR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Or(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " OR " + right;
        }

        /// <summary>
        /// Subtracts two values.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Subtract(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " - " + right;
        }

        /// <summary>
        /// Performs a logical XOR.
        /// </summary>
        /// <param name="left">The left argument.</param>
        /// <param name="right">The right argument.</param>
        /// <returns>The SQL string for the function.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="left"/> is null or empty.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="right"/> is null or empty.</exception>
        public string Xor(string left, string right)
        {
            if (string.IsNullOrEmpty(left))
                throw new ArgumentNullException("left");
            if (string.IsNullOrEmpty(right))
                throw new ArgumentNullException("right");

            return left + " XOR " + right;
        }

        #endregion
    }
}