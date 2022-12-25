import time
from collections import deque

char_map = {'2': 2, '1': 1, '0': 0, '-': -1, '=': -2}
num_map = {2: '2', 1: '1', 0: '0', -1: '-', -2: '='}


def convert(line: str):
    res = 0
    base = 1
    for i in line:
        res += base * char_map[i]
        base *= 5
    return res


def solve(lines):
    idx = 0
    res = []
    while True:
        added = False
        for x in lines:
            if len(x) <= idx:
                continue
            added = True
            if len(res) == idx:
                res.append(0)
            res[idx] += char_map[x[idx]]
        idx += 1
        if not added:
            break

    for i in range(len(res) - 1):
        while res[i] >= 3:
            res[i] -= 5
            if len(res) == i + 1:
                res.append(0)
            res[i + 1] += 1
        while res[i] < -2:
            res[i] += 5
            if len(res) == i + 1:
                res.append(0)
            res[i + 1] -= 1

    print("".join(map(lambda r: num_map[r], res))[::-1])


def main():
    filename = "_input.txt"
    with open(filename) as file:
        input_lines = [line.rstrip()[::-1] for line in file]

    start_time = time.time()
    solve(input_lines)
    print("--- %s seconds ---" % (time.time() - start_time))


if __name__ == '__main__':
    main()

'''
What I learned?
- to reverse string: text[::-1]
'''
