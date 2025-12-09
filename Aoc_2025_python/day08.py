import math


class P3:
    def __init__(self, raw_data: str):
        parts = raw_data.split(",")
        self.x = int(parts[0])
        self.y = int(parts[1])
        self.z = int(parts[2])

    def __repr__(self):
        return f"P({self.x}:{self.y}:{self.z})"

    def dist_to(self, other: "P3") -> float:
        dx = self.x - other.x
        dy = self.y - other.y
        dz = self.z - other.z
        return math.sqrt(dx**2 + dy**2 + dz**2)


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line for line in file]
    cnt = 10 if filename == "_input0.txt" else 1000 # type: ignore
    print("p1", solve1(lines, cnt))
    print("p2", solve2(lines))


def solve1(lines: list[str], nb_joins: int) -> int:
    pts = [P3(x) for x in lines]
    dist = get_sorted_dist_arr(pts)

    dsu: list[int] = [x for x in range(0, len(pts))]
    for i in range(0, nb_joins):
        _, i, j = dist[i]
        par_i = get_dsu_parent(dsu, i)
        par_j = get_dsu_parent(dsu, j)
        dsu[par_j] = par_i

    dsu = [get_dsu_parent(dsu, x) for x in dsu]
    counts = get_counts(dsu)
    top3 = sorted(counts.values(), reverse=True)[:3]
    print("top3 networks", top3)
    return math.prod(top3)


def solve2(lines: list[str]) -> int:
    pts = [P3(x) for x in lines]
    dist = get_sorted_dist_arr(pts)

    dsu: list[int] = [x for x in range(0, len(pts))]
    for i in range(0, len(dist)):
        _, i, j = dist[i]
        par_i = get_dsu_parent(dsu, i)
        par_j = get_dsu_parent(dsu, j)
        dsu[par_j] = par_i
        dsu = [get_dsu_parent(dsu, x) for x in dsu]
        if len(get_counts(dsu)) == 1:
            p1 = pts[i]
            p2 = pts[j]
            return p1.x * p2.x
    return -1


def get_counts(dsu: list[int]) -> dict[int, int]:
    counts: dict[int, int] = {}
    for x in dsu:
        counts[x] = counts.get(x, 0) + 1
    return counts


def get_dsu_parent(dsu: list[int], x: int) -> int:
    par = dsu[x]
    while par != x:
        x = par
        par = dsu[x]
    return par


def get_sorted_dist_arr(pts: list[P3]) -> list[tuple[float, int, int]]:
    dist: list[tuple[float, int, int]] = []
    for i in range(0, len(pts) - 1):
        for j in range(i + 1, len(pts)):
            dist.append((pts[i].dist_to(pts[j]), i, j))
    dist.sort(key=lambda t: t[0])
    return dist


if __name__ == "__main__":
    main()

"""
What I learned?
 - In Python classes and structs are both just classes, dataclasses auto-generate init and repr
 - Regular classes declare fields inside __init__ via self.x
 - Exponentiation uses ** and sqrt is math.sqrt or x**0.5
 - Lists must have explicit generic types in strict mode like list[int]
 - __repr__ defines print-friendly output, __str__ overrides human-readable form
 - Sorting by float uses list.sort(key=lambda t: t[0]) or sorted(...)
 - Counting items compact form is counts[v] = counts.get(v, 0) + 1 or use Counter
 - Multiplying list elements uses math.prod(...)
 - String interpolation uses f-strings like f"{value}"
"""
