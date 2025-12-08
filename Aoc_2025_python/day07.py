from collections import defaultdict

def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line for line in file]
    print("p1", solve1(lines))
    print("p2", solve2(lines))


def solve1(a: list) -> int:
    res = 0
    h = len(a)
    w = len(a[0])
    pts = {a[0].find("S")}
    for y in range(1, h):
        pts1 = set()
        for x in pts:
            if a[y][x] == ".":
                pts1.add(x)
            if a[y][x] == "^":
                res += 1
                if x > 0:
                    pts1.add(x - 1)
                if x < w:
                    pts1.add(x + 1)
        pts = pts1
    return res


def solve2(a: list) -> int:
    h = len(a)
    w = len(a[0])
    pts = defaultdict(int)
    pts[a[0].find("S")] = 1
    for y in range(1, h):
        pts1 = defaultdict(int)
        for x, cnt in pts.items():
            if a[y][x] == ".":
                pts1[x] += cnt
            if a[y][x] == "^":
                if x > 0:
                    pts1[x - 1] += cnt
                if x < w:
                    pts1[x + 1] += cnt
        pts = pts1
    return sum(pts.values())


def inc(d: dict, k: int):
    if k not in d:
        d[k] = 0
    d[k] = d[k] + 1


if __name__ == "__main__":
    main()

"""
What I learned?
 - Python set is the equivalent of HashSet<int>, literal is {1,2,3}, empty set is set()
 - Use d.get(k, 0) + 1 or defaultdict(int) to implement optimized increment
 - Iterate dict key/value pairs with for k, v in d.items()
 - Get all dict values via d.values() or list(d.values())
"""
