import time
from collections import deque


h = 0
w = 0
full_state = []
states = []
bliz = []
direction_map = {'>': 0, 'v': 1, '<': 2, '^': 3}
dx = [1, 0, -1, 0]
dy = [0, 1, 0, -1]


def add_bliz(x, y, c):
    global bliz
    bliz.append([x, y, dx[direction_map[c]], dy[direction_map[c]]])


def build_full_state(input_lines):
    global full_state
    for y in range(len(input_lines)):
        input_line = input_lines[y]
        res_line = []
        for x in range(len(input_line)):
            c = input_line[x]
            res_line.append(0 if c == '.' else 1)
            if c != "." and c != "#":
                add_bliz(x, y, c)

        full_state.append(res_line)


def wrap_location(val, lim):
    return lim - 2 if val == 0 else (1 if val == lim - 1 else val)


def move_bliz():
    global full_state, bliz
    for bz in bliz:
        full_state[bz[1]][bz[0]] -= 1
        bz[0] = wrap_location(bz[0] + bz[2], w)
        bz[1] = wrap_location(bz[1] + bz[3], h)
        full_state[bz[1]][bz[0]] += 1


def compress_last_state():
    global states, full_state
    res = []
    for y in range(h):
        item = 0
        for x in range(w):
            if full_state[y][x] != 0:
                item = item | (1 << x)
        res.append(item)
    states.append(res)


def get_state(after_min):
    while len(states) <= after_min:
        move_bliz()
        compress_last_state()
    return states[after_min]


def print_state(after_min):
    s = get_state(after_min)
    for y in range(h):
        line = ""
        for x in range(w):
            line += "#" if s[y] & (1 << x) else "."
        print(line)
    print("-" * (w + 3))


def find_min_path(start, end, t0):
    q = deque()
    q.append((start[1], start[0], t0))
    visited = {}

    while len(q) > 0:
        cur = q.popleft()
        if cur in visited:
            continue
        visited[cur] = True
        from_y, from_x, time_passed = cur
        if from_x == end[0] and from_y == end[1]:
            return time_passed
        time_passed += 1
        s = get_state(time_passed)
        for i in range(4):
            x = from_x + dx[i]
            y = from_y + dy[i]
            if x < 0 or x >= w or y < 0 or y >= h or (s[y] & (1 << x)):  # cell is not available
                continue
            q.append((y, x, time_passed))
        if not s[from_y] & (1 << from_x):
            q.append((from_y, from_x, time_passed))


def main():
    global h, w
    filename = "_input.txt"
    with open(filename) as file:
        input_lines = [line.rstrip() for line in file]
    h = len(input_lines)
    w = len(input_lines[0])

    build_full_state(input_lines)
    compress_last_state()

    start = (1, 0)
    end = (input_lines[-1].index("."), h - 1)

    start_time = time.time()
    t1 = find_min_path(start, end, 0)
    print("part1:", t1)
    t2 = find_min_path(start, end, find_min_path(end, start, t1))
    print("part2:", t2)

    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
