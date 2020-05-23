using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphes
{
    public class Graph
    {
        // Tableau de sommets
        public List<Node> Nodes { get; set; }
        // Matrice d'adjacence
        public int[,] AdjacentMatrix { get; set; }
        // Nombre de sommets dans le graphe
        public int Size { get => Nodes.Count; }

        public DegreesList[] NodesList { get; set; }
        public List<Node> MarkedNodes { get; set; }

        // Constructeur par défaut
        public Graph()
        {
            Nodes = new List<Node>();
            MarkedNodes = new List<Node>();
            // Instanciation du tableau de sommets
            //Nodes = new Node[n];

            // Instanciation de la matrice d'adjacence
            //AdjacentMatrix = new int[n,n];

            // Instanciation des sommets
            //InitializeGraph();
            LoadExistingGraph(@"ego-facebook.edges", ',');
            //ExampleGraph();

            // Construction de la matrice
            //BuildAdjacentMatrix(44);
        }

        // Initialiation par défaut du graphe
        public void InitializeGraph(int length)
        {
            for (int i = 0; i < length; i++)
            {
                Nodes.Add(new Node(this));
            }
        }

        public void ExampleGraph()
        {
            InitializeGraph(10);
            // A:0, B:1, C:2, D:3, E:4, F:5, G:6, H:7, I:8, J:9
            Nodes[0].LinkedNodes.Add(Nodes[1]);
            Nodes[0].LinkedNodes.Add(Nodes[2]);
            Nodes[0].LinkedNodes.Add(Nodes[3]);
            Nodes[0].LinkedNodes.Add(Nodes[4]);
            Nodes[0].LinkedNodes.Add(Nodes[5]);
            Nodes[1].LinkedNodes.Add(Nodes[0]);
            Nodes[1].LinkedNodes.Add(Nodes[6]);
            Nodes[2].LinkedNodes.Add(Nodes[0]);
            Nodes[2].LinkedNodes.Add(Nodes[3]);
            Nodes[2].LinkedNodes.Add(Nodes[4]);
            Nodes[3].LinkedNodes.Add(Nodes[0]);
            Nodes[3].LinkedNodes.Add(Nodes[2]);
            Nodes[3].LinkedNodes.Add(Nodes[5]);
            Nodes[4].LinkedNodes.Add(Nodes[0]);
            Nodes[4].LinkedNodes.Add(Nodes[2]);
            Nodes[4].LinkedNodes.Add(Nodes[5]);
            Nodes[4].LinkedNodes.Add(Nodes[6]);
            Nodes[5].LinkedNodes.Add(Nodes[0]);
            Nodes[5].LinkedNodes.Add(Nodes[3]);
            Nodes[5].LinkedNodes.Add(Nodes[4]);
            Nodes[5].LinkedNodes.Add(Nodes[6]);
            Nodes[5].LinkedNodes.Add(Nodes[7]);
            Nodes[5].LinkedNodes.Add(Nodes[8]);
            Nodes[5].LinkedNodes.Add(Nodes[9]);
            Nodes[6].LinkedNodes.Add(Nodes[1]);
            Nodes[6].LinkedNodes.Add(Nodes[4]);
            Nodes[6].LinkedNodes.Add(Nodes[5]);
            Nodes[6].LinkedNodes.Add(Nodes[6]);
            Nodes[7].LinkedNodes.Add(Nodes[5]);
            Nodes[7].LinkedNodes.Add(Nodes[6]);
            Nodes[7].LinkedNodes.Add(Nodes[8]);
            Nodes[8].LinkedNodes.Add(Nodes[5]);
            Nodes[8].LinkedNodes.Add(Nodes[7]);
            Nodes[9].LinkedNodes.Add(Nodes[5]);

            int maxDegree = 0;
            foreach (Node node in Nodes)
            {
                node.Degree = node.LinkedNodes.Count;
                if (node.Degree > maxDegree) maxDegree = node.Degree;
            }

            BuildDegreesLists(maxDegree);
        }

        // Charger un graph existant
        public void LoadExistingGraph(string filename, char separator)
        {
            try
            {
                // Récupération des lignes du fichier du graphe
                string[] lines = File.ReadAllLines(filename);
                // Initialisation du graphe
                InitializeGraph(lines.Length);

                AdjacentMatrix = new int[lines.Length, lines.Length];

                // On parcourt toutes les lignes
                foreach (string line in lines)
                {
                    string[] values = line.Split(separator);
                    if (values.Length != 2) continue;
                    // Parse values to int
                    int value1 = int.Parse(values[0]);
                    int value2 = int.Parse(values[1]);

                    Nodes[value1].LinkedNodes.Add(Nodes[value2]);
                    Nodes[value2].LinkedNodes.Add(Nodes[value1]);
                }
            }
            catch (IOException exception) { }

            int maxDegree = 0;
            foreach (Node node in Nodes)
            {
                node.Degree = node.LinkedNodes.Count;
                if (node.Degree > maxDegree) maxDegree = node.Degree;
            }

            BuildDegreesLists(maxDegree);
        }

        // Construire la matrice d'adjacence
        public void BuildAdjacentMatrix(int p)
        {
            Random random = new Random();
            for (int i = 0; i < Size; i++)
            {
                for (int j = i + 1; j < Size; j++)
                {
                    // Si i == j, on peut pas relier un sommet à lui-même, donc on continue
                    if (i == j) continue;

                    // Générer un nouveau nombre aléatoire
                    int rand = random.Next(0, 100);
                    if (rand <= p)
                    {
                        // On créer un nouveau lien
                        Nodes[i].LinkedNodes.Add(Nodes[j]);
                        Nodes[j].LinkedNodes.Add(Nodes[i]);
                    }
                }
            }
        }

        public void BuildDegreesLists(int maxDegree)
        {
            Console.WriteLine("Degré maximal : " + maxDegree);
            NodesList = new DegreesList[maxDegree + 1];

            for (int i = 0; i <= maxDegree; i++)
            {
                NodesList[i] = new DegreesList(this);
                if (i != 0)
                {
                    foreach (Node node in Nodes)
                    {
                        if (node.Degree == i && !MarkedNodes.Contains(node)) NodesList[i].Nodes.Add(node);
                    }
                }
            }
        }

        // Calculer le dégérérescence 
        public int Degeneracy()
        {
            // On initialise k à 0
            int k = 0;

            // On répète n fois
            for (int i = 0; i < Size; i++)
            {
                Console.WriteLine("Current i : " + i);
                // On récupère le premier indice de liste non vide
                int indice = GetFirstNotEmptyNodeList();
                Console.WriteLine("Selected indice : " + indice);
                Console.WriteLine("K : " + k);
                // On met k égal au max entre k et l'indice
                k = Math.Max(k, indice);
                if (NodesList[indice].Nodes.Count == 0) break;
                // On récupère un sommet de la liste de l'indice
                Node vertex = NodesList[indice].Nodes.First();
                // On supprime le sommet de la liste
                NodesList[indice].Nodes.Remove(vertex);
                // On ajoute le sommet dans la liste de sortie
                MarkedNodes.Add(vertex);
                // On parcourt les voisins du sommet
                for (int j = vertex.LinkedNodes.Count - 1; j >= 0; j--)
                {
                    // Instance du noeud de la boucle
                    Node node = vertex.LinkedNodes[j];

                    // Si le sommet n'est pas dans la liste de sortie
                    if (!MarkedNodes.Contains(node))
                    {
                        // On soustrait 1 à son degré
                        node.Degree--;
                        if (node.Degree < 0) node.Degree = 0;
                        // On ajoute le noeud dans la liste correspondante
                        if (!NodesList[node.Degree].Nodes.Contains(node))
                        {
                            // On met à jour le centre du noeud
                            node.Center = node.Degree;

                            // Ajout du noeud
                            NodesList[node.Degree].Nodes.Add(node);
                            NodesList[node.Degree + 1].Nodes.Remove(node);
                        }

                        // On le supprime des voisins
                        vertex.LinkedNodes.Remove(node);
                    }
                }
            }
            
            // La dégénérescence est k
            return k;
        }

        // Récupérer le premier indice de list de sommet non vide
        public int GetFirstNotEmptyNodeList()
        {
            for (int i = 0; i < NodesList.Length; i++)
            {
                Console.WriteLine("Nombre d'éléments dans la liste de degrés " + i + " : " + NodesList[i].Nodes.Count);
                if (NodesList[i].Nodes.Count > 0) return i;
            }

            return 0;
        }

        // Vérifier si tous les sommets du graphes sont marqués
        public bool AllNodeMarked()
        {
            bool allMarked = true;
            foreach (Node node in Nodes)
            {
                if (!node.Marked) allMarked = false;
            }

            return allMarked;
        }

        // Afficher la matrice d'adjacence sous forme de texte
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    s += "" + AdjacentMatrix[i, j] + "   ";
                }
                s += "\n";
            }
            return s;
        }
    }
}
