import time
from collections import deque


def get_distance(a, b):
    if a[0] == b[0]:
        if a[1] == b[1]:
            return abs(a[2] - b[2])
        if a[2] == b[2]:
            return abs(a[1] - b[1])
    if a[1] == b[1] and a[2] == b[2]:
        return abs(a[0] - b[0])
    return 1000


def part1(box):
    n = len(box)
    con = [0] * n
    for i in range(n):
        for j in range(i + 1, n):
            if i == j:
                continue
            if get_distance(box[i], box[j]) == 1:
                con[i] += 1
                con[j] += 1
    res = 0
    for i in con:
        res += 6 - i
    return res


def ext(p, d):
    return p[0], p[1], p[2], d


def cut(pd):
    return pd[0], pd[1], pd[2]


def jn(a, b):
    return a[0] + b[0], a[1] + b[1], a[2] + b[2]


shift = [
    (0, 0, 1),
    (0, 1, 0),
    (1, 0, 0),
    (0, 0, -1),
    (0, -1, 0),
    (-1, 0, 0),
]

rings = [
    [(1, 0, 0), (0, 1, 0), (-1, 0, 0), (0, -1, 0)],  # d: 0 or 3
    [(1, 0, 0), (0, 0, 1), (-1, 0, 0), (0, 0, -1)],  # d: 1 or 4
    [(0, 1, 0), (0, 0, 1), (0, -1, 0), (0, 0, -1)],  # d: 2 or 5
]

shifted_d = [
    [5, 4, 2, 1],
    [5, 3, 2, 0],
    [4, 3, 1, 0],
]

same_box_d = [
    [2, 1, 5, 4],
    [2, 0, 5, 3],
    [1, 0, 4, 3],
]


def neighs(pd, dct):
    res = []
    p = cut(pd)
    d = pd[3]
    for i in range(4):
        p_ring = rings[d % 3][i]
        p_next = jn(p, p_ring)
        p_shifted = jn(p_next, shift[d])
        if p_shifted in dct:
            res.append(ext(p_shifted, shifted_d[d % 3][i]))
        elif p_next in dct:
            res.append(ext(p_next, d))
        else:
            res.append(ext(p, same_box_d[d % 3][i]))
    return res


def part2(dct):
    visited = {}
    q = deque()
    start = None
    for k in dct.keys():
        if (k[0], k[1], k[2] + 1) not in dct:
            start = k
            break
    q.append(ext(start, 0))
    while len(q) > 0:
        cur = q.popleft()
        visited[cur] = True
        for neigh in neighs(cur, dct):
            if neigh not in visited:
                q.append(neigh)
    return len(visited)


def main():
    filename = "_input.txt"
    with open(filename) as file:
        lines = [line.rstrip() for line in file]
    box = []
    dct = {}
    for line in lines:
        p = [int(x) for x in line.split(",")]
        pc = (p[0], p[1], p[2])
        box.append(pc)
        dct[pc] = True
        box.append(p)

    start_time = time.time()
    # part 1
    print(part1(box))
    # part 2
    print(part2(dct))
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- nothing
'''
