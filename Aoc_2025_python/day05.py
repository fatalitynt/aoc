def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    print("p1", solve1(lines))
    print("p2", solve2(lines))


def solve1(lines: list) -> int:
    res = 0
    ranges = getRanges(lines)
    i = len(ranges) + 1
    while i < len(lines):
        v = int(lines[i])
        i += 1
        for r in ranges:
            if r[0] <= v and v <= r[1]:
                res += 1
                break

    return res


def solve2(lines: list) -> int:
    ranges = mergeRanges(getRanges(lines))
    return sum(r[1] - r[0] + 1 for r in ranges)


def getRanges(lines: list) -> list:
    ranges = []
    for line in lines:
        if line == "":
            break
        parts = line.split("-")
        ranges.append((int(parts[0]), int(parts[1])))
    ranges.sort()
    return ranges


def mergeRanges(ranges: list) -> list:
    res = []
    cur = ranges[0]
    for r in ranges[1:]:
        if isIntersect(r, cur):
            cur = mergeTwoRanges(cur, r)
            continue
        res.append(cur)
        cur = r

    res.append(cur)
    return res


def isIntersect(r1, r2) -> bool:
    return r1[0] <= r2[1] and r2[0] <= r1[1]


def mergeTwoRanges(r1, r2):
    return (min(r1[0], r2[0]), max(r1[1], r2[1]))


if __name__ == "__main__":
    main()

"""
What I learned?
- Declaring loop variable outside: i = 0; for i in range(i, x): ...
- Skipping first element: for r in ranges[1:]: ...
- Tuple sorting is automatic: ranges.sort()
- C# style sum: total = sum(r[1] - r[0] + 1 for r in ranges)
"""
