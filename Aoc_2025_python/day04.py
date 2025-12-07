def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [list(line.rstrip()) for line in file]
    print("p1", solve(lines, False))
    print("p2", solve(lines, True))


def solve(lines: list, shouldClean: bool) -> int:
    res = 0
    points = []
    h = len(lines)
    w = len(lines[0])
    while True:
        for y in range(h):
            for x in range(w):
                if check(lines, x, y, h, w) > 0:
                    points.append((y, x))
        res += len(points)
        if not shouldClean:
            break
        if len(points) > 0:
            clean(lines, points)
            points = []
        else:
            break

    return res


def clean(lines: list, points: list):
    for p in points:
        y = p[0]
        x = p[1]
        lines[y][x] = '.'


dx = [-1, 0, 1, 1, 1, 0, -1, -1]
dy = [-1, -1, -1, 0, 1, 1, 1, 0]


def check(lines: list, x0, y0, h, w) -> int:
    if lines[y0][x0] == ".":
        return 0
    filled = 0
    for i in range(len(dx)):
        x = x0 + dx[i]
        y = y0 + dy[i]
        if x < 0 or x >= w or y < 0 or y >= h:
            continue
        if lines[y][x] == "@":
            filled += 1
    return 1 if filled < 4 else 0


if __name__ == "__main__":
    main()

"""
What I learned?
- Simple list comprehension to make array from string [int(c) for c in line]
"""
