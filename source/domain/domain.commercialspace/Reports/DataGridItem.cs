using System.Collections.Generic;
using System.Linq;

namespace Bespoke.SphCommercialSpaces.Domain
{
    public partial class DataGridItem : ReportItem
    {
        public override void SetRows(ObjectCollection<ReportRow> rows)
        {
            this.ReportRowCollection.ClearAndAddRange(rows);
        }

        public IEnumerable<DataGridGroup> GetGroupHeaders()
        {
            var list = new ObjectCollection<DataGridGroup>();
            if (this.DataGridGroupDefinitionCollection.Count == 0) return list;



            foreach (var gf in this.DataGridGroupDefinitionCollection)
            {
                var gf1 = gf;
                var colvalues = this.ReportRowCollection
                                    .Where(r => r[gf1.Column] != null)
                                    .Select(r => r[gf1.Column].Value)
                                    .Where(o => null != o)
                                    .OrderBy(o => o)
                                    .Distinct();
                foreach (var v in colvalues)
                {
                    var v1 = v;
                    var rows = this.ReportRowCollection
                                .Where(r => r[gf1.Column] != null)
                                .Where(r => r[gf1.Column].Value as string == v1 as string).ToList();
                    var group = new DataGridGroup
                    {
                        Column = gf.Column,
                        Text = gf.EvaluateExpression(v1, rows)
                    };
                    group.ReportRowCollection.AddRange(rows);
                    list.Add(group);
                }
            }


            return list;
        }
    }
}
