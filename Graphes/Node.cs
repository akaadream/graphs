using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphes
{
    public class Node
    {
        // Sommets reliés à ce sommet
        public List<Node> LinkedNodes { get; set; }

        // Graphe parent
        public Graph Parent { get; set; }

        // Degré du sommet
        public int Degree { get; set; }

        // Centre du sommet
        public int Center { get; set; }

        // Si le sommet est marqué dans le graph
        public bool Marked { get; set; }

        // Constructeur par défaut
        public Node(Graph parent)
        {
            Marked = false;
            Center = 0;
            Parent = parent;
            LinkedNodes = new List<Node>();
        }

        // Constructeur avec une liste définie de sommets liés
        public Node(Graph parent, List<Node> nodes)
        {
            Parent = parent;
            LinkedNodes = nodes;
        }

        // Le degré du noeud lors de la dégénérescence
        public int DegeneracyDegree()
        {
            int count = 0;
            foreach (Node node in LinkedNodes)
            {
                if (!node.Marked) count++;
            }

            return count;
        }
    }
}
