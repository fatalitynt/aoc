import time

moves = [
    [[-1, -1], [0, -1], [1, -1]],  # N = ^
    [[-1, 1], [0, 1], [1, 1]],  # S = V
    [[-1, -1], [-1, 0], [-1, 1]],  # W = <
    [[1, -1], [1, 0], [1, 1]],  # E = >
]

flat_moves = [[-1, -1], [0, -1], [1, -1], [1, 0], [1, 1], [0, 1], [-1, 1], [-1, 0]]


def shift(a, b):
    return a[0] + b[0], a[1] + b[1]


class Elf:
    def __init__(self, x, y):
        self.pos = (x, y)
        self.prop = None

    def __repr__(self):
        return repr(self.pos) + " -> " + repr(self.prop)

    def try_move(self, r_idx, props, m):
        self.prop = None
        all_empty = all(map(lambda d: shift(self.pos, d) not in m, flat_moves))
        if all_empty:
            return
        for i in range(4):
            directions = moves[(i + r_idx) % 4]
            any_occupied = any(map(lambda d: shift(self.pos, d) in m, directions))
            if any_occupied:
                continue
            self.prop = shift(self.pos, directions[1])
            if self.prop not in props:
                props[self.prop] = 0
            props[self.prop] += 1
            break

    def move_if_can(self, props, m):
        if self.prop is None or props[self.prop] > 1:
            return
        del m[self.pos]
        self.pos = self.prop
        m[self.pos] = self


def get_points(elves):
    points = [elves[0].pos[0], elves[0].pos[0], elves[0].pos[1], elves[0].pos[1]]  # min_x, max_x, min_y, max_y
    for e in elves:
        points[0] = min(points[0], e.pos[0])
        points[1] = max(points[1], e.pos[0])
        points[2] = min(points[2], e.pos[1])
        points[3] = max(points[3], e.pos[1])
    return points


def print_map(m):
    elves = list(m.values())
    points = get_points(elves)
    for y in range(points[2], points[3] + 1):
        line = ""
        for x in range(points[0], points[1] + 1):
            line += "#" if (x, y) in m else "."
        print(line)
    print("-----------------")


def solve(elves, max_rounds=-1):
    m = {}

    for e in elves:
        m[e.pos] = e

    r_idx = 0
    while max_rounds < 0 or r_idx < max_rounds:
        props = {}
        for e in elves:
            e.try_move(r_idx, props, m)
        for e in elves:
            e.move_if_can(props, m)
        r_idx += 1
        if len(props) == 0:
            break

    print("steps done =", r_idx)
    points = get_points(elves)

    res = 0
    for y in range(points[2], points[3] + 1):
        for x in range(points[0], points[1] + 1):
            if (x, y) not in m:
                res += 1
    print("empty cells =", res)


def parse_elfs(lines):
    res = []
    for y in range(len(lines)):
        line = lines[y]
        for x in range(len(line)):
            if line[x] == "#":
                res.append(Elf(x, y))
    return res


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]

    elves = parse_elfs(lines)

    start_time = time.time()
    # part 1
    solve(elves, 10)
    # part 2
    solve(elves)
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
