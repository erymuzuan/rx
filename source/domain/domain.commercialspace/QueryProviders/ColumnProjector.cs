using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Bespoke.Station.Domain.QueryProviders {
    public sealed class ProjectedColumns
    {
        readonly Expression m_projector;
        readonly ReadOnlyCollection<ColumnDeclaration> m_columns;
        internal ProjectedColumns(Expression projector, ReadOnlyCollection<ColumnDeclaration> columns) {
            this.m_projector = projector;
            this.m_columns = columns;
        }
        internal Expression Projector {
            get { return this.m_projector; }
        }
        internal ReadOnlyCollection<ColumnDeclaration> Columns {
            get { return this.m_columns; }
        }
    }

    /// <summary>
    /// ColumnProjection is a visitor that splits an expression representing the result of a query into 
    /// two parts, a list of column declarations of expressions that must be evaluated on the server
    /// and a projector expression that describes how to combine the columns back into the result object
    /// </summary>
    internal class ColumnProjector : DbExpressionVisitor {
        readonly Nominator m_nominator;
        Dictionary<ColumnExpression, ColumnExpression> m_map;
        List<ColumnDeclaration> m_columns;
        HashSet<string> m_columnNames;
        HashSet<Expression> m_candidates;
        string[] m_existingAliases;
        string m_newAlias;
        int m_iColumn;

        internal ColumnProjector(Func<Expression, bool> fnCanBeColumn) {
            this.m_nominator = new Nominator(fnCanBeColumn);
        }

        internal ProjectedColumns ProjectColumns(Expression expression, string newAlias, params string[] existingAliases) {
            this.m_map = new Dictionary<ColumnExpression, ColumnExpression>();
            this.m_columns = new List<ColumnDeclaration>();
            this.m_columnNames = new HashSet<string>();
            this.m_newAlias = newAlias;
            this.m_existingAliases = existingAliases;
            this.m_candidates = this.m_nominator.Nominate(expression);
            return new ProjectedColumns(this.Visit(expression), this.m_columns.AsReadOnly());
        }

        protected override Expression Visit(Expression expression)
        {
            if (this.m_candidates.Contains(expression)) {
                if (expression.NodeType == (ExpressionType)DbExpressionType.Column) {
                    var column = (ColumnExpression)expression;
                    ColumnExpression mapped;
                    if (this.m_map.TryGetValue(column, out mapped)) {
                        return mapped;
                    }
                    if (this.m_existingAliases.Contains(column.Alias)) {
                        int ordinal = this.m_columns.Count;
                        string columnName = this.GetUniqueColumnName(column.Name);
                        this.m_columns.Add(new ColumnDeclaration(columnName, column));
                        mapped = new ColumnExpression(column.Type, this.m_newAlias, columnName, ordinal);
                        this.m_map[column] = mapped;
                        this.m_columnNames.Add(columnName);
                        return mapped;
                    }
                    // must be referring to outer scope
                    return column;
                }
                else {
                    string columnName = this.GetNextColumnName();
                    int ordinal = this.m_columns.Count;
                    this.m_columns.Add(new ColumnDeclaration(columnName, expression));
                    return new ColumnExpression(expression.Type, this.m_newAlias, columnName, ordinal);
                }
            }
            return base.Visit(expression);
        }

        private bool IsColumnNameInUse(string name) {
            return this.m_columnNames.Contains(name);
        }

        private string GetUniqueColumnName(string name) {
            string baseName = name;
            int suffix = 1;
            while (this.IsColumnNameInUse(name)) {
                name = baseName + (suffix++);
            }
            return name;
        }

        private string GetNextColumnName() {
            return this.GetUniqueColumnName("c" + (m_iColumn++));
        }

        /// <summary>
        /// Nominator is a class that walks an expression tree bottom up, determining the set of 
        /// candidate expressions that are possible columns of a select expression
        /// </summary>
        class Nominator : DbExpressionVisitor {
            readonly Func<Expression, bool> m_fnCanBeColumn;
            bool m_isBlocked;
            HashSet<Expression> candidates;

            internal Nominator(Func<Expression, bool> fnCanBeColumn) {
                this.m_fnCanBeColumn = fnCanBeColumn;
            }

            internal HashSet<Expression> Nominate(Expression expression) {
                this.candidates = new HashSet<Expression>();
                this.m_isBlocked = false;
                this.Visit(expression);
                return this.candidates;
            }

            protected override Expression Visit(Expression expression) {
                if (expression != null) {
                    bool saveIsBlocked = this.m_isBlocked;
                    this.m_isBlocked = false;
                    base.Visit(expression);
                    if (!this.m_isBlocked) {
                        if (this.m_fnCanBeColumn(expression)) {
                            this.candidates.Add(expression);
                        }
                        else {
                            this.m_isBlocked = true;
                        }
                    }
                    this.m_isBlocked |= saveIsBlocked;
                }
                return expression;
            }
        }
    }
}
