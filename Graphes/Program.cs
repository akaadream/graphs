using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphes
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph graph = new Graph();

            int degeneracy = graph.Degeneracy();
            Console.WriteLine("Dégénérescence égale à : " + degeneracy);

            Console.WriteLine("Appuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
