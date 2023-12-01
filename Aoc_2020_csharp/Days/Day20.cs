using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2020.Days
{
    public static class Day20
    {
        public static void Main1()
        {
            var input = File.ReadAllLines("Inputs/20.txt");
            var tiles = GetTiles(input).ToArray();
            var tilesDict = tiles.ToDictionary(x => x.Id);
            var arranged = new HashSet<long>();

            foreach (var me in tiles) FillMatched(me, tiles);

            var pallet = Enumerable.Range(0, 100).Select(x => new Tile[100]).ToArray();

            var firstTile = tiles.First(x => x.MatchedIds.Count == 2);
            pallet[0][0] = firstTile;
            arranged.Add(firstTile.Id);
            
            var other = firstTile.MatchedIds.Select(x => tilesDict[x]).ToArray();

            pallet[0][1] = other[0];
            arranged.Add(other[0].Id);
            
            pallet[1][0] = other[1];
            arranged.Add(other[1].Id);
            
            var common = GetCommonNeighbor(other[0], other[1], arranged);
            
            var commonTile = tilesDict[common];
            pallet[1][1] = commonTile;
            arranged.Add(commonTile.Id);
            
            AdjustLR(pallet[0][0], pallet[0][1], true);
            var flipped = AdjustBT(pallet[0][0], pallet[1][0], true);
            if (flipped) pallet[0][1].FlipVertically();
            AdjustLR(pallet[1][0], pallet[1][1], false);
            
            var bottomRowToSet = 2;
            var rightColumnToSet = 2;
            while (arranged.Count < tiles.Length)
            {
                if (TryFillBottomRow(pallet, bottomRowToSet, arranged, tilesDict)) bottomRowToSet++;
                if (TryFillRightCol(pallet, rightColumnToSet, arranged, tilesDict)) rightColumnToSet++;
            }
            
            foreach (var tile in tiles) tile.CutEdges();

            var tile0 = CombinePallet(pallet);
            tile0.Print();

            var monster = new[]
            {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   ",
            };

            var rotateCount = 0;
            var found = false;
            var imageFlipped = false;
            
            var h = tile0.Image.Length;
            var w = tile0.Image[0].Length;

            while (rotateCount < 4)
            {
                var image = tile0.Image.Select(x => x.ToCharArray()).ToArray();
                for (var i = 0; i < h; i++)
                for (var j = 0; j < w; j++)
                {
                    found = FindAndReplaceMonster(monster, image, i, j) || found;
                }

                if (found)
                {
                    var total = image.Sum(line => line.Count(x => x == '#'));
                    Console.WriteLine(total);
                    return;
                }

                if (!imageFlipped)
                {
                    tile0.FlipHorizontally();
                    imageFlipped = true;
                    continue;
                }

                tile0.RotateRight();
                rotateCount++;
            }

            Console.WriteLine("Didn't found");
        }

        private static bool ScanImage(string[] monster, char[][] image, int dh, int dw,
            Func<char[][], int, int, bool> checkOrUpdate)
        {
            for (var h = 0; h < monster.Length; h++)
            for (var w = 0; w < monster[h].Length; w++)
                if (monster[h][w] == '#' && checkOrUpdate(image, h + dh, w + dw)) return false;
            return true;
        }

        private static bool FindAndReplaceMonster(string[] monster, char[][] image, int dh, int dw)
        {
            return dh + monster.Length <= image.Length && 
                   dw + monster[0].Length <= image[0].Length && 
                   ScanImage(monster, image, dh, dw, (img, h, w) => image[h][w] != '#') && 
                   ScanImage(monster, image, dh, dw, (img, h, w) => (image[h][w] = '0') != '0');
        }

        private static Tile CombinePallet(Tile[][] pallet)
        {
            var list = new List<string>();
            foreach (var line in pallet)
            {
                if(line.All(x => x == null)) break;
                var tiles = line.TakeWhile(x => x != null).ToArray();
                var h = tiles[0].Image.Length;
                for (var i = 0; i < h; i++)
                {
                    var l = string.Join("", tiles.Select(t => t.Image[i]));
                    list.Add(l);
                }
            }
            return new Tile(-1, list.ToArray());
        }

        private static bool AdjustBT(Tile topTile, Tile bottomTime, bool canFlipTopTile)
        {
            var flipped = false;
            var rotateRightCount = 0;
            while (true)
            {
                var bot = topTile.BotEdge();
                var top = bottomTime.TopEdge();
                
                if (bot == top) break;
                if (bot == new string(top.Reverse().ToArray()))
                {
                    bottomTime.FlipHorizontally();
                    break;
                }
                
                bottomTime.RotateRight();
                rotateRightCount++;
                if (rotateRightCount == 4)
                {
                    rotateRightCount = 0;
                    if (canFlipTopTile)
                    {
                        if (flipped) throw new Exception("Already flipped");
                        flipped = true;
                        topTile.FlipVertically();
                    }
                    else throw new Exception($"cant find match at AdjustBT for {topTile.Id} and {bottomTime.Id}");
                }
            }
            return flipped;
        }

        private static void AdjustLR(Tile left, Tile right, bool canRotateL)
        {
            var rotateRightCount = 0;
            var rotateLeftCount = 0;
            while (true)
            {
                var re = left.RightEdge();
                var le = right.LeftEdge();
                
                if (re == le) break;
                if (re == new string(le.Reverse().ToArray()))
                {
                    right.FlipVertically();
                    break;
                }
                
                right.RotateRight();
                rotateRightCount++;

                if (rotateRightCount == 4)
                {
                    rotateRightCount = 0;
                    if (canRotateL)
                    {
                        left.RotateRight();
                        rotateLeftCount++;
                        if (rotateLeftCount == 4) throw new Exception("rotate left count = 4");
                    }
                    else throw new Exception($"cant find match at AdjustLR for {left.Id} and {right.Id}");
                }
            }
        }

        private static bool TryFillBottomRow(Tile[][] pallet, int rowIdx, HashSet<long> arranged,
            Dictionary<long, Tile> tilesDict)
        {
            var above = pallet[rowIdx - 1][0];
            var nextSetId = above.MatchedIds.SingleOrDefault(x => !arranged.Contains(x));
            if (nextSetId == 0) return false;
            var nextSetTile = tilesDict[nextSetId];
            pallet[rowIdx][0] = nextSetTile;
            arranged.Add(nextSetTile.Id);
            AdjustBT(above, nextSetTile, false);

            var columnIdx = 1;
            while (pallet[rowIdx - 1][columnIdx] != null)
            {
                above = pallet[rowIdx - 1][columnIdx];
                nextSetId = GetCommonNeighbor(nextSetTile, above, arranged);
                nextSetTile = tilesDict[nextSetId];
                pallet[rowIdx][columnIdx] = nextSetTile;
                arranged.Add(nextSetTile.Id);
                AdjustBT(above, nextSetTile, false);
                columnIdx++;
            }
            return true;
        }
        
        private static bool TryFillRightCol(Tile[][] pallet, int colIdx, HashSet<long> arranged,
            Dictionary<long, Tile> tilesDict)
        {
            var lefter = pallet[0][colIdx - 1];
            var nextSetId = lefter.MatchedIds.SingleOrDefault(x => !arranged.Contains(x));
            if (nextSetId == 0) return false;
            var nextSetTile = tilesDict[nextSetId];
            pallet[0][colIdx] = nextSetTile;
            arranged.Add(nextSetTile.Id);
            AdjustLR(lefter, nextSetTile, false);

            var rowIdx = 1;
            while (pallet[rowIdx][colIdx - 1] != null)
            {
                lefter = pallet[rowIdx][colIdx - 1];
                nextSetId = GetCommonNeighbor(nextSetTile, lefter, arranged);
                nextSetTile = tilesDict[nextSetId];
                pallet[rowIdx][colIdx] = nextSetTile;
                arranged.Add(nextSetTile.Id);
                AdjustLR(lefter, nextSetTile, false);
                rowIdx++;
            }
            return true;
        }

        private static long GetCommonNeighbor(Tile a, Tile b, HashSet<long> arranged)
        {
            return a.MatchedIds.Where(idA => b.MatchedIds.Contains(idA)).Single(id => !arranged.Contains(id));
        }

        private static void FillMatched(Tile me, Tile[] tiles)
        {
            var matchedIds = tiles.Where(x => x.Id != me.Id)
                .Where(other => me.Edges.Any(myE => other.EdgesWithReversedPairs().Any(otherE => myE == otherE)))
                .Select(other => other.Id);

            foreach (var matchedId in matchedIds) me.MatchedIds.Add(matchedId);
        }

        class Tile
        {
            public Tile(long id, string[] image)
            {
                Id = id;
                Image = image;
                Edges = BuildEdges(image);
                MatchedIds = new HashSet<long>();
            }
            
            public long Id { get; }
            public string[] Image { get; private set; }
            public string[] Edges { get; private set; }

            public IEnumerable<string> EdgesWithReversedPairs() =>
                Edges.SelectMany(e => new[] {e, new string(e.Reverse().ToArray())});

            public HashSet<long> MatchedIds { get; }

            public void CutEdges()
            {
                Image = Image
                    .Skip(1)
                    .Take(Image.Length - 2)
                    .Select(line => new string(line
                        .Skip(1)
                        .Take(line.Length - 2)
                        .ToArray())
                    )
                    .ToArray();
            }

            public void FlipVertically()
            {
                Image = Image.Reverse().ToArray();
                Edges = BuildEdges(Image);
            }
            
            public void FlipHorizontally()
            {
                Image = Image.Select(x => new string(x.Reverse().ToArray())).ToArray();
                Edges = BuildEdges(Image);
            }

            public void RotateRight()
            {
                var h = Image.Length;
                var w = Image[0].Length;
                var image = Enumerable.Range(0, w).Select(x => new char[h]).ToArray();
                for (var i = 0; i < h; i++)
                {
                    var line = Image[i];
                    for (var j = 0; j < w; j++)
                    {
                        image[j][w - i - 1] = line[j];
                    }
                }

                Image = image.Select(line => new string(line)).ToArray();
                Edges = BuildEdges(Image);
            }

            private string[] BuildEdges(string[] image)
            {
                return new[]
                {
                    image.First(),
                    new string(image.Select(x => x.First()).ToArray()),
                    new string(image.Select(x => x.Last()).ToArray()),
                    image.Last()
                };
            }

            public string TopEdge() => Edges[0];
            public string LeftEdge() => Edges[1];
            public string RightEdge() => Edges[2];
            public string BotEdge() => Edges[3];

            public void Print()
            {
                foreach (var line in Image) Console.WriteLine(line);
            }
        }

        private static IEnumerable<Tile> GetTiles(string[] input)
        {
            for (var i = 0; i < input.Length;)
            {
                var id = long.Parse(input[i].Substring(5, 4));
                var image = input.Skip(i + 1).Take(10).ToArray();
                yield return new Tile(id, image);
                i += 12;
            }
        }

        private static void PrintPallet(Tile[][] pallet)
        {
            Console.Clear();
            foreach (var line in pallet)
            {
                if(line.All(x => x == null)) break;
                var tiles = line.TakeWhile(x => x != null).ToArray();
                var h = tiles[0].Image.Length;
                for (var i = 0; i < h; i++)
                {
                    var l = string.Join(" ", tiles.Select(t => t.Image[i]));
                    Console.WriteLine(l);
                }
                Console.WriteLine();
            }
        }

        private static void Part1(Tile[] tiles)
        {
            var answer = tiles.OrderBy(x => x.MatchedIds.Count).Take(4).Select(x => x.Id).Aggregate((a, b) => a * b);
            Console.WriteLine(answer);
        }
    }
}