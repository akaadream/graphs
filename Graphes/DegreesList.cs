using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphes
{
    public class DegreesList
    {
        public List<Node> Nodes { get; set; }
        public Graph Parent { get; set; }

        public DegreesList(Graph parent)
        {
            Nodes = new List<Node>();
            Parent = parent;
        }
    }
}
