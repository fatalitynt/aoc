n = 10
start = (0, 0)
rope = [[start[0], start[1]] for i in range(n)]
dx = [0, 0, 1, -1]
dy = [1, -1, 0, 0]
direction_map = {'U': 1, 'D': 0, 'R': 2, 'L': 3}
dp = {(rope[0][0], rope[0][1]): 1}


def sign(v):
    return 0 if v == 0 else (1 if v > 0 else -1)


def get_fix(a, b):
    if abs(a[0] - b[0]) > 1 or abs(a[1] - b[1]) > 1:
        return [sign(a[0] - b[0]), sign(a[1] - b[1])]
    return [0, 0]


def add_point(a, b):
    a[0] += b[0]
    a[1] += b[1]


def step(direction):
    global rope
    direction_idx = direction_map[direction]
    rope[0][0] += dx[direction_idx]
    rope[0][1] += dy[direction_idx]
    for i in range(n - 1):
        add_point(rope[i + 1], get_fix(rope[i], rope[i + 1]))
    last = rope[-1]
    dp[(last[0], last[1])] = 1


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = list(map(lambda x: x.rstrip(), file))
        for line in lines:
            parts = line.split()
            dist = int(parts[1])
            for i in range(dist):
                step(parts[0])
    print(len(dp))


if __name__ == '__main__':
    main()

'''
What I learned?
- tuple can be a key for dict
'''
