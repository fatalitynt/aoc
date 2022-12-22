import time


def get_actions(line):
    line = line.strip()
    turns = []
    for c in line:
        if c == 'R' or c == 'L':
            turns.append(c)
    parts = line.replace('R', 'x').replace('L', 'x').split('x')
    res = []
    for i in range(len(turns)):
        res.append(int(parts[i]))
        res.append(turns[i])
    res.append(int(parts[-1]))
    return res


def turn(d, t):
    return ((d + 1) % 4) if t == 'R' else ((d + 3) % 4)


def teleport(lines, y, x, d):
    dx0 = dx[d] * -1
    dy0 = dy[d] * -1
    while lines[y + dy0][x + dx0] != ' ':
        y += dy0
        x += dx0
    return x, y, d


# > V < ^
dx = [1, 0, -1, 0]
dy = [0, 1, 0, -1]
dc = ['>', 'V', '<', '^']
ed = {'u': 1, 'd': 3, 'l': 0, 'r': 2}
size = 50

tps = {}


def get_range(tile_x, tile_y, s, d, step):
    x = tile_x * size + 1
    y = tile_y * size + 1
    if s == 'u':
        return (x, y - step, 1, 0) if d == 1 else (x + size - 1, y - step, -1, 0)
    if s == 'd':
        return (x, y + size - 1 + step, 1, 0) if d == 1 else (x + size - 1, y + size - 1 + step, -1, 0)
    if s == 'r':
        return (x + size - 1 + step, y, 0, 1) if d == 1 else (x + size - 1 + step, y + size - 1, 0, -1)
    if s == 'l':
        return (x - step, y, 0, 1) if d == 1 else (x - step, y + size - 1, 0, -1)


def build_tp(x_a, y_a, s_a, x_b, y_b, s_b, reverse):
    global tps
    x0_a, y0_a, dx_a, dy_a = get_range(x_a, y_a, s_a, 1, 1)
    x0_b, y0_b, dx_b, dy_b = get_range(x_b, y_b, s_b, reverse, 0)
    end_d = ed[s_b]
    for i in range(size):
        tps[(x0_a + dx_a * i, y0_a + dy_a * i)] = (x0_b + dx_b * i, y0_b + dy_b * i, end_d)


def build_both_tp(x_a, y_a, s_a, x_b, y_b, s_b, reverse):
    build_tp(x_a, y_a, s_a, x_b, y_b, s_b, reverse)
    build_tp(x_b, y_b, s_b, x_a, y_a, s_a, reverse)


def solve(lines):
    global dx, dy
    y = 1
    x = lines[y].index('.')
    d = 0
    actions = get_actions(lines[-1])
    for a in actions:
        if a == 'R' or a == 'L':
            d = turn(d, a)
            continue
        nb = a
        while nb > 0:
            nb -= 1
            c = lines[y + dy[d]][x + dx[d]]
            if c == '.':
                y += dy[d]
                x += dx[d]
            elif c == '#':
                break
            elif c == ' ':
                # part 1
                # o_x, o_y, o_d = teleport(lines, y, x, d)
                # part 2
                o_x, o_y, o_d = tps[(x + dx[d], y + dy[d])]
                if lines[o_y][o_x] == '.':
                    y = o_y
                    x = o_x
                    d = o_d
                else:
                    break
    return 1000 * y + 4 * x + d


def main():
    filename = "_input.txt"
    lines = []
    max_line_size = 0
    with open(filename) as file:
        for line in file:
            max_line_size = max(max_line_size, len(line) + 1)
            lines.append(" " + line.rstrip() + " ")
    lines.insert(0, " " * max_line_size)
    lines = list(map(lambda x: x + " " * max_line_size, lines))

    # test connections
    # build_both_tp(2, 1, 'r', 3, 2, 'u', -1)
    # build_both_tp(2, 0, 'r', 3, 2, 'r', -1)
    # build_both_tp(2, 0, 'l', 1, 1, 'u', 1)
    # build_both_tp(1, 1, 'd', 2, 2, 'l', -1)
    # build_both_tp(0, 1, 'd', 2, 2, 'd', -1)
    # build_both_tp(2, 0, 'r', 3, 2, 'r', 1)
    # build_both_tp(3, 2, 'd', 0, 1, 'l', -1)
    # build_both_tp(2, 0, 'u', 0, 1, 'u', -1)

    # real connections
    build_both_tp(2, 0, 'd', 1, 1, 'r', 1)
    build_both_tp(1, 1, 'l', 0, 2, 'u', 1)
    build_both_tp(1, 0, 'l', 0, 2, 'l', -1)
    build_both_tp(2, 0, 'r', 1, 2, 'r', -1)
    build_both_tp(1, 2, 'd', 0, 3, 'r', 1)
    build_both_tp(2, 0, 'u', 0, 3, 'd', 1)
    build_both_tp(1, 0, 'u', 0, 3, 'l', 1)

    start_time = time.time()
    print(solve(lines))
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
