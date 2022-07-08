using GameOfLife;
using System.Diagnostics;

namespace Solution
{
    /// <summary>
    /// Solutions that were scrapped:
    /// - Using a multidimensional matrix (no need if we're working based off of points)
    /// - One hash set for alive cells, one hash set for neighboring dead cells (inefficient in terms of space complexity)
    /// - Cannot use visual ui, most components only allow 32 bit ints, not 64 bit (int vs long)
    /// Going forward:
    /// - Would be nice to set board bounds, currently we consider the board infinitely bound
    /// - Abstract classes a bit more, use interfaces
    /// </summary>
    /// Notes:
    /// - Tested against https://playgameoflife.com/
    public class ConwaysGameOfLife
    {
        private static int _ITERATIONS = 10;
        private static bool _PLAYAGAIN = true;

        public static void Main()
        {
            while(_PLAYAGAIN)
            {
                #region Variables

                Dictionary<(long, long), HashSet<(long, long)>> coords = new();
                HashSet<(long, long)> points = new();
                Stopwatch watch = Stopwatch.StartNew();
                string? input = "riotgames";

                #endregion

                #region Gather Input

                Console.WriteLine("--------- WELCOME TO CONWAY'S GAME OF LIFE ---------");
                Console.WriteLine();
                Console.WriteLine("--------- ENTER THE NUMBER OF ITERATIONS (DEFAULT IS 10) ---------");
                Console.WriteLine();

                input = Console.ReadLine();
                _ITERATIONS = input == string.Empty ? _ITERATIONS : int.Parse(input!);

                Console.WriteLine();
                Console.WriteLine("--------- ENTER YOUR ALIVE COORDINATES (FORMAT: x y) ---------");
                Console.WriteLine();
                input = Console.ReadLine();

                while (input != string.Empty)
                {
                    long[] parsed = input!.Split(' ').Select(n => (long)Convert.ToDouble(n)).ToArray();

                    if (parsed.Length == 2)
                    {
                        points.Add((parsed[0], parsed[1]));
                    }

                    input = Console.ReadLine();
                }

                #endregion

                #region Run Iterations

                watch.Restart();

                GameState initialGameState = new(coords);
                GameOfLifeBoard gameInstance = new(initialGameState);

                // Populate initial dead neighbors
                foreach (var cell in points)
                {
                    coords.TryAdd(cell, GameOfLifeBoard.ComputeNextDead(cell, new HashSet<(long, long)>()));
                }

                // Run iterations
                gameInstance.RunIterations(_ITERATIONS);

                watch.Stop();
                Console.WriteLine($"Elapsed milliseconds: {watch.ElapsedMilliseconds}");
                Console.WriteLine($"Elapsed timespan: {watch.Elapsed}");
                Console.WriteLine($"Elapsed ticks: {watch.ElapsedTicks}");
                Console.WriteLine();

                #endregion

                #region Play Again?

                Console.WriteLine("--------- DO YOU WANT TO PLAY AGAIN? (YES/NO) ---------");
                Console.WriteLine();
                input = Console.ReadLine();

                if(!string.Equals(input, "yes", StringComparison.OrdinalIgnoreCase))
                {
                    _PLAYAGAIN = false;
                }

                Console.WriteLine();

                #endregion
            }
        }
    }
}
