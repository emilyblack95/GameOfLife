﻿namespace GameOfLife
{
    /// <summary>
    ///  Class for Conway's Game of Life
    /// </summary>
    public class GameOfLife
    {
        // List of states of board after each iteration
        public List<GameState> States { get; set; }

        /// <summary>
        /// The 8 Cardinal Points: N S E W NE SE SW NW
        /// </summary>
        private static readonly List<(long, long)> CardinalPoints = new() { (-1, -1), (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1) };

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameOfLife()
        {
            States = new();
        }

        /// <summary>
        /// Overloaded constructor.
        /// </summary>
        public GameOfLife(GameState input)
        {
            States = new() { input };
        }


        /// <summary>
        /// Runs N number of iterations given an initial game state.
        /// </summary>
        public void RunIterations(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                GameState nextState = new() { Coordinates = ComputeNextState() };

                // Fetch recent state, compute next alive/dead, store next state
                States.Add(nextState);

                #region Print State

                Console.WriteLine($"--------- RESULTS FROM ITERATION {i + 1} ---------");
                Console.WriteLine();

                // Life 1.06 format
                foreach (var cell in nextState.Coordinates.Keys)
                {
                    Console.WriteLine(cell.Item1 + " " + cell.Item2);
                }

                Console.WriteLine();

                #endregion                
            }
        }

        /// <summary>
        /// Computes next set of coordinates based off of last stored state.
        /// Rule 1: If an "alive" cell had less than 2 or more than 3 alive neighbors (in any of the 8 surrounding cells), it becomes dead.
        /// Rule 2: If a "dead" cell had *exactly* 3 alive neighbors, it becomes alive.
        /// </summary>
        /// <returns>Next set of coordinates.</returns>
        public Dictionary<(long, long), HashSet<(long, long)>> ComputeNextState()
        {
            Dictionary<(long, long), HashSet<(long, long)>> nextAlive = new();
            (long, long) computedCoord;
            int counter = 0;

            #region Rule 1

            foreach (var cell in States.Last().Coordinates.Keys)
            {
                foreach (var cardinalPoint in CardinalPoints)
                {
                    computedCoord = (cell.Item1 + cardinalPoint.Item1, cell.Item2 + cardinalPoint.Item2);

                    if (States.Last().Coordinates.ContainsKey(computedCoord))
                    {
                        counter++;
                    }
                }

                // If rule 1 is passed, add to next alive list
                if (counter == 2 || counter == 3)
                {
                    nextAlive.TryAdd(cell, new HashSet<(long, long)>());
                }

                counter = 0;
            }

            #endregion

            #region Rule 2

            foreach (var deadNeighbors in States.Last().Coordinates.Values)
            {
                foreach (var cell in deadNeighbors)
                {
                    // If cell exists 3 times in all of the lists of dead neighbors,
                    // that means it's around 3 active cells. Add to alive list.
                    if (CountDeadOccurences(cell) == 3)
                    {
                        nextAlive.TryAdd(cell, new HashSet<(long, long)>());
                    }
                }
            }

            #endregion

            #region Generate New Dead Neighbors

            // Populate each new alive cell with it's next dead neighbors
            foreach (var cell in nextAlive.Keys)
            {
                nextAlive[cell] = ComputeNextDead(cell, nextAlive.Keys.ToHashSet());
            }

            #endregion

            return nextAlive;
        }

        /// <summary>
        /// Computes next list of dead neighbors given the next list of alive cells.
        /// </summary>
        /// <returns>Next set of dead neighbors.</returns>
        public static HashSet<(long, long)> ComputeNextDead((long, long) cell, HashSet<(long, long)> listOfAlive)
        {
            HashSet<(long, long)> result = new();

            foreach (var cardinalPoint in CardinalPoints)
            {
                (long, long) computedCoord = (cell.Item1 + cardinalPoint.Item1, cell.Item2 + cardinalPoint.Item2);

                if (!listOfAlive.Contains((cell.Item1 + cardinalPoint.Item1, cell.Item2 + cardinalPoint.Item2)))
                {
                    result.Add(computedCoord);
                }
            }

            return result;
        }

        /// <summary>
        /// Counts number of occurences of cell between all dead neighbors based off of last stored state.
        /// </summary>
        /// <returns>Count of cell in dead neighbors lists.</returns>
        private int CountDeadOccurences((long, long) cell)
        {
            int count = 0;

            foreach (var list in States.Last().Coordinates.Values)
            {
                foreach (var item in list)
                {
                    if (item.Equals(cell))
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
